using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

using SoftPainter.History.Class;
using SoftPainter.Core.Scanner;

namespace SoftPainter.History.Util
{
    public class ScanUtil
    {
        public const string MACHINECODESAFE = "0";
        public const string MACHINECODEUNSAFE_X = "-1";
        public const string MACHINECODEUNSAFE_Y = "-2";
        public const string MACHINECODEUNSAFE_Z = "-3";
        public const string MACHINECODEUNSAFE_RZ = "-4";

        public static string PROCESS_MESSAGE = "";
        public static string LOG_MESSAGE = "";

        public static void ScanSavePCDMultiSocketAsync()
        {
            StringBuilder log = new StringBuilder();

            if (Directory.Exists("scan"))   //扫描开始前，清空scan文件夹或创建空scan文件夹
                FileHandler.DeleteDir("scan");
            else
                Directory.CreateDirectory("scan");

            StaticAll.Scanner.ScanFastAsync();
        }

        public static void ScanSaveRawPCDAsync()
        {
            StringBuilder log = new StringBuilder();

            if (Directory.Exists("scan"))   //扫描开始前，清空scan文件夹或创建空scan文件夹
                FileHandler.DeleteDir("scan");
            else
                Directory.CreateDirectory("scan");

            StaticAll.Scanner.ScanRawAsync();
        }

        public static bool ScanSavePCDMultiSocket()
        {
            StringBuilder log = new StringBuilder();

            if (Directory.Exists("scan"))   //扫描开始前，清空scan文件夹或创建空scan文件夹
                FileHandler.DeleteDir("scan");
            else
                Directory.CreateDirectory("scan");

            if (!StaticAll.Scanner.ScanFast())
            {
                log.Append("-1\t激光器扫描失败\n");
                LOG_MESSAGE = log.ToString();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取scan目录下生成的BMP图像路径。
        /// 代码对应_3DScanPanel.xaml.cs里的NewSave2D_Button_Click函数。
        /// </summary>
        /// <returns></returns>
        public static string GetSavedFileName()
        {
            string[] fileInfo = Directory.GetFiles("scan", "merged*.bmp");

            if (fileInfo.Length == 0) return null;
            else
            {
                //Bitmap bp = new Bitmap(fileInfo[0]);
                //Image.Source = PicHandler.ToBitmapSource(bp);
                //bp.Dispose();
                //this.bmpfile = fileInfo[0];
                return fileInfo[0];
            }
        }

        /// <summary>
        /// 获取scan目录下生成的PCD文件路径。
        /// </summary>
        /// <returns></returns>
        public static string[] GetSavedPCD()
        {
            string[] fileInfo = Directory.GetFiles("scan", "*.pcd");

            if (fileInfo.Length == 0) return null;
            else
            {
                return fileInfo;
            }
        }

        /// <summary>
        /// 把合并后的PCD文件转化成BMP图像。
        /// 代码对应_3DScanPanel.xaml.cs里的NewSave2D_Button_Click函数。
        /// </summary>
        public static void ScanMergedPCD2BMP()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("project2d.exe");
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            string debugstr = "";
            startInfo.Arguments = "scan//merged.pcd -line 1 -kernel " + StaticAll.Parameters.KernelSize.ToString() +
                " -mk " + StaticAll.Parameters.MeanK.ToString() + " -min_pl " + StaticAll.Parameters.MinPL.ToString() + " -max_keep " + StaticAll.Parameters.MinKeep.ToString() +
                " -minx " + StaticAll.Parameters.Minx.ToString() + " -maxx " + StaticAll.Parameters.Maxx.ToString() + " -miny " + StaticAll.Parameters.Miny.ToString() + " -maxy " + StaticAll.Parameters.Maxy.ToString() +
                " -minz " + StaticAll.Parameters.Minz.ToString() + " -maxz " + StaticAll.Parameters.Maxz.ToString() + debugstr;

            StringBuilder sb = new StringBuilder();

            Process p = Process.Start(startInfo);
            p.OutputDataReceived += (s, _e) => sb.AppendLine(_e.Data);
            p.BeginOutputReadLine();
            p.WaitForExit();

            ScanUtil.PROCESS_MESSAGE = sb.ToString();
        }

        /// <summary>
        /// 把保存的多个PCD文件合并成单个PCD文件。
        /// 代码对应_3DScanPanel.xaml.cs里的NewSave2D_Button_Click函数。
        /// </summary>
        public static void ScanMergePCD(int pcdCount = 1)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("project2d.exe");
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            // merge clouds
            //startInfo.Arguments = "scan_merge -times " + pcdCount.ToString();
            //startInfo.Arguments = "scan_merge -times " + pcdCount.ToString() + " -line " + StaticVar.Parameters.LineWidth + " -kernel " + StaticVar.Parameters.KernelSize +

            //startInfo.Arguments = "scan_merge_fast -times " + pcdCount.ToString() + " -line " + StaticVar.Parameters.LineWidth + " -kernel " + StaticVar.Parameters.KernelSize +
            //            " -mk " + StaticVar.Parameters.MeanK + " -min_pl " + StaticVar.Parameters.MinPL + " -max_keep " + StaticVar.Parameters.MinKeep +
            //            " -minx " + StaticVar.Parameters.Minx + " -maxx " + StaticVar.Parameters.Maxx + " -miny " + StaticVar.Parameters.Miny + " -maxy " + StaticVar.Parameters.Maxy +
            //            //" -minz " + StaticVar.Parameters.Minz + " -maxz " + StaticVar.Parameters.Maxz + " -transE 1e-10" + " -euFE 0.01";
            //            " -minz " + StaticVar.Parameters.Minz + " -maxz " + StaticVar.Parameters.Maxz + " -transE 1e-8" + " -euFE 1";

            startInfo.Arguments = "scan_merge_fast -times " + pcdCount.ToString() + " -line 1" + " -kernel " + StaticAll.Parameters.KernelSize +
                        " -mk " + StaticAll.Parameters.MeanK + " -min_pl " + StaticAll.Parameters.MinPL + " -max_keep " + StaticAll.Parameters.MinKeep +
                        " -minx " + StaticAll.Parameters.Minx + " -maxx " + StaticAll.Parameters.Maxx + " -miny " + StaticAll.Parameters.Miny + " -maxy " + StaticAll.Parameters.Maxy +
                        //" -minz " + StaticVar.Parameters.Minz + " -maxz " + StaticVar.Parameters.Maxz + " -transE 1e-10" + " -euFE 0.01";
                        " -minz " + StaticAll.Parameters.Minz + " -maxz " + StaticAll.Parameters.Maxz + " -transE 1e-8" + " -euFE 1" + " -max_flat_dist " + 0 + " -poly_epsilon 1" + " -planar_threshold " + StaticAll.Parameters.PlanarThresold;

            //.\project2d.exe scan_merge_fast -times 6 -line 1 -kernel 10 -mk 1 -min_pl 1 -max_keep 10000 -transE 1e-8 -euFE 1 -max_flat_dist 80 -poly_epsilon 1
            //Process p = Process.Start(startInfo);
            //p.WaitForExit();
            StringBuilder sb = new StringBuilder();

            Process p = Process.Start(startInfo);
            //p.OutputDataReceived += (s, _e) => sb.AppendLine(_e.Data);
            //p.BeginOutputReadLine();
            p.WaitForExit();
        }

