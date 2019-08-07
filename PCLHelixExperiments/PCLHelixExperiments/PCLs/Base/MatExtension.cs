using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using Emgu.CV;
using Emgu.CV.Structure;

namespace SoftPainter.Core.Base
{
    public static class MatExtension
    {
        public static double[] GetDoubleArray(this Mat mat)
        {
            double[] temp = new double[mat.Height * mat.Width];
            Marshal.Copy(mat.DataPointer, temp, 0, mat.Height * mat.Width);
            return temp;
        }

        public static int[] GetIntArray(this Mat mat)
        {
            int[] temp = new int[mat.Height * mat.Width];
            Marshal.Copy(mat.DataPointer, temp, 0, mat.Height * mat.Width);
            return temp;
        }

        public static byte[] GetByteArray(this Mat mat)
        {
            byte[] temp = new byte[mat.Height * mat.Width];
            Marshal.Copy(mat.DataPointer, temp, 0, mat.Height * mat.Width);
            return temp;
        }

        public static Image<Bgr, Byte> GetBgrImage(this Mat mat)
        {
            Image<Bgr, Byte> image = mat.ToImage<Bgr, Byte>();
            return image;
        }

        public static Image<Xyz, Byte> GetXyzImage(this Mat mat)
        {
            Image<Xyz, Byte> image = mat.ToImage<Xyz, Byte>();
            return image;
        }

        public static Image<Bgra, Byte> GetBgraImage(this Mat mat)
        {
            Image<Bgra, Byte> image = mat.ToImage<Bgra, Byte>();
            return image;
        }

        public static void SetImage(this Mat mat, Image<Bgr, Byte> image)
        {
            mat.Dispose();
            mat = image.Mat;
        }

        public static void SetImage(this Mat mat, Image<Bgra, Byte> image)
        {
            mat.Dispose();
            mat = image.Mat;
        }

        public static void SetImage(this Mat mat, Image<Xyz, Byte> image)
        {
            mat.Dispose();
            mat = image.Mat;
        }

        public static void SetArray(this Mat mat, double[] data)
        {
            Marshal.Copy(data, 0, mat.DataPointer, mat.Height * mat.Width);
        }

        public static void SetArray(this Mat mat, int[] data)
        {
            Marshal.Copy(data, 0, mat.DataPointer, mat.Height * mat.Width);
        }

        public static void SetArray(this Mat mat, byte[] data)
        {
            Marshal.Copy(data, 0, mat.DataPointer, mat.Height * mat.Width);
        }
    }
}
