using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WololoGame.Graphics
{
    public class Butterfly : AnimatedGraphics
    {
        public static Texture2D butterflyTexture_dark;
        public static Texture2D butterflyTexture_sunny;
        public static SpriteSheetDescription sheetDescription_dark;
        public static SpriteSheetDescription sheetDescription_sunny;
        public static Random randomGenerator;

        bool lastQueryOfIsNightModeProperty;
        int moveCounter;
        float ax;
        float ay;

        public Butterfly(Game game, IPhysicsObject po) : base(game, po)
        {
            moveState = MoveState.Jumping;
            frameIndex = sheetDescription_sunny.jumpFrameIndices[0];
            lastQueryOfIsNightModeProperty = false;
            facingLeft = randomGenerator.Next() % 2 == 0;
            moveCounter = 0;
        }

        public override void Draw(GameTime gameTime)
        {
            var screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            var screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            var aspectRatio = (float)screenWidth / screenHeight;
            var sheetDescription = GetSheetDescFromDerived();

            // Use lastQueryOfIsNightModeProperty instead of GlobalConfig.NightMode!!
            spriteBatch.Draw(GetTextureFromDerived(),
                 new Rectangle((int)(Position.X * screenHeight),
                     (int)(Position.Y * screenHeight),
                    (int)(Width * screenHeight),
                    (int)(Height * screenHeight)),
                 new Rectangle(frameIndex * sheetDescription.frameWidth, 0, sheetDescription.frameWidth, sheetDescription.frameHeight),
                 Color.White,
                 0.0f,
                 new Vector2(),
                 facingLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                 0.0f);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            int movecounterLimit = 24;
            if (moveCounter > movecounterLimit)
            {
                if (randomGenerator.Next(100) < 70)
                {
                    physicsObject.PVX = ((float)randomGenerator.NextDouble() - 0.5f) * 0.2128954f;
                    physicsObject.PVY = ((float)randomGenerator.NextDouble() - 0.5f) * 0.2128954f;
                }
                facingLeft = physicsObject.PVX < 0.0f;
                moveCounter = 0;
            }
            else
                moveCounter++;

        }

        public override SpriteSheetDescription GetSheetDescFromDerived()
        {
            lastQueryOfIsNightModeProperty = GlobalConfig.NightMode;
            return lastQueryOfIsNightModeProperty ? sheetDescription_dark : sheetDescription_sunny;
        }

        public override Texture2D GetTextureFromDerived()
        {
            return lastQueryOfIsNightModeProperty ? butterflyTexture_dark : butterflyTexture_sunny;
        }

        public override void HandleNightMode(bool nightMode)
        {
            jumpIndicator = 0;
            frameIndex = nightMode ? sheetDescription_dark.jumpFrameIndices[jumpIndicator] : sheetDescription_sunny.jumpFrameIndices[jumpIndicator];
            if (nightMode)
            {
                physicsObject.Width = Width = Width * 2.0f;
                physicsObject.Height = Height = Height * 2.0f;
            }
            else
            {
                physicsObject.Width = Width = Width * 0.5f;
                physicsObject.Height = Height = Height * 0.5f;
            }
        }
    }
}