        /// <summary>
        /// 执行一次扫描并保存PCD操作。
        /// 代码对应_3DScanPanel.xaml.cs里的NewSave2D_Button_Click函数。
        /// </summary>
        /// <param name="scanOrder">指定是在几号位置上扫描</param>
        public static void ScanSavePCD(int scanOrder)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("project2d.exe");
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            //string debugstr = (this.Debug.IsChecked.HasValue && this.Debug.IsChecked.Value) ? " -debug 1" : "";
            string debugstr = "";
            startInfo.Arguments = "scan_save_" + scanOrder.ToString() + debugstr;
            Process p = Process.Start(startInfo);
            p.WaitForExit();
            //TODO move robot @Xiaodong
        }

        /// <summary>
        /// 根据bmp图像名称和transform.txt把2D机器码转化为3D机器码。
        /// 代码对应_3DScanPanel.xaml.cs里的MachineCode_Button_Click函数。
        /// </summary>
        /// <param name="sourceCodePath"></param>
        /// <param name="destCodePath"></param>
        /// <param name="bmpname"></param>
        /// <returns></returns>
        public static bool TransMachineCode(string sourceCodePath, string destCodePath, string bmpname)
        {
            string[] items = bmpname.Substring(0, bmpname.Length - 4).Split('#');
            if (items.Length < 3)
            {
                //MessageBox.Show("bmp图像文件名解析失败！");
                return false;
            }
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

            Matrix<double> T2 = Matrix<double>.Build.Dense(4, 4);
            using (StreamReader sr = new StreamReader("transform.txt"))
            {
                string str = sr.ReadLine();
                string[] vals = str.Split();
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        T2[i, j] = float.Parse(vals[i * 4 + j]);
                    }
            }
            T1 = T2 * T1;

            double Trx = Math.Atan2(T1[2, 1], T1[2, 2]) / Math.PI * 180;
            double Try = Math.Asin(-T1[2, 0]) / Math.PI * 180;
            double Trz = Math.Atan2(T1[1, 0], T1[0, 0]) / Math.PI * 180;

