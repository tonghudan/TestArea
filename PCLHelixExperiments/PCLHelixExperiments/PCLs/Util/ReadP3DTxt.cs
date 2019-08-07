using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SoftPainter.History.Util
{
    public static class ReadP3DTxt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rpList"></param>
        /// <param name="Path"></param>
        /// <param name="startLine"></param>
        /// <param name="Length"></param>
        /// <param name="filterMultiple"></param>
        /// <param name="xZoom">x放大或缩小</param>
        /// <param name="yZoom"></param>
        /// <param name="zZoom"></param>
        /// <param name="xTransfer">x平移</param>
        /// <param name="yTransfer"></param>
        /// <param name="zTransfer"></param>
        /// <returns></returns>
        public static bool ReadTxtPoint3DsZoom(ref Point3DCollection rpList, string Path, int startLine, int Length, out double cX, out double cY, out double cZ,
            int filterMultiple = 1, bool ZoomCenter = true,
            double xZoom = 1, double yZoom = 1, double zZoom = 1, double cX2 = 0, double cY2 = 0, double cZ2 = 0,
            double xTransfer = 0, double yTransfer = 0, double zTransfer = 0)
        {
            cX = 0; cY = 0; cZ = 0;
            if (filterMultiple < 1) { filterMultiple = 1; }
            if (File.Exists(Path))
            {
                //using (StreamReader sr = new StreamReader("scan\\clipped_pointsShoes.pcd"))
                using (StreamReader sr = new StreamReader(Path))
                {
                    rpList.Clear();
                    String line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)//按行读取 line为每行的数据
                    {
                        i++;
                        if (i < startLine) continue;
                        if (i % filterMultiple == 0)
                        {
                            string[] parts = line.Trim().Split(' ');
                            if (parts.Length != Length) continue;
                            Point3D rp = new Point3D();
                            rp.X = -double.Parse(parts[0]);
                            rp.Y = -double.Parse(parts[1]);
                            rp.Z = double.Parse(parts[2]);
                            cX += rp.X;
                            cY += rp.Y;
                            cZ += rp.Z;

                            rpList.Add(rp);
                        }
                    }

                    if (rpList.Count() > 0)
                    {
                        //Point3D rp1 = rpList[0];
                        //rpList.Add(rp1);
                    }
                    else { return false; }
                }
            }
            else { return true; }

            if (ZoomCenter)
            {
                cX = cX / rpList.Count();
                cY = cY / rpList.Count();
                cZ = cZ / rpList.Count();
            }
            else
            {
                cX = cX2; cY = cY2; cZ = cZ2;
            }

            //缩放/平移/并居中显示
            for (int i = 0; i < rpList.Count(); i++)
            {
                Point3D rp = new Point3D();
                rp.X = (rpList[i].X - cX) * xZoom + xTransfer;
                rp.Y = (rpList[i].Y - cY) * yZoom + yTransfer;
                rp.Z = (rpList[i].Z - cZ) * zZoom + zTransfer;
                rpList[i] = rp;
            }


            return true;
        }

        /// <summary>
        /// 只显示，返回中心点
        /// </summary>
        /// <param name="rpList"></param>
        /// <param name="Path"></param>
        /// <param name="startLine"></param>
        /// <param name="Length"></param>
        /// <param name="pCenter"></param>
        /// <param name="filterMultiple"></param>
        /// <returns></returns>
        public static bool ReadTxtPoint3DsOutZoom(ref Point3DCollection rpList, string Path, int startLine, int Length, out Point3D pCenter, int filterMultiple = 1)
        {
            pCenter = new Point3D();
            double cX = 0; double cY = 0; double cZ = 0;

            if (filterMultiple < 1) { filterMultiple = 1; }
            if (File.Exists(Path))
            {
                //using (StreamReader sr = new StreamReader("scan\\clipped_pointsShoes.pcd"))
                using (StreamReader sr = new StreamReader(Path))
                {
                    rpList.Clear();
                    String line;
                    int i = 0;

                    while ((line = sr.ReadLine()) != null)//按行读取 line为每行的数据
                    {
                        i++;
                        if (i < startLine) continue;
                        if (i % filterMultiple == 0)
                        {
                            string[] parts = line.Trim().Split(' ');
                            if (parts.Length != Length && parts.Length != 3 && parts.Length != 6) continue;
                            Point3D rp = new Point3D();
                            rp.X = -double.Parse(parts[0]);
                            rp.Y = -double.Parse(parts[1]);
                            rp.Z = double.Parse(parts[2]);
                            cX += rp.X;
                            cY += rp.Y;
                            cZ += rp.Z;

                            rpList.Add(rp);
                        }
                    }

                    if (rpList.Count() > 0)
                    {
                        //Point3D rp1 = rpList[0];
                        //rpList.Add(rp1);
                    }
                    else { return false; }
                }
            }
            else { return true; }

            cX = cX / rpList.Count();
            cY = cY / rpList.Count();
            cZ = cZ / rpList.Count();

            pCenter.X = cX;
            pCenter.Y = cY;
            pCenter.Z = cZ;


            //缩放/平移/并居中显示
            for (int i = 0; i < rpList.Count(); i++)
            {
                Point3D rp = new Point3D();
                rp.X = rpList[i].X;
                rp.Y = rpList[i].Y;
                rp.Z = rpList[i].Z;
                rpList[i] = rp;
            }


            return true;
        }

        /// <summary>
        /// 只显示，不做居中
        /// </summary>
        /// <param name="rpList"></param>
        /// <param name="Path"></param>
        /// <param name="startLine"></param>
        /// <param name="Length"></param>
        /// <param name="filterMultiple"></param>
        /// <returns></returns>
        public static bool ReadTxtPoint3Ds(ref Point3DCollection rpList, string Path, int startLine, int Length, int filterMultiple = 1)
        {
            if (filterMultiple < 1) { filterMultiple = 1; }

            if (File.Exists(Path))
            {
                //using (StreamReader sr = new StreamReader("scan\\clipped_pointsShoes.pcd"))
                using (StreamReader sr = new StreamReader(Path))
                {
                    rpList.Clear();
                    String line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null)//按行读取 line为每行的数据
                    {
                        i++;
                        if (i < startLine) continue;
                        if (i % filterMultiple == 0)
                        {

                            string[] parts = line.Trim().Split(' ');
                            if (parts.Length != Length && parts.Length != 3 && parts.Length != 6) continue;
                            Point3D rp = new Point3D();
                            rp.X = -double.Parse(parts[0]);
                            rp.Y = -double.Parse(parts[1]);
                            rp.Z = double.Parse(parts[2]);

                            rpList.Add(rp);
                        }
                    }

                    if (rpList.Count() > 0)
                    {
                        //Point3D rp1 = rpList[0];
                        //rpList.Add(rp1);
                    }
                    else { return false; }
                }
            }
            else { return true; }



            return true;
        }

        public static double Distance(Point3D p1, Point3D p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
        }

        /// <summary>
        /// 将点按距离往目标点方向移动
        /// </summary>
        /// <param name="orgPoint"></param>
        /// <param name="targetPoint"></param>
        /// <param name="displacement"></param>
        /// <returns></returns>
        public static Point3D MovePoint(Point3D orgPoint, Point3D targetPoint, double displacement)
        {
            Point3D p3dTmp = new Point3D();
            p3dTmp.Z = orgPoint.Z;

            double xDistance = targetPoint.X - orgPoint.X;
            double yDistance = targetPoint.Y - orgPoint.Y;
            double pDistance = Distance(orgPoint, targetPoint);
            if (pDistance <= 0) return orgPoint;
            double disRate = displacement / pDistance;
            p3dTmp.X = orgPoint.X + xDistance * disRate;
            p3dTmp.Y = orgPoint.Y + yDistance * disRate;
            return p3dTmp;
        }

        /// <summary>
        /// 向中心点偏移固定距离
        /// </summary>
        /// <param name="orgP3DC"></param>
        /// <param name="displacement"></param>
        /// <returns></returns>
        public static Point3DCollection ZoomPoints(Point3DCollection orgP3DC, double displacement)
        {
            double xSum = 0, ySum = 0, zSum = 0;    //取出中心点
            for (int i = 0; i < orgP3DC.Count(); i++)
            {
                xSum += orgP3DC[i].X;
                ySum += orgP3DC[i].Y;
                zSum += orgP3DC[i].Z;
            }
            Point3D midPoint = new Point3D(xSum / orgP3DC.Count(), ySum / orgP3DC.Count(), zSum / orgP3DC.Count());

            for (int i = 0; i < orgP3DC.Count(); i++)       //循环取向中心点缩放的某个点  Convert.ToDouble(Global.pointZoom)
            {
                orgP3DC[i] = MovePoint(orgP3DC[i], midPoint, displacement);
            }

            return orgP3DC;
        }

        /// <summary>
        /// 按中心点距离等比例缩放算法
        /// </summary>
        /// <param name="orgP3DC"></param>
        /// <param name="displacement"></param>
        /// <returns></returns>
        public static Point3DCollection ZoomRatePoints(Point3DCollection orgP3DC, double displacement)
        {
            double xSum = 0, ySum = 0, zSum = 0;    //取出中心点
            for (int i = 0; i < orgP3DC.Count(); i++)
            {
                xSum += orgP3DC[i].X;
                ySum += orgP3DC[i].Y;
                zSum += orgP3DC[i].Z;
            }
            Point3D midPoint = new Point3D(xSum / orgP3DC.Count(), ySum / orgP3DC.Count(), zSum / orgP3DC.Count());

            for (int i = 0; i < orgP3DC.Count(); i++)       //循环取向中心点缩放的某个点  Convert.ToDouble(Global.pointZoom)
            {
                Point3D ptmp = new Point3D();
                ptmp.X = midPoint.X + (orgP3DC[i].X - midPoint.X) * displacement;
                ptmp.Y = midPoint.Y + (orgP3DC[i].Y - midPoint.Y) * displacement;
                ptmp.Z = orgP3DC[i].Z;
                orgP3DC[i] = ptmp;
            }

            return orgP3DC;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgP3DC"></param>
        /// <param name="displacement"></param>
        /// <returns></returns>
        public static Point3DCollection ZoomOutline(Point3DCollection orgP3DC, float displacement)
        {
            Point3DCollection outP3DC = new Point3DCollection();
            List<Vector3D> pList = new List<Vector3D>();

            List<Vector3D> dpList = new List<Vector3D>();
            List<Vector3D> ndpList = new List<Vector3D>();

            List<Vector3D> outList = new List<Vector3D>();

            int count = orgP3DC.Count();

            for (int i = 0; i < count; i++)
            {
                Vector3D v3d = new Vector3D(orgP3DC[i].X, orgP3DC[i].Y, 0);
                pList.Add(v3d);
            }

            for (int i = 0; i < count; i++)
            {
                int next = (i == (count - 1) ? 0 : (i + 1));
                dpList.Add(pList[next] - pList[i]);
                float unitLen = 1.0f / (float)Math.Sqrt(Dot(dpList[i], dpList[i]));
                ndpList.Add(dpList[i] * unitLen);
            }

            float SAFELINE = displacement;
            for (int i = 0; i < count; i++)
            {
                int startIndex = (i == 0 ? (count - 1) : (i - 1));
                int endIndex = i;
                float sinTheta = (float)Cross(ndpList[startIndex], ndpList[endIndex]); if (sinTheta == 0) { sinTheta = 0.00001f; }//避免为零的情况
                Vector3D orientVector = ndpList[endIndex] - ndpList[startIndex];//i.e. PV2-V1P=PV2+PV1
                Vector3D temp_out = new Vector3D();
                temp_out.X = pList[i].X + SAFELINE / sinTheta * orientVector.X;
                temp_out.Y = pList[i].Y + SAFELINE / sinTheta * orientVector.Y;
                outList.Add(temp_out);
            }

            for (int i = 0; i < count; i++)
            {
                Point3D p3tmp = new Point3D();
                p3tmp.X = outList[i].X;
                p3tmp.Y = outList[i].Y;
                p3tmp.Z = orgP3DC[i].Z;
                outP3DC.Add(p3tmp);
            }
            return outP3DC;
        }




        public static double Dot(Vector3D a, Vector3D b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public static double Cross(Vector3D a, Vector3D b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
    }

}
