using Microsoft.Xna.Framework;
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
        public int jumpFrameCount;
        public int takingDamageFrameCount;

        public float runFrameTimespan;
        public float jumpFrameTimespan;
        public float takingDamageFrameTimespan;

        public List<int> runFrameIndices;
        public List<int> jumpFrameIndices;
        public List<int> takingDamageFrameIndices;
    }


    public class GraphicsObject : DrawableGameComponent
    {
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

        public State moveState { get; set; }
    }

    interface IGraphicsFactory
    {
        GraphicsObject getCharacter();
        GraphicsObject getTerrain(float width, float height);
    }
}
