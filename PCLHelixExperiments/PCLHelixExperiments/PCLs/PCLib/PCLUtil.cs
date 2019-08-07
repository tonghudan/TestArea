using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Media.Media3D;
using System.IO;
using SoftPainter.Core.Base;

namespace PCLs
{
    public class PCLUtil
    {
        public static bool PCL_rotationPCD(string inputpcdfile, string outputpcdfile, float x1, float y1, float z1, float x2, float y2, float z2, float angle)
        {
            try
            {
                return PCLApi.pcdRotationArbitraryAxisapi(inputpcdfile, outputpcdfile, x1, y1, z1, x2, y2, z2, angle);
            }
            catch(Exception err)
            {
                Trace.TraceError(err.ToString());
                return false;
            }
        }

        public static bool PCL_calibrateMatrixFileConvert(string sourceCommaSplitFile, string destSpaceSplitFile)
        {
            try
            {
                if (File.Exists(destSpaceSplitFile))
                {
                    DateTime now = DateTime.Now;
                    File.Copy(destSpaceSplitFile, string.Format("{0}.{1}.{2:D2}.{3:D2}.{4:D2}.{5:D2}.{6:D2}.backup",
                        destSpaceSplitFile, now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second), true);
                }

                using (StreamReader sr = new StreamReader(sourceCommaSplitFile))
                using (StreamWriter sw = new StreamWriter(destSpaceSplitFile, false))
                {
                    string matrix_comma = sr.ReadToEnd();
                    string matrix_space = matrix_comma.Replace(',', ' ');
                    sw.Write(matrix_space);
                }

                return true;
            }
            catch(Exception err)
            {
                Trace.TraceError(err.ToString());
                return false;
            }
        }

        public static string PCL_pcd2image(string pcdfile, int meanK, int kernel, int planar_threshold, int minx, int maxx, int miny, int maxy, int minz, int maxz, int min_cloud_size = 100)
        {
            try
            {
                StringBuilder result_str = new StringBuilder(1000);
                int len = PCLApi.pcd2imageapi(pcdfile, result_str,
                    meanK: meanK, kernel_size: kernel, planar_threshold: planar_threshold,
                    minx: minx, maxx: maxx, miny: miny, maxy: maxy, minz: minz, maxz: maxz, 
                    min_cloud_size: min_cloud_size);
                if (len < 0) return null;
                string image_name = result_str.ToString().Substring(0, len).Trim();
                return image_name;
            }
            catch(Exception err)
            {
                Trace.TraceError(err.ToString());
                return null;
            }
        }

        public static int PCL_Preprocess()
        {

            return 0;
        }

        public static bool PCL_calcTransform(Point3D p1, Point3D p2, Point3D p3, Point3D pw1, Point3D pw2, Point3D pw3, out double Trx, out double Try, out double Trz)
        {
            try
            {
                StringBuilder result_str = new StringBuilder(1000);
                int len = PCLApi.calcTransformapi(
                    new double[3] { p1.X, p1.Y, p1.Z }, new double[3] { p2.X, p2.Y, p2.Z }, new double[3] { p3.X, p3.Y, p3.Z },
                    new double[3] { pw1.X, pw1.Y, pw1.Z }, new double[3] { pw2.X, pw2.Y, pw2.Z }, new double[3] { pw3.X, pw3.Y, pw3.Z },
                    result_str);
                string matrix_str = result_str.ToString().Substring(0, len).Trim();
                Translation translation = new Translation(matrix_str.Split());
                Trx = translation.Trx; Try = translation.Try; Trz = translation.Trz;
                return true;
            }
            catch (Exception err)
            {
                Trx = 0; Try = 0; Trz = 0;
                Trace.TraceError(err.ToString());
                return false;
            }
        }

        public static bool PCL_downsampling(string inputfile, string outputfile)
        {
            string output_dowsample = inputfile.Replace(".pcd", "_downsample.pcd");
            string output_outlierremoval = inputfile.Replace(".pcd", "_outlierremoval.pcd");

            int resCount = PCLApi.downsamplingapi(inputfile, output_dowsample, output_outlierremoval, outputfile, radius: 50, std_dev_mul: (float)2.0);
            return resCount > 2000; //
        }


