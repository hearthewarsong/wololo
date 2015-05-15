using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WololoGame.Logic
{
    public class PlayerLogic : LogicObjectBase
    {
        public override void CollidedWith(ILogicObjectForPhysics otherObject)
        {
            var logicObject = otherObject as LogicObjectBase;
            if (logicObject != null)
            {
                if (logicObject.IsDeadly())
                    Logger.Get().Log("main", LogLevel.error, "Halál");
            }
        }
    }
}
