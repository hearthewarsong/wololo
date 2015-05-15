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

        void createGrassyTerrain(Vector4 rec, Visibility v)
        {
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
            GrassyPlatform platform = new GrassyPlatform(this, (float)physicsObj.Width, (float)physicsObj.Height, new Vector2((float)physicsObj.X, (float)physicsObj.Y));
            Components.Add(platform);
        }
        void createPlayer(Vector4 rec)
        {

        }




        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //FPSCamera = new Camera {
            //    FOV = 60.0f,
            //    AspectRatio = (float)GraphicsDevice.PresentationParameters.BackBufferWidth / (float)GraphicsDevice.PresentationParameters.BackBufferHeight,
            //    NearPlane = 0.1f,
            //    FarPlane = 100.0f,
            //    Position = new Vector3(0.0f, 1.5f, -1.0f),
            //    Target = new Vector3(),
            //    Up = Vector3.UnitY };
            Logger.Get().SetLogLevel("main", LogLevel.warning);

            lastKeyboardState = new KeyboardState();
            MapLoader maploader = new MapLoader();

            maploader.LoadMap(this, "maps/level1.txt");
            
            Components.Add(new GrassyPlatform(this, 0.11f, 0.2f, new Vector2(0.05f, 0.75f), Visibility.NightModeOnly));
            Components.Add(new GrassyPlatform(this, 0.4f, 0.18f, new Vector2(0.666f, 0.75f)));

            SpriteSheetDescription desc = new SpriteSheetDescription();
            desc.jumpFrameCount = 1;
            desc.jumpFrameIndex = 4;
            desc.takingDamageFrameCount = 1;
            desc.takingDamageFrameIndex = 6;
            desc.runFrameCount = 3;
            desc.runFrameIndices = new List<int> { 0, 1, 2};
            desc.runFrameTimespan = 0.15f;

            player = new Player(this, 96.0f / GraphicsDevice.PresentationParameters.BackBufferWidth, 128.0f / GraphicsDevice.PresentationParameters.BackBufferHeight, desc, new Vector2(0.666f, 0.8f));
            Components.Add(player);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
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
            GrassyPlatform.spriteBatch = this.spriteBatch;

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

            if (currentState.IsKeyDown(Keys.Space) && lastKeyboardState.IsKeyUp(Keys.Space))
            {
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
            spriteBatch.Draw(GlobalConfig.NightMode ? backgroundDark : backgroundSunny,
                new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight),
                Color.White);
            spriteBatch.End();
 
            base.Draw(gameTime);
        }
    }
}
