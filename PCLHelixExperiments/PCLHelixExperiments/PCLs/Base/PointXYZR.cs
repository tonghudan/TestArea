using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftPainter.Core.Base
{
    public class PointXYZR
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public double Rx { get; set; }
        public double Ry { get; set; }
        public double Rz { get; set; }

        public PointXYZR() { }

        public PointXYZR(double x, double y, double z, double rx, double ry, double rz)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Rx = rx;
            this.Ry = ry;
            this.Rz = rz;
        }
    }
}