        public static bool PCL_downsamplingxyz(string inputfile, string outputfile, float minx, float maxx, float miny, float maxy, float minz, float maxz)
        {
            string output_dowsample = inputfile.Replace(".pcd", "_downsample.pcd");
            string output_outlierremoval = inputfile.Replace(".pcd", "_outlierremoval.pcd");

            int resCount = PCLApi.downsamplingxyzapi(inputfile, output_dowsample, output_outlierremoval, outputfile, minx, maxx, miny, maxy, minz, maxz, radius: 50, std_dev_mul: (float)2.0, min_cloud_size: 7000);
            return resCount > 2000; //
        }

        public static bool PCL_downsamplingxyz_sor_voxel(string inputfile, string outputfile, float minx, float maxx, float miny, float maxy, float minz, float maxz)
        {
            string output_dowsample = inputfile.Replace(".pcd", "_downsample.pcd");
            string output_outlierremoval = inputfile.Replace(".pcd", "_outlierremoval.pcd");

            int resCount = PCLApi.downsamplingxyz_sor_voxelapi(inputfile, output_dowsample, output_outlierremoval, outputfile, minx, maxx, miny, maxy, minz, maxz, meanK: 20, radius: 50, std_dev_mul: (float)1.2, min_cloud_size: 7000);
            return resCount > 2000; //
        }

        /// <summary>
        /// 点云三角化重建，待完善
        /// </summary>
        /// <param name="inputfile"></param>
        /// <param name="outputfile"></param>
        public static void PCL_surfaceReconstruct(string inputfile, string outputfile)
        {
            PCLApi.greedyTriangulationapi(inputfile, outputfile, search_radius: 50, mu: 15);
        }

        //public static void Meshlab_Viewer(string filename)
        //{
        //    if(!File.Exists(Properties.Settings.Default.meshlab_install_path))
        //    {
        //        System.Windows.Forms.MessageBox.Show("文件：\"" + Properties.Settings.Default.meshlab_install_path + "\"不存在");
        //        return;
        //    }

        //    string absolutePath = System.IO.Path.Combine(FileIO.BaseDirectory, filename);
        //    if (File.Exists(absolutePath))
        //        filename = absolutePath;

        //    ProcessStartInfo startInfo = new ProcessStartInfo(Properties.Settings.Default.meshlab_install_path);
        //    startInfo.UseShellExecute = false;
        //    startInfo.CreateNoWindow = true;

        //    startInfo.Arguments = filename;
        //    Process p = Process.Start(startInfo);
        //}
        public static void Meshlab_Viewer(string filename)
        {
            string strMeshLabPath = "";
            try
            {
                string softName = "meshlab";
                string strKeyName = string.Empty;
                string softPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\";
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.LocalMachine;
                Microsoft.Win32.RegistryKey regSubKey = regKey.OpenSubKey(softPath + softName + ".exe", false);

                object objResult = regSubKey.GetValue(strKeyName);
                Microsoft.Win32.RegistryValueKind regValueKind = regSubKey.GetValueKind(strKeyName);
                if (regValueKind == Microsoft.Win32.RegistryValueKind.String)
                {
                    strMeshLabPath = objResult.ToString();
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("请确认安装了MeshLab"); return;
            }


            if (strMeshLabPath == "" && !File.Exists(strMeshLabPath))
            {
                System.Windows.MessageBox.Show("文件：\"" + strMeshLabPath + "\"不存在");
                return;
            }

            //string absolutePath = System.IO.Path.Combine(FileIO.BaseDirectory, filename);
            string absolutePath = filename;
            if (File.Exists(absolutePath))
                filename = absolutePath;

            ProcessStartInfo startInfo = new ProcessStartInfo(strMeshLabPath);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            startInfo.Arguments = filename;
            Process p = Process.Start(startInfo);
            p.WaitForExit();
        }

    }
}
