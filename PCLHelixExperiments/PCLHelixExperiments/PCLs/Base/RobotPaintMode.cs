using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftPainter.Core.Base
{
    [Serializable]
    public class RobotPaintMode
    {
        public bool IsStatic;
        public bool IsDefault;
        public bool IsInside;
        public bool IsFacade;
        public bool IsSide;

        public bool IsEmpty() { return (!IsStatic) && (!IsDefault) && (!IsInside) && (!IsFacade) && (!IsSide); }
    }
}
