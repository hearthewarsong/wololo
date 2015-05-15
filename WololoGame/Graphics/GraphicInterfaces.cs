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
        public int jumpFrameTimespan;
        public List<int> jumpFrameIndices;

        public int takingDamageFrameCount;
        public int takingDamageFrameTimespan;
        public List<int> takingDamageFrameIndices;

        public int standingFrameIndex;
        public int frameHeight;
        public int frameWidth;
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
            Position = new Vector2( (float)po.X, (float)po.Y);
        }

        public Vector2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public IPhysicsObject physicsObject { get; set; }

    }

    public class AnimatedGraphics : GraphicsObject
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
        }
        public AnimatedGraphics(Game game, IPhysicsObject po) : base(game, po)
        {
        }

        public State moveState { get; set; }
    }

    interface IGraphicsFactory
    {
        GraphicsObject getCharacter();
        GraphicsObject getTerrain(float width, float height);
    }
}
