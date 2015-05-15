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

    public class GraphicsObject : DrawableGameComponent
    {
        public GraphicsObject(Game game, float width, float height, Vector2 pos = new Vector2()) :base(game)
        {
            Width = width;
            Height = height;
            Position = pos;
        }
        public Vector2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
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
        public struct SpriteSheetDescription
        {
            int runFrameCount;
            int jumpFrameCount;
            int takingDamageFrameCount;

            float runFrameTimespan;
            float jumpFrameTimespan;
            float takingDamageFrameTimespan;

            List<int> runFrameIndices;
            List<int> jumpFrameIndices;
            List<int> takingDamageFrameIndices;
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
