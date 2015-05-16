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
            moveState = State.Running;
//            moveState = State.Standing;
//            frameIndex = sheetDescription.standingFrameIndex;
            frameIndex = sheetDescription.runFrameIndices[0];
        }


        public override void Update(GameTime gameTime)
        {
            double elapsedSeconds = gameTime.ElapsedGameTime.TotalSeconds;

            Position = new Vector2((float)physicsObject.X, (float)physicsObject.Y);
            frameTime += elapsedSeconds;
            int counter = 0;

            switch (moveState)
            {
                case State.Standing:
                    frameIndex = sheetDescription.standingFrameIndex;
                    break;
                case State.Running:
                    while (frameTime > sheetDescription.runFrameTimespan)
                    {
                        frameTime -= sheetDescription.runFrameTimespan;
                        counter++;
                    }
                    for (int i = 0; i < sheetDescription.runFrameCount; i++)
                    {
                        if (frameIndex == sheetDescription.runFrameIndices[i])
                        {
                            // Itt counter értéke lehet 0 is, ekkor tulképp ugyanaz marad a frameIndex
                            frameIndex = sheetDescription.runFrameIndices[(i + counter) % sheetDescription.runFrameCount];
                            break;
                        }
                    }
                    break;
                case State.Jumping:
                    while (frameTime > sheetDescription.jumpFrameTimespan)
                    {
                        frameTime -= sheetDescription.jumpFrameTimespan;
                        counter++;
                    }
                    for (int i = 0; i < sheetDescription.jumpFrameCount; i++)
                    {
                        if (frameIndex == sheetDescription.jumpFrameIndices[i])
                        {
                            frameIndex = sheetDescription.jumpFrameIndices[(i + counter) % sheetDescription.jumpFrameCount];
                            break;
                        }
                    }
                    break;
                case State.TakingDamage:
                    while (frameTime > sheetDescription.takingDamageFrameTimespan)
                    {
                        frameTime -= sheetDescription.takingDamageFrameTimespan;
                        counter++;
                    }
                    for (int i = 0; i < sheetDescription.takingDamageFrameCount; i++)
                    {
                        if (frameIndex == sheetDescription.takingDamageFrameIndices[i])
                        {
                            frameIndex = sheetDescription.takingDamageFrameIndices[(i + 1) % sheetDescription.takingDamageFrameCount];
                            break;
                        }
                    }
                    break;
            }

            base.Update(gameTime);
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
                 Color.White);
        }

        private double frameTime;
        private int frameIndex;
    }
}
