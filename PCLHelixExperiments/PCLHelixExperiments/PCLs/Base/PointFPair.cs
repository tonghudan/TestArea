using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SoftPainter.Core.Base
{
    public class PointFPair : IComparable
    {
        public PointF SlipP1 { get; set; }
        public PointF P1 { get; set; }
        public PointF P2 { get; set; }
        public PointF SlipP2 { get; set; }
        public double Angle { get; set; }
        public PointFPair AccPointPair { get; set; }

        public PointXYZR PP_SlipP1 { get; set; }
        public PointXYZR PP_P1 { get; set; }
        public PointXYZR PP_P2 { get; set; }
        public PointXYZR PP_SlipP2 { get; set; }

        /// <summary>
        /// 元素比较时基于的值
        /// </summary>
        private double CompareValue { get; set; }

        public PointFPair(PointF _p1, PointF _p2, double _compareValue = 0)
        {
            this.P1 = new PointF(_p1.X, _p1.Y);
            this.P2 = new PointF(_p2.X, _p2.Y);
            this.CompareValue = _compareValue;
        }

        public PointFPair(PointF _p1, PointF _p2, PointF _slipP1, PointF _slipP2, double _angle, double _compareValue = 0)
        {
            this.P1 = new PointF(_p1.X, _p1.Y);
            this.P2 = new PointF(_p2.X, _p2.Y);
            this.SlipP1 = new PointF(_slipP1.X, _slipP1.Y);
            this.SlipP2 = new PointF(_slipP2.X, _slipP2.Y);
            this.Angle = _angle;
            this.CompareValue = _compareValue;
        }

        public int CompareTo(Object obj)
        {
            PointFPair other = obj as PointFPair;
            if (this.CompareValue > other.CompareValue) return 1;
            if (this.CompareValue < other.CompareValue) return -1;
            return 0;
        }
    }
}
