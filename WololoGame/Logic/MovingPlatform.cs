using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using WololoGame.Graphics;

namespace WololoGame.Logic
{
    class MovingPlatform : LogicObjectBase, LogicObjectWithUpdate
    {
        private double maxX, minX, speed;
        IPhysicsObject po;
        double dir = 1;
        GrassyPlatform platform;
        public override void CollidedWith(ILogicObjectForPhysics otherObject)
        {
            
        }
        public MovingPlatform(double minX,double maxX, double speed, IPhysicsObject po, GrassyPlatform platform)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.speed = speed;
            this.po = po;
            this.platform = platform;
        }

        public void Update(GameTime gameTime)
        {
            if (dir == -1 && minX >= po.X)
            {
                dir = 1;
            }
            if (dir == 1 && maxX <= po.X)
            {
                dir = -1;
            }
            po.MoveIntentionX = speed * dir;
            platform.Position = new Vector2((float)po.X, (float)po.Y);
        }
    }
}
