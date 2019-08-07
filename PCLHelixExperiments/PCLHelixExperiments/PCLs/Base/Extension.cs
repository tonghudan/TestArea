using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SoftPainter.Core.Base
{
    public static class Extension
    {
        public static Point ToPoint(this PointF pointF)
        {
            return new Point((int)pointF.X, (int)pointF.Y);
        } 

        public static Size ToSize(this SizeF sizeF)
        {
            return new Size((int)sizeF.Width, (int)sizeF.Height);
        }

        public static PointF ToPointF(this Vector<double> vector)
        {
            return new PointF((float)vector[0], (float)vector[1]);
        }

        public static Vector<double> ToMathNetVector(this PointF pointF, bool isAppend = false)
        {
            if (isAppend)
                return Vector<double>.Build.DenseOfArray(new double[] { pointF.X, pointF.Y, 1.0 });
            else
                return Vector<double>.Build.DenseOfArray(new double[] { pointF.X, pointF.Y });
        }

        public static Matrix<double> ToMathNetMatrix(this Emgu.CV.Mat mat, bool isAppend = false)
        {
            Emgu.CV.Matrix<double> matrix = new Emgu.CV.Matrix<double>(mat.Rows, mat.Cols);
            mat.CopyTo(matrix);

            double[,] array;
            if (isAppend)
                array = new double[mat.Rows + 1, mat.Cols];
            else
                array = new double[mat.Rows, mat.Cols];

            for (int i = 0; i < mat.Rows; i++)
            {
                for (int j = 0; j < mat.Cols; j++)
                {
                    array[i, j] = matrix[i, j];
                }
            }

            if (isAppend)
            {
                for (int j = 0; j < mat.Cols - 1; j++)
                    array[mat.Rows, j] = 0;
                array[mat.Rows, mat.Cols - 1] = 1;
            }

            return DenseMatrix.OfArray(array);
        }

        public static Emgu.CV.Util.VectorOfPoint ToVectorOfPoint(this Emgu.CV.Util.VectorOfPointF vectorOfPointF)
        {
            List<Point> pts = new List<Point>();
            for (int i = 0; i < vectorOfPointF.Size; i++)
                pts.Add(Point.Round(vectorOfPointF[i]));
            return new Emgu.CV.Util.VectorOfPoint(pts.ToArray());
        }

        public static Emgu.CV.Util.VectorOfPoint ToClockwise(this Emgu.CV.Util.VectorOfPoint vectorOfPoint)
        {
            var array = new List<Point>(vectorOfPoint.ToArray());
            array.Reverse();

            if (Emgu.CV.CvInvoke.ContourArea(vectorOfPoint, true) < 0)
                return new Emgu.CV.Util.VectorOfPoint(array.ToArray());
            return vectorOfPoint;
        }

        public static Point[] ToArray(this Emgu.CV.Util.VectorOfVectorOfPoint vectorOfVectorOfPoint)
        {
            List<Point> insidePoints = new List<Point>();
            for (int i = 0; i < vectorOfVectorOfPoint.Size; i++)
                    insidePoints.AddRange(vectorOfVectorOfPoint[i].ToArray());
            return insidePoints.ToArray();
        }

        public static PointF[] ToArrayF(this Emgu.CV.Util.VectorOfVectorOfPoint vectorOfVectorOfPoint)
        {
            List<PointF> insidePoints = new List<PointF>();
            for (int i = 0; i < vectorOfVectorOfPoint.Size; i++)
                for (int j = 0; j < vectorOfVectorOfPoint[i].Size; j++)
                    insidePoints.Add(vectorOfVectorOfPoint[i][j]);
            return insidePoints.ToArray();
        }
    }
}
