﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WololoGame.Graphics;

namespace WololoGame.Logic
{
    public class PlayerLogic : LogicObjectBase, LogicObjectWithUpdate
    {
        protected Vector2 startingPoint;
        public Player player;
        IPhysicsObject po;
        public override void CollidedWith(ILogicObjectForPhysics otherObject)
        {
            var logicObject = otherObject as LogicObjectBase;
            if (logicObject != null)
            {
                if (logicObject.IsDeadly())
                {
                    Die();
                }
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
            player.SetMoveState(MoveState.TakingDamage);
        }

        public PlayerLogic(IPhysicsObject po)
        {
            this.po = po;
            startingPoint = new Vector2((float)po.X, (float)po.Y);
        }
    }
}
