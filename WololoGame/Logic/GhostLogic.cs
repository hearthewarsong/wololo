using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WololoGame.Logic
{
    public class GhostLogic : LogicObjectBase
    {
        public override void CollidedWith(ILogicObjectForPhysics otherObject)
        {
        }

        public override bool IsDeadly()
        {
            return GlobalConfig.NightMode;
        }
    }
}
