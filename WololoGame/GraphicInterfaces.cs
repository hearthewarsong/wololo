using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WololoGame
{
    public class GraphicsObject : DrawableGameComponent
    {
        public GraphicsObject(Game game) :base(game)
        {

        }
        public float X { get; set; }
        public float Y { get; set; }
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
        public AnimatedGraphics(Game game) :base(game)
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
