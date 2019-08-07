using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Windows;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using System.Drawing.Drawing2D;

namespace SoftPainter.History.Util
{
    public class PicHandler
    {
        public static int GREEN = 0;
        public static int RED = 1;
        public static int BLUE = 2;

        public static int HEIGHT = 640;
        public static int WIDTH = 760;

        //public static int ABANDONHEIGHT = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["AbandonHeight"]);

        public static int IgnoreHeight = 0;

        private static char[] Convert2BitArray(int[] imageData)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int d in imageData)
            {
                //for (int i = 0; i < 16; i++)
                for (int i = 0; i < 32; i++)
                {
                    int flag = (d >> i) & 1;
                    if (flag == 0)
                        sb.Append("00");  //每一位代表2个像素点
                    else
                        sb.Append("11");  //每一位代表2个像素点
                }
            }

            return sb.ToString().ToCharArray();
        }

        private static char[] Convert2BitArray(short[] imageData)
        {
            StringBuilder sb = new StringBuilder();
            foreach (short d in imageData)
            {
                for (int i = 0; i < 16; i++)
                {
                    int flag = (d >> i) & 1;
                    if (flag == 0)
                        sb.Append("0000");  //每一位代表4个像素点
                    else
                        sb.Append("1111");  //每一位代表4个像素点
                }
            }

            return sb.ToString().ToCharArray();
        }

        public static Bitmap DataToBitmap(int[] imageData, int width, int height)
        {
            Bitmap image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            char[] bitArray = Convert2BitArray(imageData);

            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                image.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * image.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            #region 先列后行，每次计算索引值，更新图像像素值
            int rgbValIdx, count = 0;       //当前像素点在数组中的索引
            int offset = image.Width * 3;   //索引每次需要加上的偏置

            //int endX = startX + columns;    //X结束条件
            int endX = image.Width;
            //endX = endX > image.Width ? image.Width : endX;
            int endY = image.Height;        //Y结束条件

            for (int x = 0; x < endX; x++)
            {
                rgbValIdx = x * 3;
                //for (int y = 0; y < image.Height; y++)//访问image.Height会大大降低程序执行速度
                for (int y = 0; y < endY; y++)
                {
                    if (bitArray[count] == '1')
                    {
                        rgbValues[rgbValIdx] = 255;
                        rgbValues[rgbValIdx + 1] = 255;
                        rgbValues[rgbValIdx + 2] = 255;
                    }
                    else
                    {
                        rgbValues[rgbValIdx] = 0;
                        rgbValues[rgbValIdx + 1] = 0;
                        rgbValues[rgbValIdx + 2] = 0;
                    }

                    rgbValIdx += offset;
                    count++;
                }
            }
            #endregion

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            image.UnlockBits(bmpData);

            return image;
        }

        public static Bitmap DataToBitmap(short[] imageData, int width, int height)
        {
            Bitmap image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            char[] bitArray = Convert2BitArray(imageData);

            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                image.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * image.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            #region 先列后行，每次计算索引值，更新图像像素值
            int rgbValIdx, count = 0;       //当前像素点在数组中的索引
            int offset = image.Width * 3;   //索引每次需要加上的偏置

            //int endX = startX + columns;    //X结束条件
            int endX = image.Width;
            //endX = endX > image.Width ? image.Width : endX;
            int endY = image.Height;        //Y结束条件

            for (int x = 0; x < endX; x++)
            {
                rgbValIdx = x * 3;
                //for (int y = 0; y < image.Height; y++)//访问image.Height会大大降低程序执行速度
                for (int y = 0; y < endY; y++)
                {
                    if (bitArray[count] == '1')
                    {
                        rgbValues[rgbValIdx] = 255;
                        rgbValues[rgbValIdx + 1] = 255;
                        rgbValues[rgbValIdx + 2] = 255;
                    }
                    else
                    {
                        rgbValues[rgbValIdx] = 0;
                        rgbValues[rgbValIdx + 1] = 0;
                        rgbValues[rgbValIdx + 2] = 0;
                    }

                    rgbValIdx += offset;
                    count++;
                }
            }
            #endregion

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            image.UnlockBits(bmpData);

            return image;
        }

        #region 【服役】高效的专用初始化图像方法，耗时约1.5ms
        public static Bitmap BlackImage(int height, int width)
        {
            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics gra = Graphics.FromImage(bmp);
            gra.FillRectangle(System.Drawing.Brushes.Black, 0, 0, width, height);
            return bmp;
        }
        #endregion

        #region 【退役】高效的初始化图像方法，耗时约3ms
        //public static Bitmap BlackImage(int height, int width)
        //{
        //    Bitmap image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

        //    //Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
        //    Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
        //    System.Drawing.Imaging.BitmapData bmpData =
        //        image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
        //        image.PixelFormat);

        //    // Get the address of the first line.
        //    IntPtr ptr = bmpData.Scan0;

        //    // Declare an array to hold the bytes of the bitmap.
        //    int bytes = Math.Abs(bmpData.Stride) * image.Height;
        //    byte[] rgbValues = new byte[bytes];

        //    // Copy the RGB values into the array.
        //    System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

        //    // Set each third values to 0. A 24bpp bitmap will look black.  
        //    for (int counter = 2; counter < rgbValues.Length; counter += 3)
        //    //for (int counter = 2; counter < rgbValues.Length; counter += 3)
        //    {
        //        rgbValues[counter - 2] = 0;
        //        rgbValues[counter - 1] = 0;
        //        rgbValues[counter] = 0;
        //    }
        //    // Copy the RGB values back to the bitmap
        //    System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

        //    // Unlock the bits.
        //    image.UnlockBits(bmpData);

        //    return image;
        //}
        #endregion

        #region 【退役】低效的初始化图像方法，耗时约600ms
        //public static Bitmap BlackImage(int height, int width)
        //{
        //    Bitmap image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        //    int x, y;

        //    for (x = 0; x < image.Width; x++)
        //    {
        //        for (y = 0; y < image.Height; y++)
        //        {
        //            //System.Drawing.Color pixelColor = image.GetPixel(x, y);
        //            System.Drawing.Color newColor = System.Drawing.Color.FromArgb(0, 0, 0);

        //            image.SetPixel(x, y, newColor);
        //        }
        //    }

        //    return image;
        //}
        #endregion

        //高效
        public static ImageSource InitImage(int height, int width)
        {
            return ToBitmapSource(BlackImage(height, width));
        }

        //低效
        //初始化指定宽度和高度的空白（黑色）图像
        //public static ImageSource InitImage(int height, int width)
        //{
        //    Bitmap image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        //    int x, y;

        //    for (x = 0; x < image.Width; x++)
        //    {
        //        for (y = 0; y < image.Height; y++)
        //        {
        //            //System.Drawing.Color pixelColor = image.GetPixel(x, y);
        //            System.Drawing.Color newColor = System.Drawing.Color.FromArgb(0, 0, 0);

        //            image.SetPixel(x, y, newColor);
        //        }
        //    }

        //    return ToBitmapSource(image);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="originImg"></param>
        /// <param name="anandonHeight"></param>
        /// <param name="maxHeight"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        public static Bitmap ResizeImage(Bitmap originImg, int anandonHeight, double maxHeight, double maxWidth)
        {
            //复制制定高度以下的图像
            Rectangle cropArea = new Rectangle();

            cropArea.X = 0;
            cropArea.Y = anandonHeight;
            cropArea.Width = originImg.Width;
            cropArea.Height = originImg.Height - anandonHeight;

            Bitmap reducedImg = originImg.Clone(cropArea, originImg.PixelFormat);

            //进行resize操作
            Bitmap resizedImg;

            double widthRatio = ((int)maxWidth) * 1.0 / reducedImg.Width;
            double heightRatio = ((int)maxHeight) * 1.0 / reducedImg.Height;
            double resizedRatio = widthRatio < heightRatio ? widthRatio : heightRatio;

            System.Drawing.Size newSize = new System.Drawing.Size((int)(resizedRatio * reducedImg.Width), (int)(resizedRatio * reducedImg.Height));
            resizedImg = new Bitmap(reducedImg, newSize);

            return resizedImg;
            //reducedImg.Save(@"D:\workingwc\AutoSprayPainting\CentralPanel\ImageProcessResult\reduced.bmp");
            //reducedImg = originImg.Clone()
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="maxHeight"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        public static Bitmap ResizeImage(Bitmap originImg, double maxHeight, double maxWidth)
        {
            Bitmap resizedImg;

            double widthRatio = ((int)maxWidth) * 1.0 / originImg.Width;
            double heightRatio = ((int)maxHeight) * 1.0 / originImg.Height;
            double resizedRatio = widthRatio < heightRatio ? widthRatio : heightRatio;
            
            System.Drawing.Size newSize = new System.Drawing.Size((int)(resizedRatio * originImg.Width), (int)(resizedRatio * originImg.Height));
            //resizedImg = new Bitmap(originImg, newSize);
            //resizedImg = new Bitmap(originImg.Clone(new Rectangle(0, 0, originImg.Width, originImg.Height), originImg.PixelFormat), newSize);
            //resizedImg = new Bitmap(originImg.Clone() as Bitmap, newSize);
            resizedImg = KiResizeImage(originImg, newSize.Width, newSize.Height, 0);

            return resizedImg;
        }

        ///
        /// Resize图片
        ///
        /// 原始Bitmap
        /// 新的宽度
        /// 新的高度
        /// 保留着，暂时未用
        /// 处理以后的图片
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH, int Mode)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                //g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.DrawImage(bmp, 0, 0, newW, newH);
                g.Dispose();

                return b;
            }
            catch
            {
                return null;
            }
        }

        #region 【服役】高效的图像扩展方式，耗时<3ms，且时间不随列数增加
        public static void AppendBitArrayToOldPicByIndex(ref Bitmap image, string bitArrayStr, int startX, int columns)
        {
            //Stopwatch wb = new Stopwatch();
            if (string.IsNullOrEmpty(bitArrayStr))
                return;

            char[] bitArray = bitArrayStr.ToCharArray();

            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                image.LockBits(rect, System.Drawing.Imaging.ImageLockMode.WriteOnly,
                image.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * image.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            #region 先列后行，每次计算索引值，更新图像像素值
            int rgbValIdx, count = 0;       //当前像素点在数组中的索引
            int offset = image.Width * 3;   //索引每次需要加上的偏置

            int endX = startX + columns;    //X结束条件
            endX = endX > image.Width ? image.Width : endX;
            int endY = image.Height;        //Y结束条件

            for (int x = startX; x < endX; x++)
            {
                rgbValIdx = x * 3;
                //for (int y = 0; y < image.Height; y++)//访问image.Height会大大降低程序执行速度
                for (int y = 0; y < endY; y++)
                {
                    if (bitArray[count] == '1')
                    {
                        rgbValues[rgbValIdx] = 255;
                        rgbValues[rgbValIdx + 1] = 255;
                        rgbValues[rgbValIdx + 2] = 255;
                    }
                    else
                    {
                        rgbValues[rgbValIdx] = 0;
                        rgbValues[rgbValIdx + 1] = 0;
                        rgbValues[rgbValIdx + 2] = 0;
                    }

                    rgbValIdx += offset;
                    count++;
                }
            }
            #endregion

            // Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            image.UnlockBits(bmpData);

            return;
        }
        #endregion

        #region 【退役】低效的图像扩展方式，耗时15ms/20列
        //public static void AppendBitArrayToOldPicByIndex(ref Bitmap oldPic, string bitArrayStr, int startX, int columns)
        //{
        //    if (string.IsNullOrEmpty(bitArrayStr))
        //        return;

        //    char[] bitArray = bitArrayStr.ToCharArray();

        //    //Bitmap image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        //    //image.Save("D:/test.bmp");

        //    int x, y;

        //    int count = 0;

        //    System.Drawing.Color newColor;
        //    System.Drawing.Color white = System.Drawing.Color.FromArgb(255, 255, 255);
        //    System.Drawing.Color black = System.Drawing.Color.FromArgb(0, 0, 0);
        //    // Loop through the images pixels to reset color.
        //    int endX = startX + columns;
        //    endX = endX > oldPic.Width ? oldPic.Width : endX;
        //    for (x = startX; x < endX; x++)//横轴
        //    {
        //        for (y = 0; y < oldPic.Height; y++)//纵轴
        //        {
        //            //System.Drawing.Color pixelColor = image.GetPixel(x, y);
        //            if (bitArray[count] == '1')
        //                newColor = white;
        //            //newColor = System.Drawing.Color.FromArgb(255, 255, 255);//可优化
        //            else
        //                newColor = black;
        //            //newColor = System.Drawing.Color.FromArgb(0, 0, 0);

        //            oldPic.SetPixel(x, y, newColor);
        //            count++;
        //        }
        //    }

        //    //startX += columns;

        //    return;
        //}
        #endregion

        #region 【退役】低效的图像扩展方式，耗时15ms/20列
        //public static void AppendBitArrayToOldPic(ref Bitmap oldPic, string bitArrayStr, ref int startX, int columns)
        //{
        //    if (string.IsNullOrEmpty(bitArrayStr))
        //        return;

        //    char[] bitArray = bitArrayStr.ToCharArray();

        //    //Bitmap image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        //    //image.Save("D:/test.bmp");

        //    int x, y;

        //    int count = 0;

        //    System.Drawing.Color newColor;
        //    System.Drawing.Color white = System.Drawing.Color.FromArgb(255, 255, 255);
        //    System.Drawing.Color black = System.Drawing.Color.FromArgb(0, 0, 0);
        //    // Loop through the images pixels to reset color.
        //    int endX = startX + columns;
        //    endX = endX > oldPic.Width ? oldPic.Width : endX;
        //    for (x = startX; x < endX; x++)//横轴
        //    {
        //        for (y = 0; y < oldPic.Height; y++)//纵轴
        //        {
        //            //System.Drawing.Color pixelColor = image.GetPixel(x, y);
        //            if (bitArray[count] == '1')
        //                newColor = white;
        //            //newColor = System.Drawing.Color.FromArgb(255, 255, 255);//可优化
        //            else
        //                newColor = black;
        //            //newColor = System.Drawing.Color.FromArgb(0, 0, 0);

        //            oldPic.SetPixel(x, y, newColor);
        //            count++;
        //        }
        //    }

        //    startX += columns;

        //    return;
        //}  
        #endregion

        //public static Bitmap ConvertBitArrayToPic(int[] bitArray, int height, int width)
        public static Bitmap ConvertBitArrayToPic(byte[] bitArray, int height, int width)    
        {
            if (bitArray == null)
                return null;
            
            Bitmap image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            //image.Save("D:/test.bmp");

            int x, y;

            int alen = bitArray.Length, count = 0;

            System.Drawing.Color newColor;
            // Loop through the images pixels to reset color.
            for (x = 0; x < image.Width; x++)//横轴
            {
                for (y = 0; y < image.Height; y++)//纵轴
                {
                    //System.Drawing.Color pixelColor = image.GetPixel(x, y);
                    if (count < alen && bitArray[count] == 1)
                        newColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    else
                        newColor = System.Drawing.Color.FromArgb(0, 0, 0);
                    
                    image.SetPixel(x, y, newColor);
                    count++;
                }
            }

            return image;
        }

        public static Bitmap ConvertBitArrayToPic2(byte[] bitArray, int height, int width)
        {
            if (bitArray == null)
                return null;

            Bitmap image = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //image.Save("D:/test.bmp");

            int x, y;

            int alen = bitArray.Length, count = 0;

            System.Drawing.Color newColor;
            // Loop through the images pixels to reset color.
            for (y = 0; y < image.Height; y++)//纵轴
            {
                for (x = 0; x < image.Width; x++)//横轴
                {
                    //System.Drawing.Color pixelColor = image.GetPixel(x, y);
                    if (count < alen && bitArray[count] == 1)
                        newColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    else
                        newColor = System.Drawing.Color.FromArgb(0, 0, 0);

                    image.SetPixel(x, y, newColor);
                    count++;
                }
            }

            return image;
        }  

        //public static int[] ConvertStrToBitArray(string str)
        public static byte[] ConvertStrToBitArray(string str)
        {
            //List<int> bitArray = new List<int>();
            List<byte> bitArray = new List<byte>();

            Regex isBit = new Regex(@"[01]*");

            if (isBit.IsMatch(str) && (isBit.Match(str).Length == str.Length))
            {
                foreach (char curBit in str)
                    bitArray.Add(byte.Parse(curBit.ToString()));
                return bitArray.ToArray();
            }
            else
                return null;
        }

        public static string GetReversedStr(string roughStr)
        {
            char[] charArray = roughStr.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string GetExtendedStr(string roughStr, int repeatCount)
        {
            StringBuilder strBuild = new StringBuilder();
            foreach (char c in roughStr)
                strBuild.Append(c, repeatCount);
            return strBuild.ToString();
        }

        public static string GetCopiedStr(string roughStr, int times)
        {
            //string newStr = "";
            StringBuilder newStr = new StringBuilder();
            int count = 0;
            while (count++ < times)
                newStr.Append(roughStr);
            return newStr.ToString();
        }

        public static ImageSource DrawCircles(string picFilePath, List<VectorOfPoint> coords)
        {
            Mat rawImg = new Mat(picFilePath, ImreadModes.Color);
            foreach (var fourPoints in coords)
            {
                foreach (System.Drawing.Point curPoint in fourPoints.ToArray())
                {
                    CvInvoke.Circle(rawImg, curPoint, 5, new Emgu.CV.Structure.MCvScalar(0, 0, 255), thickness: 2);
                }
            }
            Bitmap bi = rawImg.Bitmap;
            //rawImg.Save(@"D:/circle_temp.bmp");
            //CvInvoke.Circle(rawImg, )
            return ToBitmapSource(bi);
            //return null;
        }

        public static Bitmap DrawPolyLines(string picFilePath, Dictionary<int, List<VectorOfPoint>> colorVPs)
        {
            Mat rawImg = new Mat(picFilePath, ImreadModes.Color);

            List<VectorOfPoint> greenList = new List<VectorOfPoint>();
            List<VectorOfPoint> redList = new List<VectorOfPoint>();
            List<VectorOfPoint> blueList = new List<VectorOfPoint>();

            if (colorVPs.ContainsKey(PicHandler.GREEN))
                greenList = colorVPs[PicHandler.GREEN];
            if (colorVPs.ContainsKey(PicHandler.RED))
                redList = colorVPs[PicHandler.RED];
            if (colorVPs.ContainsKey(PicHandler.BLUE))
                blueList = colorVPs[PicHandler.BLUE];

            int thick = rawImg.Cols == 3000 ? 10 : 2;

            foreach (VectorOfPoint polygon in greenList)
                CvInvoke.Polylines(rawImg, polygon, true, new Emgu.CV.Structure.MCvScalar(0, 255, 0), thickness: thick);
            foreach (VectorOfPoint polygon in redList)
                CvInvoke.Polylines(rawImg, polygon, true, new Emgu.CV.Structure.MCvScalar(0, 0, 255), thickness: thick);
            foreach (VectorOfPoint polygon in blueList)
                CvInvoke.Polylines(rawImg, polygon, true, new Emgu.CV.Structure.MCvScalar(255, 0, 0), thickness: thick);

            return rawImg.Bitmap;
            //Bitmap bi = rawImg.Bitmap;
            //rawImg.Save(@"D:/circle_temp.bmp");

            //return bi;
        }

        #region 【服役】较高效的图像类型转换方式，耗时约9ms
        public static BitmapSource FastToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height, 96, 96, PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            return bitmapSource;
        }
        #endregion

        #region 【服役】低效的图像类型转换方式，耗时约14ms
        #region Convert Bitmap to ImageSource...
        [DllImport("gdi32.dll", SetLastError = true)]

        private static extern bool DeleteObject(IntPtr hObject);

        //耗时12毫秒（可能存在内存泄漏）
        public static ImageSource ToBitmapSource(Bitmap p_bitmap)
        {
            IntPtr hBitmap = p_bitmap.GetHbitmap();
            ImageSource wpfBitmap;
            try
            {
                wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                //p_bitmap.Dispose();
                DeleteObject(hBitmap);
            }
            return wpfBitmap;
        }
        #endregion
        #endregion

        #region 【退役】低效的图像类型转换方式，耗时约20ms
        //public static BitmapSource TranslateFromBitmap(System.Drawing.Bitmap bitmap)
        //{
        //    BitmapSource destination;
        //    IntPtr hBitmap = bitmap.GetHbitmap();
        //    BitmapSizeOptions sizeOptions = BitmapSizeOptions.FromEmptyOptions();
        //    destination = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, sizeOptions);
        //    destination.Freeze();
        //    return destination;
        //}
        #endregion
    }
}
