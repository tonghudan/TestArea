using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SoftPainter.Core.Base
{
    public class Translation
    {
        public Matrix<double> T { get; set; }

        public double Trx { get; set; }
        public double Try { get; set; }
        public double Trz { get; set; }

        public Translation(string bmpname)
        {
            string[] items = null;
            if (bmpname.EndsWith("bmp") || bmpname.EndsWith("jpg"))
                items = bmpname.Substring(0, bmpname.Length - 4).Split('#');
            else if (bmpname.EndsWith("jpeg"))
                items = bmpname.Substring(0, bmpname.Length - 5).Split('#');
            if (items == null || items.Length < 3) return;

            string[] t_xyz = items[1].Split('_');
            string[] r_xyz = items[2].Split('_');
            float tx = float.Parse(t_xyz[0]), ty = float.Parse(t_xyz[1]), tz = float.Parse(t_xyz[2]);
            float rx = float.Parse(r_xyz[0]), ry = float.Parse(r_xyz[1]), rz = float.Parse(r_xyz[2]);

            double A = Math.Cos(rz), B = Math.Sin(rz), C = Math.Cos(ry), D = Math.Sin(ry), E = Math.Cos(rx), F = Math.Sin(rx);
            Matrix<double> T1 = DenseMatrix.OfArray(new double[,]
            {
                {A*C, A*D*F-B*E, B*F+A*D*E, tx},
                {B*C, A*E-B*D*F, B*D*E-A*F, ty},
                {-D, C*F, C*E, tz},
                {0, 0, 0, 1 } });

            this.Trx = Math.Atan2(T1[2, 1], T1[2, 2]) / Math.PI * 180;
            this.Try = Math.Asin(-T1[2, 0]) / Math.PI * 180;
            this.Trz = Math.Atan2(T1[1, 0], T1[0, 0]) / Math.PI * 180;
            this.T = T1;
        }

        public Translation(string[] matrix)
        {
            if (matrix.Length != 16) return;

            Matrix<double> T1 = DenseMatrix.OfArray(new double[,]
            {
                {double.Parse(matrix[0]), double.Parse(matrix[1]), double.Parse(matrix[2]), double.Parse(matrix[3])},
                {double.Parse(matrix[4]), double.Parse(matrix[5]), double.Parse(matrix[6]), double.Parse(matrix[7])},
                {double.Parse(matrix[8]), double.Parse(matrix[9]), double.Parse(matrix[10]), double.Parse(matrix[11])},
                {double.Parse(matrix[12]), double.Parse(matrix[13]), double.Parse(matrix[14]), double.Parse(matrix[15])}
            });

            this.Trx = Math.Atan2(T1[2, 1], T1[2, 2]) / Math.PI * 180;
            this.Try = Math.Asin(-T1[2, 0]) / Math.PI * 180;
            this.Trz = Math.Atan2(T1[1, 0], T1[0, 0]) / Math.PI * 180;
            this.T = T1;
        }

        /// <summary>
        /// 把一个2D的坐标点转化为3D的坐标点
        /// </summary>
        /// <param name="bmpname"></param>
        /// <param name="sourceRPD"></param>
        /// <returns></returns>
        public PointXYZR ExecuteTrans(double x, double y, double z, double rx, double ry, double rz, bool isReverse = false)
        { 
            Vector<double> v = Vector<double>.Build.DenseOfArray(new double[] { x, y, z, 1.0 });

            if (isReverse)
            {
                Vector<double> newv = T.Inverse() * v;
                return new PointXYZR(newv[0], newv[1], newv[2], rx - Trx, ry - Try, rz - Trz);
            }
            else
            {
                Vector<double> newv = T * v;
                return new PointXYZR(newv[0], newv[1], newv[2], rx + Trx, ry + Try, rz + Trz);
            }
        }
    }
}
