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
            moveState = State.Standing;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            switch(moveState)
            {
                case State.Standing:
                    break;
                case State.Running:
                    break;
                case State.Jumping:
                    break;
                case State.TakingDamage:
                    break;
            }
        }

        private int frameTime;
        private int frameIndex;

    }
}
