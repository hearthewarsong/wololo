using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WololoGame
{
    public enum Visibility
    {
        NightModeOnly,
        SunnyModeOnly,
        Both
    }

    public struct SpriteSheetDescription
    {
        public int runFrameCount;
        public float runFrameTimespan;
        public List<int> runFrameIndices;

        public int jumpFrameCount;
        public float jumpFrameTimespan;
        public List<int> jumpFrameIndices;

        public int takingDamageFrameCount;
        public float takingDamageFrameTimespan;
        public List<int> takingDamageFrameIndices;

        public int standingFrameIndex;
        public int frameHeight;
        public int frameWidth;
        public int totalFrameCount;
    }


    public class GraphicsObject : DrawableGameComponent
    {
        /// <summary>
        /// Used to draw the sprites
        /// </summary>
        public static SpriteBatch spriteBatch { get; set; }

        public GraphicsObject(Game game, float width, float height, Vector2 pos = new Vector2()) :base(game)
        {
            Width = width;
            Height = height;
            Position = pos;
        }
        public GraphicsObject(Game game, IPhysicsObject po) : base(game)
        {
            Width = (float)po.Width;
            Height = (float)po.Height;
            Position = new Vector2((float)po.X, (float)po.Y);
            this.physicsObject = po;
        }

        public Vector2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public IPhysicsObject physicsObject { get; set; }

    }

    public abstract class AnimatedGraphics : GraphicsObject
    {
        public enum State
        {
            Standing,
            Running,
            Jumping,
            TakingDamage
        }

        public AnimatedGraphics(Game game, float width, float height, Vector2 pos = new Vector2()) :
            base(game, width, height, pos)
        {
            frameTime = 0;
            runIndicator = 0;
            jumpIndicator = 0;
            takingDamageIndicator = 0;
        }
        public AnimatedGraphics(Game game, IPhysicsObject po) : base(game, po)
        {
            frameTime = 0;
            runIndicator = 0;
            jumpIndicator = 0;
            takingDamageIndicator = 0;
        }

        public override void Update(GameTime gameTime)
        {
            double elapsedSeconds = gameTime.ElapsedGameTime.TotalSeconds;
            Position = new Vector2((float)physicsObject.X, (float)physicsObject.Y);
            frameTime += elapsedSeconds;
            int counter = 0;
            var sheetDescription = GetSheetDescFromDerived();

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
                    if (counter > 0)
                    {
                        runIndicator = (runIndicator + counter) % sheetDescription.runFrameCount;
                        frameIndex = sheetDescription.runFrameIndices[runIndicator];
                    }
                    break;
                case State.Jumping:
                    while (frameTime > sheetDescription.jumpFrameTimespan)
                    {
                        frameTime -= sheetDescription.jumpFrameTimespan;
                        counter++;
                    }
                    if (counter > 0)
                    {
                        jumpIndicator = (jumpIndicator + counter) % sheetDescription.jumpFrameCount;
                        frameIndex = sheetDescription.jumpFrameIndices[jumpIndicator];
                    }
                    break;
                case State.TakingDamage:
                    while (frameTime > sheetDescription.takingDamageFrameTimespan)
                    {
                        frameTime -= sheetDescription.takingDamageFrameTimespan;
                        counter++;
                    }
                    if (counter > 0)
                    {
                        takingDamageIndicator = (takingDamageIndicator + counter) % sheetDescription.takingDamageFrameCount;
                        frameIndex = sheetDescription.takingDamageFrameIndices[takingDamageIndicator];
                    }
                    break;
            }

            base.Update(gameTime);
        }

        public abstract SpriteSheetDescription GetSheetDescFromDerived();
        public abstract Texture2D GetTextureFromDerived();

        public State moveState { get; set; }
        public bool facingLeft { get; set; }

        protected double frameTime;
        protected int frameIndex;
        protected int runIndicator;
        protected int jumpIndicator;
        protected int takingDamageIndicator;
    }

    interface IGraphicsFactory
    {
        GraphicsObject getCharacter();
        GraphicsObject getTerrain(float width, float height);
    }
}
