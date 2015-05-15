using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        Texture2D background;
        Model stickFigure;

        Matrix World;
        Camera FPSCamera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            FPSCamera = new Camera {
                FOV = 60.0f,
                AspectRatio = (float)GraphicsDevice.PresentationParameters.BackBufferWidth / (float)GraphicsDevice.PresentationParameters.BackBufferHeight,
                NearPlane = 0.1f,
                FarPlane = 100.0f,
                Position = new Vector3(0.0f, 1.5f, -1.0f),
                Target = new Vector3(),
                Up = Vector3.UnitY };

            World = Matrix.CreateWorld(new Vector3(), Vector3.UnitZ, Vector3.UnitY);
            lastKeyboardState = new KeyboardState();
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

            background = Content.Load<Texture2D>("images/backgroujnd1");
            stickFigure = Content.Load<Model>("models/figura");
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
            var currentState = Keyboard.GetState();
            if (currentState.IsKeyDown(Keys.W) && lastKeyboardState.IsKeyUp(Keys.W))
                FPSCamera.Position.Z += 0.02f;

            if (currentState.IsKeyDown(Keys.S) && lastKeyboardState.IsKeyUp(Keys.S))
                FPSCamera.Position.Z -= 0.02f;

            if (currentState.IsKeyDown(Keys.A) && lastKeyboardState.IsKeyUp(Keys.A))
                FPSCamera.Position.X += 0.02f;

            if (currentState.IsKeyDown(Keys.D) && lastKeyboardState.IsKeyUp(Keys.D))
                FPSCamera.Position.X -= 0.02f;

            lastKeyboardState = currentState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background,
                new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight),
                Color.AliceBlue);
            spriteBatch.End();

            foreach (var mesh in stickFigure.Meshes)
            {
                foreach (BasicEffect e in mesh.Effects)
                {
                    e.World = World;
                    e.View = FPSCamera.View;
//                    e.Projection = FPSCamera.Projection;
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
