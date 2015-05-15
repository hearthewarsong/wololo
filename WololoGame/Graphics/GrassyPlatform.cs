using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WololoGame.Graphics
{
    public class GrassyPlatform: GraphicsObject
    {
        public static Texture2D tex_sunny_leftCorner;
        public static Texture2D tex_sunny_rightCorner;
        public static Texture2D tex_sunny_middle;
        public static Texture2D tex_sunny_soil;
        public static Texture2D tex_dark_leftCorner;
        public static Texture2D tex_dark_rightCorner;
        public static Texture2D tex_dark_middle;
        public static Texture2D tex_dark_soil;

        public Visibility VisibilityMode { get; set; }

        public GrassyPlatform(Game1 game, float width, float height, Vector2 pos = new Vector2(), Visibility m = Visibility.Both) :
            base(game, width, height, pos)
        {
            VisibilityMode = m;
        }
        public GrassyPlatform(Game game, IPhysicsObject po, Visibility m) : base(game, po)
        {
            VisibilityMode = m;
        }

        public override void Draw(GameTime gameTime)
        {
            if (GlobalConfig.NightMode && VisibilityMode == Visibility.SunnyModeOnly ||
                !GlobalConfig.NightMode && VisibilityMode == Visibility.NightModeOnly)
                return;

            var screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            var screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            var aspectRatio = (float)screenWidth / screenHeight;

//            spriteBatch.Begin();

            spriteBatch.Draw(GlobalConfig.NightMode ? tex_dark_soil : tex_sunny_soil,
                new Rectangle((int)(Position.X * screenWidth),
                    (int)(Position.Y * screenHeight) + 20,
                    (int)(Width * screenWidth),
                    (int)(Height * screenHeight)),
                Color.White);

            spriteBatch.Draw(GlobalConfig.NightMode ? tex_dark_leftCorner : tex_sunny_leftCorner,
                new Rectangle((int)(Position.X * screenWidth),
                    (int)(Position.Y * screenHeight),
                    (int)(screenWidth / (24 * aspectRatio)),
                    screenHeight / 24),
                Color.White);

            float i = (0.9f / 24.0f);
            for (; i < (Width - 0.9f / 24.0f); i += (0.95f / 48.0f))
            {
                spriteBatch.Draw(GlobalConfig.NightMode ? tex_dark_middle : tex_sunny_middle,
                    new Rectangle((int)((Position.X + i) * screenWidth),
                        (int)(Position.Y * screenHeight),
                        (int)(screenWidth / (48 * aspectRatio)),
                        screenHeight / 24),
                    Color.White);
            }

            spriteBatch.Draw(GlobalConfig.NightMode ? tex_dark_rightCorner : tex_sunny_rightCorner,
                new Rectangle((int)((Position.X + i) * screenWidth),
                    (int)(Position.Y * screenHeight),
                    (int)(screenWidth / (24 * aspectRatio)),
                    screenHeight / 24),
                Color.White);

     //       spriteBatch.End();
        }
    }
}
