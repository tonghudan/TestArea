using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;

namespace SoftPainter.Core.Base
{
    public class ByteMat
    {
        /// <summary>
        /// 一维化的矩阵数据
        /// </summary>
        public byte[] Data { get; set; }
        /// <summary>
        /// 矩阵高度
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 矩阵宽度
        /// </summary>
        public int Width { get; set; }

        public ByteMat(Mat mat)
        {
            this.Data = mat.GetByteArray();
            this.Height = mat.Height;
            this.Width = mat.Width;
        }
    }
}
