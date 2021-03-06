﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WololoGame.Logic
{
    public abstract class LogicObjectBase : ILogicObjectForPhysics
    {
        public abstract void CollidedWith(ILogicObjectForPhysics otherObject);

        public virtual bool IsDeadly()
        {
            return false;
        }
    }

    public interface LogicObjectWithUpdate
    {
        void Update(GameTime gameTime);
    }
}
