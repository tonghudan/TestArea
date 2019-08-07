using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftPainter.Core.Base
{
    public class Line
    {
        public const double PARALLEL_ANGLE_LIMIT = 5 * Math.PI / 180;

        public PointF P1 { get; set; }
        public PointF P2 { get; set; }

        private double A { get; set; }
        private double B { get; set; }
        private double C { get; set; }

        public Line(PointF _pt0, PointF _pt1)
        {
            this.P1 = _pt0;
            this.P2 = _pt1;

            double x_dis = P1.X - P2.X;
            if (x_dis == 0)
            {
                this.A = 1;
                this.B = 0;
                this.C = -P1.X;
            }
            else
            {
                double k = (P1.Y - P2.Y) / (P1.X - P2.X);

                this.A = k;
                this.B = -1;
                this.C = P1.Y - k * P1.X;
            }
        }

        public Line(Line _old)
        {
            this.P1 = new PointF(_old.P1.X, _old.P1.Y);
            this.P2 = new PointF(_old.P2.X, _old.P2.Y);
            this.A = _old.A;
            this.B = _old.B;
            this.C = _old.C;
        }

        public Line()
        {
            this.P1 = PointF.Empty;
            this.P2 = PointF.Empty;
        }

        /// <summary>
        /// 得到两条平行线之间的距离 
        /// </summary>
        /// <param name="line0"></param>
        /// <param name="line1"></param>
        /// <param name="distance"></param>
        /// <returns>line0和line1平行则返回true，否则返回false</returns>
        public static bool GetDisOfParallelLines(Line line0, Line line1, out double distance)
        {
            distance = 0;

            if (!IsParallelLines(line0, line1))
                return false;

            //中点
            PointF midPoint = new PointF((line0.P1.X + line0.P2.X) / 2, (line0.P1.Y + line0.P2.Y) / 2);
            //点到直线距离公式
            distance = Math.Abs(line1.A * midPoint.X + line1.B * midPoint.Y + line1.C) / Math.Sqrt(line1.A * line1.A + line1.B * line1.B);
            return true;
        }

        /// <summary>
        /// 判断两直线是否平行
        /// </summary>
        /// <param name="line0"></param>
        /// <param name="line1"></param>
        /// <returns></returns>
        public static bool IsParallelLines(Line line0, Line line1)
        {
            double cosTheta = Math.Abs(line0.A * line1.A + line0.B * line1.B) / (Math.Sqrt(line0.A * line0.A + line0.B * line0.B) * Math.Sqrt(line1.A * line1.A + line1.B * line1.B));

            double theta = Math.Acos(cosTheta);
            if (theta < PARALLEL_ANGLE_LIMIT)
                return true;

            return false;
        }
    }
}
