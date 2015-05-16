using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WololoGame.Logic
{
    public class PlayerLogic : LogicObjectBase, LogicObjectWithUpdate
    {
        protected Vector2 startingPoint;
        IPhysicsObject po;
        public override void CollidedWith(ILogicObjectForPhysics otherObject)
        {
            var logicObject = otherObject as LogicObjectBase;
            if (logicObject != null)
            {
                if (logicObject.IsDeadly())
                    Logger.Get().Log("main", LogLevel.error, "Halál");
                    Die();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (po.Y> 10)
            {
                Die();
            }
        }

        protected void Die()
        {
            po.X = startingPoint.X;
            po.Y = startingPoint.Y;
            po.PVX = 0;
            po.PVY = 0;
        }

        public PlayerLogic(IPhysicsObject po)
        {
            this.po = po;
            startingPoint = new Vector2((float)po.X, (float)po.Y);
        }
    }
}