            //MessageBox.Show("rx,ry,rz=" + rx.ToString() + "," + ry.ToString() + "," + rz.ToString() + " trx,try,trz=" +
            //                              Trx.ToString() + "," + Try.ToString() + "," + Trz.ToString());
            using (StreamReader sr = new StreamReader(sourceCodePath))
            {
                StreamWriter sw = new StreamWriter(destCodePath);
                bool status_get_position = false;
                string line, linenew;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line.IndexOf("P00001=") >= 0)
                    {
                        status_get_position = true;
                    }
                    if (line.IndexOf("//INST") >= 0)
                    {
                        status_get_position = false;
                    }

                    if (status_get_position)
                    {
                        string[] xyz = line.Substring(7, line.Length - 7).Split(',');
                        Vector<double> v = Vector<double>.Build.DenseOfArray(new double[] { double.Parse(xyz[0]), double.Parse(xyz[1]), double.Parse(xyz[2]), 1.0 });
                        Vector<double> newv = T1 * v;
                        double newrx = double.Parse(xyz[3]) + Trx, newry = double.Parse(xyz[4]) + Try, newrz = double.Parse(xyz[5]) + Trz;
                        linenew = line.Substring(0, 7) + newv[0].ToString("F3") + "," + newv[1].ToString("F3") + "," + newv[2].ToString("F3") + "," +
                            newrx.ToString("F4") + "," + newry.ToString("F4") + "," + newrz.ToString("F4");

                    }
                    else
                    {
                        linenew = line;
                    }
                    sw.WriteLine(linenew);
                }
                sw.Close();
            }
            //MessageBox.Show("转换3D机器码成功！");
            return true;
        }

        public static bool TransMachineCodeDxDy(string sourceCodePath, string destCodePath, int robotID)
        {
            //StaticVar.Parameters.ID = robotID;

            using (StreamReader sr = new StreamReader(sourceCodePath))
            {
                StreamWriter sw = new StreamWriter(destCodePath);
                bool status_get_position = false;
                string line, linenew;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line.IndexOf("P00001=") >= 0)
                    {
                        status_get_position = true;
                    }
                    if (line.IndexOf("//INST") >= 0)
                    {
                        status_get_position = false;
                    }

                    if (status_get_position)
                    {
                        string[] xyz = line.Substring(7, line.Length - 7).Split(',');

                        double x = double.Parse(xyz[0]), y = double.Parse(xyz[1]), z = double.Parse(xyz[2]);
                        double Trx = double.Parse(xyz[3]) * Math.PI / 180, Try = double.Parse(xyz[4]) * Math.PI / 180;

                        double newX = x - StaticAll.Parameters.workpieceLength * Math.Sin(Try);
                        double newY = y + StaticAll.Parameters.workpieceLength * Math.Sin(Trx);
                        double newZ = z + StaticAll.Parameters.workpieceLength * (2 - Math.Cos(Trx) - Math.Cos(Try));

                        linenew = line.Substring(0, 7) + newX.ToString("F3") + "," + newY.ToString("F3") + "," + newZ.ToString("F3") + "," +
                            xyz[3] + "," + xyz[4] + "," + xyz[5];

                    }
                    else
                    {
                        linenew = line;
                    }
                    sw.WriteLine(linenew);
                }
                sw.Close();
            }
            //MessageBox.Show("转换3D机器码成功！");
            return true;
        }

        public static bool TransMachineCodeRxRy(string sourceCodePath, string destCodePath)
        {
            using (StreamReader sr = new StreamReader(sourceCodePath))
            {
                StreamWriter sw = new StreamWriter(destCodePath);
                bool status_get_position = false;
                bool is180 = false;
                string line, linenew;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line.IndexOf("P00001=") >= 0)
                    {
                        status_get_position = true;
                        string[] xyz = line.Substring(7, line.Length - 7).Split(',');
                        double Trx = double.Parse(xyz[3]), Try = double.Parse(xyz[4]), rz = double.Parse(xyz[5]);
                        if (Trx > 100)
                            is180 = true;
                    }
                    if (line.IndexOf("//INST") >= 0)
                    {
                        status_get_position = false;
                    }

                    if (status_get_position)
                    {
                        if (!is180)
                        {
                            string[] xyz = line.Substring(7, line.Length - 7).Split(',');
                            double Trx = double.Parse(xyz[3]), Try = double.Parse(xyz[4]), rz = double.Parse(xyz[5]);
                            double newrz = rz;
                            double newrx = CalRx(Trx, Try, newrz), newry = CalRy(Trx, Try, newrz);
                            linenew = line.Substring(0, 7) + xyz[0] + "," + xyz[1] + "," + xyz[2] + "," +
                                newrx.ToString("F4") + "," + newry.ToString("F4") + "," + newrz.ToString("F4");
                        }
                        else
                        {
                            string[] xyz = line.Substring(7, line.Length - 7).Split(',');
                            double Trx = double.Parse(xyz[3]) - 180, Try = double.Parse(xyz[4]), rz = double.Parse(xyz[5]);
                            double newrz = rz;
                            double newrx = CalRx(Trx, Try, newrz), newry = CalRy(Trx, Try, newrz);
                            linenew = line.Substring(0, 7) + xyz[0] + "," + xyz[1] + "," + xyz[2] + "," +
                                (newrx + 180).ToString("F4") + "," + newry.ToString("F4") + "," + newrz.ToString("F4");
                        }
                    }
                    else
                    {
                        linenew = line;
                    }
                    sw.WriteLine(linenew);
                }
                sw.Close();
            }
            //MessageBox.Show("转换3D机器码成功！");
            return true;
        }

        public static string MachineCodeSafetyCheck(string codePath, int robotID)
        {
            //增加Rz范围检测
            if (!MachineCodeRzCheck(Path.Combine(Path.GetDirectoryName(codePath), "AUTO001.JBI"), robotID))
                return MACHINECODEUNSAFE_RZ;

            //StaticVar.Parameters.ID = robotID;
            using (StreamReader sr = new StreamReader(codePath))
            {
                bool status_get_position = false;
                string line;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line.IndexOf("P00001=") >= 0)
                    {
                        status_get_position = true;
                    }
                    if (line.IndexOf("//INST") >= 0)
                    {
                        status_get_position = false;
                    }

                    if (status_get_position)
                    {
                        string[] xyz = line.Substring(7, line.Length - 7).Split(',');

                        double x = double.Parse(xyz[0]), y = double.Parse(xyz[1]), z = double.Parse(xyz[2]);

                        if (StaticAll.Parameters.CodeMinX < StaticAll.Parameters.CodeMaxX)
                            if (x < StaticAll.Parameters.CodeMinX || x > StaticAll.Parameters.CodeMaxX)
                                return MACHINECODEUNSAFE_X;
                        if (StaticAll.Parameters.CodeMinY < StaticAll.Parameters.CodeMaxY)
                            if (y < StaticAll.Parameters.CodeMinY || y > StaticAll.Parameters.CodeMaxY)
                                return MACHINECODEUNSAFE_Y;
                        if (StaticAll.Parameters.CodeMinZ < StaticAll.Parameters.CodeMaxZ)
                            if (z < StaticAll.Parameters.CodeMinZ || z > StaticAll.Parameters.CodeMaxZ)
                                return MACHINECODEUNSAFE_Z;
                    }
                }
            }
            return MACHINECODESAFE;
        }

        public static bool MachineCodeRzCheck(string codePath, int robotID)
        {
            //StaticVar.Parameters.ID = robotID;
            using (StreamReader sr = new StreamReader(codePath))
            {
                bool status_get_position = false;
                string line;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line.IndexOf("P00001=") >= 0)
                    {
                        status_get_position = true;
                    }
                    if (line.IndexOf("//INST") >= 0)
                    {
                        status_get_position = false;
                    }

                    if (status_get_position)
                    {
                        string[] xyz = line.Substring(7, line.Length - 7).Split(',');

                        double rz = double.Parse(xyz[5]);

                        if (StaticAll.Parameters.CodeMinRz < StaticAll.Parameters.CodeMaxR)
                            if (rz < StaticAll.Parameters.CodeMinRz || rz > StaticAll.Parameters.CodeMaxR)
                                return false;
                    }
                }
            }
            return true;
        }

        public static double CalRx(double rx, double ry, double rz)
        {
            return rx * Math.Sin(toRadian(90 + rz)) + ry * Math.Sin(toRadian(rz));
        }

        public static double CalRy(double rx, double ry, double rz)
        {
            return rx * Math.Cos(toRadian(90 + rz)) + ry * Math.Cos(toRadian(rz));
        }

        public static double toRadian(double angle)
        {
            return angle * Math.PI / 180;
        }

        public static double toAngle(double radian)
        {
            return radian * 180 / Math.PI;
        }

        public static RobotPositionData[] GetRPD(RobotPositionData rpd1, RobotPositionData rpd2)
        {

            return null;
        }

        /// <summary>
        /// 把一个2D的坐标点转化为3D的坐标点
        /// </summary>
        /// <param name="bmpname"></param>
        /// <param name="sourceRPD"></param>
        /// <returns></returns>
        public static RobotPositionData TransTo3DRPD(string bmpname, double x, double y, double z, double rx, double ry, double rz)
        {
            RobotPositionData result = new RobotPositionData(x, y, z, rx, ry, rz);

            Matrix<double> T1 = Matrix<double>.Build.Dense(4, 4);
            double Trx = 0, Try = 0, Trz = 0;
            bool res = ResolveImageName(bmpname, ref Trx, ref Try, ref Trz, ref T1);
            if (!res)   //如果不是3D文件格式，则是2D文件格式，直接返回原始
                return result;

            Vector<double> v = Vector<double>.Build.DenseOfArray(new double[] { x, y, z, 1.0 });
            Vector<double> newv = T1 * v;

            result.X = newv[0];
            result.Y = newv[1];
            result.Z = newv[2];
            result.Rx = rx + Trx;
            result.Ry = ry + Try;
            result.Rz = rz + Trz;

            return result;
        }

        /// <summary>
        /// 解析图像名字，得到旋转矩阵和旋转的rx,ry,rz
        /// </summary>
        /// <param name="bmpname"></param>
        /// <param name="Trx"></param>
        /// <param name="Try"></param>
        /// <param name="Trz"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public static bool ResolveImageName(string bmpname, ref double Trx, ref double Try, ref double Trz, ref Matrix<double> T)
        {
            try
            {
            string[] items = bmpname.Substring(0, bmpname.Length - 4).Split('#');
            if (items.Length < 3)
            {
                //MessageBox.Show("bmp图像文件名解析失败！");
                return false;
            }
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

            Matrix<double> T2 = Matrix<double>.Build.Dense(4, 4);
            using (StreamReader sr = new StreamReader("transform.txt"))
            {
                string str = sr.ReadLine();
                string[] vals = str.Split();
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        T2[i, j] = float.Parse(vals[i * 4 + j]);
                    }
            }
            T1 = T2 * T1;

            Trx = Math.Atan2(T1[2, 1], T1[2, 2]) / Math.PI * 180;
            Try = Math.Asin(-T1[2, 0]) / Math.PI * 180;
            Trz = Math.Atan2(T1[1, 0], T1[0, 0]) / Math.PI * 180;
            T = T1;

            return true;
            }
            catch(Exception err)
            {
                return false;
            }
        }

        /// <summary>
        /// 绕任意轴旋转的旋转矩阵
        /// </summary>
        /// <param name="m"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="theta"></param>
        public static Matrix<double> RotateArbitraryLine(Vector<double> v1, Vector<double> v2, double theta)
        {
            Matrix<double> m = Matrix<double>.Build.Dense(4, 4);

            double a = v1[0];
            double b = v1[1];
            double c = v1[2];

            Vector<double> p = v2 - v1;
            Normalize(ref p);
            double u = p[0];
            double v = p[1];
            double w = p[2];

            double uu = u * u;
            double uv = u * v;
            double uw = u * w;
            double vv = v * v;
            double vw = v * w;
            double ww = w * w;
            double au = a * u;
            double av = a * v;
            double aw = a * w;
            double bu = b * u;
            double bv = b * v;
            double bw = b * w;
            double cu = c * u;
            double cv = c * v;
            double cw = c * w;

            double costheta = Math.Cos(toRadian(theta));
            double sintheta = Math.Sin(toRadian(theta));

            m[0, 0] = uu + (vv + ww) * costheta;
            m[0, 1] = uv * (1 - costheta) + w * sintheta;
            m[0, 2] = uw * (1 - costheta) - v * sintheta;
            m[0, 3] = 0;

            m[1, 0] = uv * (1 - costheta) - w * sintheta;
            m[1, 1] = vv + (uu + ww) * costheta;
            m[1, 2] = vw * (1 - costheta) + u * sintheta;
            m[1, 3] = 0;

            m[2, 0] = uw * (1 - costheta) + v * sintheta;
            m[2, 1] = vw * (1 - costheta) - u * sintheta;
            m[2, 2] = ww + (uu + vv) * costheta;
            m[2, 3] = 0;

            m[3, 0] = (a * (vv + ww) - u * (bv + cw)) * (1 - costheta) + (bw - cv) * sintheta;
            m[3, 1] = (b * (uu + ww) - v * (au + cw)) * (1 - costheta) + (cu - aw) * sintheta;
            m[3, 2] = (c * (uu + vv) - w * (au + bv)) * (1 - costheta) + (av - bu) * sintheta;
            m[3, 3] = 1;

            return m;
        }

        /// <summary>
        /// 归一化向量
        /// </summary>
        /// <param name="v"></param>
        public static void Normalize(ref Vector<double> v)
        {
            //计算二阶范数
            double powSum = 0;
            for (int i = 0; i < v.Count; i++)
                powSum += v[i] * v[i];
            powSum = Math.Sqrt(powSum);

            //归一化
            for (int i = 0; i < v.Count; i++)
                v[i] /= powSum;
        }

        /// <summary>
        /// 获取和rpd1与rpd2在空间中垂直的经过rpd1的两个顶点
        /// </summary>
        /// <param name="rpd1"></param>
        /// <param name="rpd2"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        public static void GetVerticalVector(RobotPositionData rpd1, RobotPositionData rpd2, out Vector<double> v1, out Vector<double> v2)
        {
            double a1 = rpd1.X, b1 = rpd1.Y, c1 = rpd1.Z;
            double a2 = rpd2.X, b2 = rpd2.Y, c2 = rpd2.Z;

            //计算垂直于该线段的点坐标
            //double a3 = rpd1.X - StaticVar.Parameters.workpieceLength * Math.Sin(rpd1.Ry);
            //double b3 = rpd1.Y + StaticVar.Parameters.workpieceLength * Math.Sin(rpd1.Rx);
            //double c3 = rpd1.Z + StaticVar.Parameters.workpieceLength * (2 - Math.Cos(rpd1.Rx) - Math.Cos(rpd1.Ry));
            double a3, b3, c3;
            GetVerticalPoint(rpd1, out a3, out b3, out c3);

            double m1 = a1 - a2, n1 = b1 - b2, p1 = c1 - c2;
            double m2 = a1 - a3, n2 = b1 - b3, p2 = c1 - c3;

            double s = p1 * m2 - p2 * m1;
            double t = m2 * n1 - m1 * n2;

            double Z = 1;
            double Y = s * Z / t + c1 * s / t + b1;
            double X = (p1 * (c1 - Z) + n1 * (b1 - Y)) / m1 + a1;

            if (rpd1.Rx == 0 && rpd1.Ry == 0)
            {
                X = rpd1.X - 100 * Math.Sin(toRadian(rpd1.Rz));
                Y = rpd1.Y + 100 * Math.Cos(toRadian(rpd1.Rz));
                Z = rpd1.Z;
            }

            v1 = Vector<double>.Build.DenseOfArray(new double[] { rpd1.X, rpd1.Y, rpd1.Z });
            v2 = Vector<double>.Build.DenseOfArray(new double[] { X, Y, Z });
        }

        /// <summary>
        /// 获取和rpd1与rpd2在空间中垂直的经过rpd1的两个顶点
        /// </summary>
        /// <param name="rpd1"></param>
        /// <param name="rpd2"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        //public static void GetVerticalVector(PointXYZR rpd1, PointXYZR rpd2, out Vector<double> v1, out Vector<double> v2)
        //{
        //    double a1 = rpd1.X, b1 = rpd1.Y, c1 = rpd1.Z;
        //    double a2 = rpd2.X, b2 = rpd2.Y, c2 = rpd2.Z;

        //    //计算垂直于该线段的点坐标
        //    //double a3 = rpd1.X - StaticVar.Parameters.workpieceLength * Math.Sin(rpd1.Ry);
        //    //double b3 = rpd1.Y + StaticVar.Parameters.workpieceLength * Math.Sin(rpd1.Rx);
        //    //double c3 = rpd1.Z + StaticVar.Parameters.workpieceLength * (2 - Math.Cos(rpd1.Rx) - Math.Cos(rpd1.Ry));
        //    double a3, b3, c3;
        //    GetVerticalPoint(rpd1, out a3, out b3, out c3);

        //    double m1 = a1 - a2, n1 = b1 - b2, p1 = c1 - c2;
        //    double m2 = a1 - a3, n2 = b1 - b3, p2 = c1 - c3;

        //    double s = p1 * m2 - p2 * m1;
        //    double t = m2 * n1 - m1 * n2;

        //    double Z = 1;
        //    double Y = s * Z / t + c1 * s / t + b1;
        //    double X = (p1 * (c1 - Z) + n1 * (b1 - Y)) / m1 + a1;

        //    if (rpd1.Rx == 0 && rpd1.Ry == 0)
        //    {
        //        X = rpd1.X - 100 * Math.Sin(toRadian(rpd1.Rz));
        //        Y = rpd1.Y + 100 * Math.Cos(toRadian(rpd1.Rz));
        //        Z = rpd1.Z;
        //    }

        //    v1 = Vector<double>.Build.DenseOfArray(new double[] { rpd1.X, rpd1.Y, rpd1.Z });
        //    v2 = Vector<double>.Build.DenseOfArray(new double[] { X, Y, Z });
        //}

        /// <summary>
        /// 获取在空间中垂直于当前点的坐标，即回撤点
        /// </summary>
        /// <param name="rpd"></param>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="newZ"></param>
        public static void GetVerticalPoint(RobotPositionData rpd, out double newX, out double newY, out double newZ)
        {
            newX = rpd.X - 100 * Math.Sin(toRadian(rpd.Ry));
            newY = rpd.Y + 100 * Math.Sin(toRadian(rpd.Rx));
            //newZ = rpd.Z - 100 * (Math.Cos(toRadian(rpd.Rx)) + Math.Cos(toRadian(rpd.Ry)));
            newZ = rpd.Z - 100 * Math.Cos(toRadian(rpd.Rx)) * Math.Cos(toRadian(rpd.Ry));
        }

        #region 废弃
        public static bool GenetateCoordiTransMatrix(string bmpname, PointF p1, PointF p2, PointF p3)
        {
            //解析图像名
            string[] items = bmpname.Substring(0, bmpname.Length - 4).Split('#');
            if (items.Length < 3)
            {
                MessageBox.Show("bmp图像文件名解析失败！");
                return false;
            }
            string[] t_xyz = items[1].Split('_');
            string[] r_xyz = items[2].Split('_');
            float tx = float.Parse(t_xyz[0]), ty = float.Parse(t_xyz[1]), tz = float.Parse(t_xyz[2]);
            float rx = float.Parse(r_xyz[0]), ry = float.Parse(r_xyz[1]), rz = float.Parse(r_xyz[2]);

            Vector<double> Po1 = Restore2OCS(p1, rx, ry, rz, tx, ty, tz);
            Vector<double> Po2 = Restore2OCS(p2, rx, ry, rz, tx, ty, tz);
            Vector<double> Po3 = Restore2OCS(p3, rx, ry, rz, tx, ty, tz);

            Vector<double> Xw = Po2 - Po1;
            Xw = Xw / Math.Sqrt(Math.Pow(Xw[0], 2) + Math.Pow(Xw[1], 2) + Math.Pow(Xw[2], 2));

            Vector<double> tmp = Po3 - Po1;
            Vector<double> Zw = Cross(Xw, tmp);
            Zw = Zw / Math.Sqrt(Math.Pow(Zw[0], 2) + Math.Pow(Zw[1], 2) + Math.Pow(Zw[2], 2));

            Vector<double> Yw = Cross(Zw, Xw);
            Yw = Yw / Math.Sqrt(Math.Pow(Yw[0], 2) + Math.Pow(Yw[1], 2) + Math.Pow(Yw[2], 2));

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0} {1} {2}\n", Po1[0], Po1[1], Po1[2]));
            sb.Append(string.Format("{0} {1} {2}\n", Xw[0], Xw[1], Xw[2]));
            sb.Append(string.Format("{0} {1} {2}\n", Yw[0], Yw[1], Yw[2]));
            sb.Append(string.Format("{0} {1} {2}\n", Zw[0], Zw[1], Zw[2]));
            FileHandler.WriteAll("coorditrans.txt", sb.ToString());

            return true;
        }

        public static bool TransMachineCodeSimplify(string sourceCodePath, string destCodePath, string bmpname)
        {
            string[] items = bmpname.Substring(0, bmpname.Length - 4).Split('#');
            if (items.Length < 3)
            {
                //MessageBox.Show("bmp图像文件名解析失败！");
                return false;
            }
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

            double Trx = Math.Atan2(T1[2, 1], T1[2, 2]) / Math.PI * 180;
            double Try = Math.Asin(-T1[2, 0]) / Math.PI * 180;
            double Trz = Math.Atan2(T1[1, 0], T1[0, 0]) / Math.PI * 180;

            Matrix<double> T2 = Matrix<double>.Build.Dense(3, 3);
            Vector<double> Po1 = null;
            using (StreamReader sr = new StreamReader("coorditrans.txt"))
            {
                string str = sr.ReadToEnd();
                string[] lines = str.Split('\n');

                string[] xyz = lines[0].Split();
                Po1 = Vector<double>.Build.DenseOfArray(new double[] { double.Parse(xyz[0]), double.Parse(xyz[1]), double.Parse(xyz[2]) });

                for (int i = 1; i < 4; i++)
                {
                    string[] vals = lines[i].Split();
                    for (int j = 0; j < 3; j++)
                    {
                        T2[i - 1, j] = float.Parse(vals[j]);
                    }
                }
            }

            //MessageBox.Show("rx,ry,rz=" + rx.ToString() + "," + ry.ToString() + "," + rz.ToString() + " trx,try,trz=" +
            //                              Trx.ToString() + "," + Try.ToString() + "," + Trz.ToString());
            using (StreamReader sr = new StreamReader(sourceCodePath))
            {
                StreamWriter sw = new StreamWriter(destCodePath);
                bool status_get_position = false;
                string line, linenew;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (line.IndexOf("P00001=") >= 0)
                    {
                        status_get_position = true;
                    }
                    if (line.IndexOf("//INST") >= 0)
                    {
                        status_get_position = false;
                    }

                    if (status_get_position)
                    {
                        string[] xyz = line.Substring(7, line.Length - 7).Split(',');
                        Vector<double> v = Vector<double>.Build.DenseOfArray(new double[] { double.Parse(xyz[0]), double.Parse(xyz[1]), double.Parse(xyz[2]), 1.0 });
                        Vector<double> newv = T1 * v;

                        Vector<double> nnewv = T2 * (Vector<double>.Build.DenseOfArray(new double[] { newv[0], newv[1], newv[2] }) - Po1);


                        double newrx = double.Parse(xyz[3]) + Trx, newry = double.Parse(xyz[4]) + Try, newrz = double.Parse(xyz[5]) + Trz;
                        linenew = line.Substring(0, 7) + nnewv[0].ToString("F3") + "," + nnewv[1].ToString("F3") + "," + nnewv[2].ToString("F3") + "," +
                            //newrx.ToString("F4") + "," + newry.ToString("F4") + "," + newrz.ToString("F4");
                            double.Parse(xyz[3]).ToString("F4") + "," + double.Parse(xyz[4]).ToString("F4") + "," + double.Parse(xyz[5]).ToString("F4");
                    }
                    else
                    {
                        linenew = line;
                    }
                    sw.WriteLine(linenew);
                }
                sw.Close();
            }

            return true;
        }

        /// <summary>
        /// 把2D图像中的点坐标转换为原始3D点云坐标
        /// </summary>
        /// <param name="basePoint"></param>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <param name="rz"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <param name="tz"></param>
        /// <returns></returns>
        public static RobotPositionData Restore2OCS(RobotPositionData basePoint, double rx, double ry, double rz, double tx, double ty, double tz)
        {
            double A = Math.Cos(rz), B = Math.Sin(rz), C = Math.Cos(ry), D = Math.Sin(ry), E = Math.Cos(rx), F = Math.Sin(rx);
            Matrix<double> T1 = DenseMatrix.OfArray(new double[,]
            {
                {A*C, A*D*F-B*E, B*F+A*D*E, tx},
                {B*C, A*E-B*D*F, B*D*E-A*F, ty},
                {-D, C*F, C*E, tz},
                {0, 0, 0, 1 } });

            double Trx = Math.Atan2(T1[2, 1], T1[2, 2]) / Math.PI * 180;
            double Try = Math.Asin(-T1[2, 0]) / Math.PI * 180;
            double Trz = Math.Atan2(T1[1, 0], T1[0, 0]) / Math.PI * 180;

            Vector<double> v = Vector<double>.Build.DenseOfArray(new double[] { basePoint.X, basePoint.Y, basePoint.Z, 1.0 });
            Vector<double> newv = T1 * v;

            double newrx = basePoint.Rx + Trx, newry = basePoint.Ry + Try, newrz = basePoint.Rz + Trz;

            return new RobotPositionData(newv[0], newv[1], newv[2], newrx, newry, newrz);
        }

        /// <summary>
        /// （以验证此函数x,y,z计算没问题）
        /// 把2D图像上的点转换到原始3D点云中去。（Z恒等于0）
        /// </summary>
        /// <param name="basePoint"></param>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        /// <param name="rz"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <param name="tz"></param>
        /// <returns></returns>
        public static Vector<double> Restore2OCS(PointF basePoint, double rx, double ry, double rz, double tx, double ty, double tz)
        {
            double A = Math.Cos(rz), B = Math.Sin(rz), C = Math.Cos(ry), D = Math.Sin(ry), E = Math.Cos(rx), F = Math.Sin(rx);
            Matrix<double> T1 = DenseMatrix.OfArray(new double[,]
            {
                {A*C, A*D*F-B*E, B*F+A*D*E, tx},
                {B*C, A*E-B*D*F, B*D*E-A*F, ty},
                {-D, C*F, C*E, tz},
                {0, 0, 0, 1 } });

            double Trx = Math.Atan2(T1[2, 1], T1[2, 2]) / Math.PI * 180;
            double Try = Math.Asin(-T1[2, 0]) / Math.PI * 180;
            double Trz = Math.Atan2(T1[1, 0], T1[0, 0]) / Math.PI * 180;

            Vector<double> v = Vector<double>.Build.DenseOfArray(new double[] { basePoint.X, basePoint.Y, 0, 1.0 });
            Vector<double> newv = T1 * v;

            //double newrx = basePoint.Rx + Trx, newry = basePoint.Ry + Try, newrz = basePoint.Rz + Trz;

            //Matrix<double> TT = DenseMatrix.OfArray(new double[,]
            //{
            //    {1, 2, 3},
            //    {4, 5, 6},
            //    {7, 8, 9} });
            //Vector<double> vv = Vector<double>.Build.DenseOfArray(new double[] { 1, 1, 1});

            return Vector<double>.Build.DenseOfArray(new double[] { newv[0], newv[1], newv[2] });
        }

        public static Vector<double> Cross(Vector<double> left, Vector<double> right)
        {
            if ((left.Count != 3 || right.Count != 3))
            {
                string message = "Vectors must have a length of 3.";
                throw new Exception(message);
            }
            Vector<double> result = new DenseVector(3);
            result[0] = left[1] * right[2] - left[2] * right[1];
            result[1] = -left[0] * right[2] + left[2] * right[0];
            result[2] = left[0] * right[1] - left[1] * right[0];

            return result;
        }
        #endregion
    }
}
