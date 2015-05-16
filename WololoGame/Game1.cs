using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using WololoGame.Graphics;
using System;
using WololoGame.Logic;
using Microsoft.Xna.Framework.Audio;

namespace WololoGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState lastKeyboardState;
        PhysicalEngine physics;
        /// <summary>
        /// The sound clip played as background music
        /// </summary>
        private static SoundEffect bgMusic;
        private static SoundEffectInstance bgMusicInstance;

        Texture2D backgroundDark;
        Texture2D backgroundSunny;

        private Effect bloom_effect;
        private RenderTarget2D texTarget;
        private RenderTarget2D downsampledTexTarget;
        private RenderTarget2D blurredTexTarget;
        private RenderTarget2D upsampledTexTarget;
        private VertexBuffer post_vertexBuffer_screen;
        // screen-sized quad stretched to fit the screen
        private IndexBuffer post_indexBuffer;
        // index buffer for the screen-sized quad

        Player player;

        List<LogicObjectWithUpdate> loUpdates = new List<LogicObjectWithUpdate>();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            physics = new PhysicalEngine(this);
            Content.RootDirectory = "Content";
        }

        public void CreateGrassyTerrain(Vector4 rec, Visibility v)
        {
            IPhysicsObject physicsObj = physics.AbbObject(
                rec.X,
                rec.Y,
                rec.Z,
                rec.W
                );
            GrassyPlatform platform = new GrassyPlatform(this, physicsObj, v);
            Components.Add(platform);
        }
        public void CreatePlayer(Vector4 rec)
        {
            if (player != null)
                throw new Exception("Error! Only one player is allowed!");

            IPhysicsObject physicsObj = physics.AbbObject(
                rec.X,
                rec.Y,
                rec.Z,
                rec.W,
                true,
                true,
                true
                );
            PlayerLogic pl = new PlayerLogic(physicsObj);
            physicsObj.LogicObject = pl;
            loUpdates.Add(pl);
            player = new Player(this, physicsObj);
            pl.player = player;
            Components.Add(player);
        }
        public void CreateButterfly(Vector4 rec)
        {
            IPhysicsObject physicsObj = physics.AbbObject(
                rec.X,
                rec.Y,
                rec.Z,
                rec.W,
                false,
                false,
                false,
                new GhostLogic()                
                );
            Components.Add(new Butterfly(this, physicsObj));
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Logger.Get().SetLogLevel("main", LogLevel.warning);

            lastKeyboardState = new KeyboardState();
            Butterfly.randomGenerator = new Random();
            MapLoader maploader = new MapLoader();

            InitSpriteSheets();
            maploader.LoadMap(this, "Content/maps/beginning.txt");
            Components.Add(physics);

            texTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth16);
            downsampledTexTarget = new RenderTarget2D(GraphicsDevice, (int)(GraphicsDevice.PresentationParameters.BackBufferWidth / 2.0f), (int)(GraphicsDevice.PresentationParameters.BackBufferHeight / 2.0f));
            blurredTexTarget = new RenderTarget2D(GraphicsDevice, (int)(GraphicsDevice.PresentationParameters.BackBufferWidth / 2.0f), (int)(GraphicsDevice.PresentationParameters.BackBufferHeight / 2.0f));
            upsampledTexTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);

            Window.ClientSizeChanged += (s, e) =>
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                graphics.ApplyChanges();

                // Prepare the render targets used for post effects
                texTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth16);
                downsampledTexTarget = new RenderTarget2D(GraphicsDevice, (int)(GraphicsDevice.PresentationParameters.BackBufferWidth / 2.0f), (int)(GraphicsDevice.PresentationParameters.BackBufferHeight / 2.0f));
                blurredTexTarget = new RenderTarget2D(GraphicsDevice, (int)(GraphicsDevice.PresentationParameters.BackBufferWidth / 2.0f), (int)(GraphicsDevice.PresentationParameters.BackBufferHeight / 2.0f));
                upsampledTexTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            };

            var vertices = new VertexPositionTexture[4];
            post_vertexBuffer_screen = new VertexBuffer(GraphicsDevice, VertexPositionTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            vertices[0] = new VertexPositionTexture() { Position = new Vector3(1, 1, 0), TextureCoordinate = new Vector2(1, 0) };
            vertices[1] = new VertexPositionTexture() { Position = new Vector3(1, -1, 0), TextureCoordinate = new Vector2(1, 1) };
            vertices[2] = new VertexPositionTexture() { Position = new Vector3(-1, 1, 0), TextureCoordinate = new Vector2(0, 0) };
            vertices[3] = new VertexPositionTexture() { Position = new Vector3(-1, -1, 0), TextureCoordinate = new Vector2(0, 1) };
            post_vertexBuffer_screen.SetData<VertexPositionTexture>(vertices);

            var indices = new ushort[4];
            for (ushort i = 0; i < (ushort)indices.Length; i++) indices[i] = i;
            post_indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);

            base.Initialize();
        }

        public void CreateMovingGrassyTerrain(Vector4 rec, Visibility v, float minX, float maxX, float speed)
        {
            IPhysicsObject physicsObj = physics.AbbObject(
                rec.X,
                rec.Y,
                rec.Z,
                rec.W
                );
            GrassyPlatform platform = new GrassyPlatform(this, physicsObj, v);
            MovingPlatform logic = new MovingPlatform(minX, maxX, speed, physicsObj, platform);
            Components.Add(platform);
            loUpdates.Add(logic);
        }

        void InitSpriteSheets()
        {
            Player.sheetDescription = new SpriteSheetDescription();
            Player.sheetDescription.jumpFrameCount = 1;
            Player.sheetDescription.jumpFrameIndices = new List<int> { 2 };
            Player.sheetDescription.jumpFrameTimespan = 100000;    // így sose vált
            Player.sheetDescription.takingDamageFrameCount = 1;
            Player.sheetDescription.takingDamageFrameIndices = new List<int> { 0 };
            Player.sheetDescription.takingDamageFrameTimespan = 100000;    // így sose vált
            Player.sheetDescription.runFrameCount = 3;
            Player.sheetDescription.runFrameIndices = new List<int> { 6, 5, 4, 5 };
            Player.sheetDescription.runFrameTimespan = 0.12f;
            Player.sheetDescription.standingFrameIndex = 6;
            Player.sheetDescription.frameHeight = 128;
            Player.sheetDescription.frameWidth = 96;

            Butterfly.sheetDescription_dark = new SpriteSheetDescription();
            Butterfly.sheetDescription_dark.jumpFrameCount = 9;
            Butterfly.sheetDescription_dark.jumpFrameIndices = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            Butterfly.sheetDescription_dark.jumpFrameTimespan = 0.6f;
            Butterfly.sheetDescription_dark.takingDamageFrameCount = 1;
            Butterfly.sheetDescription_dark.takingDamageFrameIndices = new List<int> { 0 };
            Butterfly.sheetDescription_dark.takingDamageFrameTimespan = 100000;    // így sose vált
            Butterfly.sheetDescription_dark.runFrameCount = 1;
            Butterfly.sheetDescription_dark.runFrameIndices = new List<int> { 0 };
            Butterfly.sheetDescription_dark.runFrameTimespan = 100000;  // így sose vált
            Butterfly.sheetDescription_dark.standingFrameIndex = 0;
            Butterfly.sheetDescription_dark.frameHeight = 97;
            Butterfly.sheetDescription_dark.frameWidth = 100;

            Butterfly.sheetDescription_sunny = new SpriteSheetDescription();
            Butterfly.sheetDescription_sunny.jumpFrameCount = 10;
            Butterfly.sheetDescription_sunny.jumpFrameIndices = new List<int> { 0, 1, 2, 3, 4, 5, 4, 3, 2, 1 };
            Butterfly.sheetDescription_sunny.jumpFrameTimespan = 0.055f;
            Butterfly.sheetDescription_sunny.takingDamageFrameCount = 1;
            Butterfly.sheetDescription_sunny.takingDamageFrameIndices = new List<int> { 0 };
            Butterfly.sheetDescription_sunny.takingDamageFrameTimespan = 100000;    // így sose vált
            Butterfly.sheetDescription_sunny.runFrameCount = 1;
            Butterfly.sheetDescription_sunny.runFrameIndices = new List<int> { 0 };
            Butterfly.sheetDescription_sunny.runFrameTimespan = 100000; // így sose vált
            Butterfly.sheetDescription_sunny.standingFrameIndex = 0;
            Butterfly.sheetDescription_sunny.frameHeight = 80;
            Butterfly.sheetDescription_sunny.frameWidth = 80;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsObject.spriteBatch = spriteBatch;
            bgMusic = Content.Load<SoundEffect>("sounds/bgmusic");
            bgMusicInstance = bgMusic.CreateInstance();
            bgMusicInstance.IsLooped = true;
            bgMusicInstance.Play();

            backgroundDark = Content.Load<Texture2D>("images/background_dark");
            backgroundSunny = Content.Load<Texture2D>("images/background_sunny");
            bloom_effect = Content.Load<Effect>("post_effects/bloom");

            GrassyPlatform.tex_dark_leftCorner = Content.Load<Texture2D>("images/grass_dark_left_corner");
            GrassyPlatform.tex_dark_middle = Content.Load<Texture2D>("images/grass_dark_middle");
            GrassyPlatform.tex_dark_rightCorner = Content.Load<Texture2D>("images/grass_dark_right_corner");
            GrassyPlatform.tex_dark_soil = Content.Load<Texture2D>("images/grass_dark_soil");
            GrassyPlatform.tex_sunny_leftCorner = Content.Load<Texture2D>("images/grass_sunny_left_corner");
            GrassyPlatform.tex_sunny_middle = Content.Load<Texture2D>("images/grass_sunny_middle");
            GrassyPlatform.tex_sunny_rightCorner = Content.Load<Texture2D>("images/grass_sunny_right_corner");
            GrassyPlatform.tex_sunny_soil = Content.Load<Texture2D>("images/grass_sunny_soil");

            Player.playerTexture = Content.Load<Texture2D>("images/player");

            Butterfly.butterflyTexture_dark = Content.Load<Texture2D>("images/butterfly_dark");
            Butterfly.butterflyTexture_sunny = Content.Load<Texture2D>("images/butterfly_sunny");

            Logger.Get().Log("main", LogLevel.warning, "LoadingContentFinished!"); 
        }

        private void BlurHighlights(RenderTarget2D source)
        {
            // Apply horizontal blur
            GraphicsDevice.SetRenderTarget(downsampledTexTarget);
            GraphicsDevice.SetVertexBuffer(post_vertexBuffer_screen);
            GraphicsDevice.Indices = post_indexBuffer;
            bloom_effect.Parameters["originalTex"].SetValue(source);
            bloom_effect.Parameters["threshold"].SetValue(0.3f);
            bloom_effect.Parameters["screenSize"].SetValue(new Vector2(source.Width, source.Height));
            bloom_effect.Techniques[0].Passes[0].Apply();
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

            // Apply vertical blur
            GraphicsDevice.SetRenderTarget(blurredTexTarget);
            bloom_effect.Parameters["originalTex"].SetValue(downsampledTexTarget);
            bloom_effect.Parameters["screenSize"].SetValue(new Vector2(downsampledTexTarget.Width, downsampledTexTarget.Height));
            bloom_effect.Techniques[0].Passes[1].Apply();
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

            // Upsample the blurred texture and either present it on the screen or render it to a texture for further use
            GraphicsDevice.SetRenderTarget(upsampledTexTarget);
            bloom_effect.Parameters["originalTex"].SetValue(blurredTexTarget);
            bloom_effect.Parameters["screenSize"].SetValue(new Vector2(blurredTexTarget.Width, blurredTexTarget.Height));
            bloom_effect.Techniques[0].Passes[2].Apply();
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            HandleInput();
            base.Update(gameTime);
            foreach (var item in loUpdates)
            {
                item.Update(gameTime);
            }
        }

        private void HandleInput()
        {
            var currentState = Keyboard.GetState();

            if (currentState.IsKeyDown(Keys.Space) && lastKeyboardState.IsKeyUp(Keys.Space))
            {
                GlobalConfig.NightMode = !GlobalConfig.NightMode;
                foreach (var c in Components)
                {
                    if (c is GraphicsObject)
                    {
                        var go = c as GraphicsObject;
                        go.HandleNightMode(GlobalConfig.NightMode);
                    }
                }
            }

            if (currentState.IsKeyDown(Keys.F) && lastKeyboardState.IsKeyUp(Keys.F))
                ToggleFullScreen();

            if (currentState.IsKeyDown(Keys.Up) && lastKeyboardState.IsKeyUp(Keys.Up))
            {
                if (!player.physicsObject.CantJump)
                {
                    player.physicsObject.PVY = -1.0;
                }
            }

            if (currentState.IsKeyDown(Keys.Left))
            {
                player.physicsObject.MoveIntentionX = -0.4;
                player.facingLeft = true;
            }

            if (currentState.IsKeyDown(Keys.Right))
            {
                player.physicsObject.MoveIntentionX = 0.4;
                player.facingLeft = false;
            }

            if (player.physicsObject.CantJump)
                player.SetMoveState(MoveState.Jumping);
            else
            {
                if (currentState.IsKeyUp(Keys.Left) && currentState.IsKeyUp(Keys.Right))
                    player.SetMoveState(MoveState.Standing);
                else
                    player.SetMoveState(MoveState.Running);
            }

            lastKeyboardState = currentState;
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            // Render to texture
            GraphicsDevice.SetRenderTarget(texTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            var texture = GlobalConfig.NightMode ? backgroundDark : backgroundSunny;
            float heightRatio = Math.Min((texture.Width / (float)texture.Height) * (GraphicsDevice.PresentationParameters.BackBufferHeight / (float)GraphicsDevice.PresentationParameters.BackBufferWidth), 1.0f);
            float widthRatio = Math.Min((texture.Height / (float)texture.Width) * (GraphicsDevice.PresentationParameters.BackBufferWidth / (float)GraphicsDevice.PresentationParameters.BackBufferHeight), 1.0f);
            spriteBatch.Draw(texture,
                new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight),
                new Rectangle(
                    0,
                    0,
                    (int)(widthRatio * texture.Width),
                    (int)(heightRatio * texture.Height)
                    ),
                Color.White);
            base.Draw(gameTime);
            spriteBatch.End();

            // Apply blur effect on the highlighted areas (areas with luminance above a certain threshold)
            BlurHighlights(texTarget);

            // Switch to back buffer and set the screen-sized quad VB/IB
            GraphicsDevice.SetRenderTarget(null);

            // Apply the post effect and present it on the screen
            bloom_effect.Parameters["originalTex"].SetValue(texTarget);
            bloom_effect.Parameters["blurredTex"].SetValue(upsampledTexTarget);
            bloom_effect.Parameters["originalIntensity"].SetValue(1.0f);
            bloom_effect.Parameters["bloomIntensity"].SetValue(1.3f);
            bloom_effect.Parameters["originalSaturation"].SetValue(1.0f);
            bloom_effect.Parameters["bloomSaturation"].SetValue(1.0f);
            bloom_effect.Techniques[0].Passes[3].Apply();
            GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);

        }

        private void ToggleFullScreen()
        {
            graphics.ToggleFullScreen();
            // Prepare the render targets used for post effects
            texTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth16);
            downsampledTexTarget = new RenderTarget2D(GraphicsDevice, (int)(GraphicsDevice.PresentationParameters.BackBufferWidth / 2.0f), (int)(GraphicsDevice.PresentationParameters.BackBufferHeight / 2.0f));
            blurredTexTarget = new RenderTarget2D(GraphicsDevice, (int)(GraphicsDevice.PresentationParameters.BackBufferWidth / 2.0f), (int)(GraphicsDevice.PresentationParameters.BackBufferHeight / 2.0f));
            upsampledTexTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
        }
        /// <summary>
        /// Stops the sound effects.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (null != bgMusicInstance && false == bgMusicInstance.IsDisposed)
                bgMusicInstance.Stop();

            base.Dispose(disposing);
        }
    }
}
