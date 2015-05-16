using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using WololoGame.Graphics;
using System;
using WololoGame.Logic;

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

        Texture2D backgroundDark;
        Texture2D backgroundSunny;

        Player player;

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
        GrassyPlatform playerPlatform;
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
                true,
                new PlayerLogic()
                );
            player = new Player(this, physicsObj);
            Components.Add(player);
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
            MapLoader maploader = new MapLoader();

            InitSpriteSheets();
            maploader.LoadMap(this, "Content/maps/beginning.txt");
            Components.Add(physics);
            base.Initialize();
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
            Player.sheetDescription.runFrameTimespan = 0.15f;
            Player.sheetDescription.standingFrameIndex = 6;
            Player.sheetDescription.frameHeight = 128;
            Player.sheetDescription.frameWidth = 96;
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

            backgroundDark = Content.Load<Texture2D>("images/background_dark");
            backgroundSunny = Content.Load<Texture2D>("images/background_sunny");

            GrassyPlatform.tex_dark_leftCorner = Content.Load<Texture2D>("images/grass_dark_left_corner");
            GrassyPlatform.tex_dark_middle = Content.Load<Texture2D>("images/grass_dark_middle");
            GrassyPlatform.tex_dark_rightCorner = Content.Load<Texture2D>("images/grass_dark_right_corner");
            GrassyPlatform.tex_dark_soil = Content.Load<Texture2D>("images/grass_dark_soil");
            GrassyPlatform.tex_sunny_leftCorner = Content.Load<Texture2D>("images/grass_sunny_left_corner");
            GrassyPlatform.tex_sunny_middle = Content.Load<Texture2D>("images/grass_sunny_middle");
            GrassyPlatform.tex_sunny_rightCorner = Content.Load<Texture2D>("images/grass_sunny_right_corner");
            GrassyPlatform.tex_sunny_soil = Content.Load<Texture2D>("images/grass_sunny_soil");

            Player.playerTexture = Content.Load<Texture2D>("images/player");

            Logger.Get().Log("main", LogLevel.warning, "LoadingContentFinished!"); 
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
        }

        private void HandleInput()
        {
            var currentState = Keyboard.GetState();

            if (currentState.IsKeyDown(Keys.H) && lastKeyboardState.IsKeyUp(Keys.H))
                GlobalConfig.NightMode = !GlobalConfig.NightMode;

            if (currentState.IsKeyDown(Keys.LeftAlt) && currentState.IsKeyDown(Keys.Enter) && lastKeyboardState.IsKeyUp(Keys.Enter))
                graphics.ToggleFullScreen();

            if (currentState.IsKeyDown(Keys.Space) && lastKeyboardState.IsKeyUp(Keys.Space))
            {
                if (!player.physicsObject.CantJump)
                {
                    player.physicsObject.PVY = -0.5;
                }
            }

            if (currentState.IsKeyDown(Keys.Left))
            {
                player.physicsObject.MoveIntentionX = -0.1;
                player.facingLeft = true;
            }

            if (currentState.IsKeyDown(Keys.Right))
            {
                player.physicsObject.MoveIntentionX = 0.1;
                player.facingLeft = false;
            }
            lastKeyboardState = currentState;
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
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
        }
    }
}
