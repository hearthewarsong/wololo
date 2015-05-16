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
        public static Texture2D playerTexture;
        public static SpriteSheetDescription sheetDescription;

        public Butterfly(Game game, IPhysicsObject po) : base(game, po)
        {
            moveState = State.Jumping;
            frameIndex = sheetDescription.jumpFrameIndices[0];
        }

        public bool facingLeft { get; set; }
    }
}
