using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WololoGame.Graphics
{
    public class Player : AnimatedGraphics
    {
        public static Texture2D playerTexture;
        public static SpriteSheetDescription sheetDescription;

        public Player(Game game, IPhysicsObject po) : base(game, po)
        {
            moveState = MoveState.Standing;
            frameIndex = sheetDescription.standingFrameIndex;
            facingLeft = true;
        }

        public override void Draw(GameTime gameTime)
        {
            var screenWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            var screenHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            var aspectRatio = (float)screenWidth / screenHeight;

            spriteBatch.Draw(playerTexture,
                 new Rectangle((int)(Position.X * screenHeight),
                     (int)(Position.Y * screenHeight),
                    (int)(Width * screenHeight),
                    (int)(Height * screenHeight)),
                 new Rectangle(0, frameIndex * sheetDescription.frameHeight, sheetDescription.frameWidth, sheetDescription.frameHeight),
                 Color.White,
                 0.0f,
                 new Vector2(),
                 facingLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                 0.0f);
        }


        public override SpriteSheetDescription GetSheetDescFromDerived()
        {
            return sheetDescription;
        }

        public override Texture2D GetTextureFromDerived()
        {
            return playerTexture;
        }
    }
}
