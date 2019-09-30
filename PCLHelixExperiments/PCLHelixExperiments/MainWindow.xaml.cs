using HelixToolkit.Wpf;
using PCLHelixExperiments.PCLs;
using PCLs.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PCLHelixExperiments
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        #region 点云 Visual3D

        private PointsVisual3D pointsVisualReal;
        private static Point3DCollection rpListReal = new Point3DCollection();

        private Point3DCollection pointsReal;
        public Point3DCollection PointsReal
        {
            get
            {
                return this.pointsReal;
            }

            set
            {
                this.pointsReal = value;

            }
        }

        public static IEnumerable<Point3D> GeneratePointsReal()
        {
            for (int i = 0; i < rpListReal.Count; i++)
            {

                var pt = rpListReal[i];
                yield return pt;
                if (i > 0 && i < rpListReal.Count - 1)
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private PointsVisual3D pointsVisualConvert;
        private static Point3DCollection rpListConvert = new Point3DCollection();

        private Point3DCollection pointsConvert;
        public Point3DCollection PointsConvert
        {
            get
            {
                return this.pointsConvert;
            }

            set
            {
                this.pointsConvert = value;

            }
        }

        public static IEnumerable<Point3D> GeneratePointsConvert()
        {
            for (int i = 0; i < rpListConvert.Count; i++)
            {

                var pt = rpListConvert[i];
                yield return pt;
                if (i > 0 && i < rpListConvert.Count - 1)
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void RealPointShow(string path = "")
        {
            if (path == "")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "scan/board.pcd"; //点云数据
            }

            ReadP3DTxt.ReadTxtPoint3DsOutZoom(ref rpListReal, path, 13, 6, out Point3D pCenter);
            //ReadP3DTxt.ReadTxtPoint3Ds(ref rpListReal, path, 13, 6);
            vp.CameraController.CameraTarget = pCenter;

            //PCDReader<PointXYZ> reader = new PCDReader<PointXYZ>();
            //PointCloud<PointXYZ> pCloud = reader.Read(path);
            

            Point3D pCenterTmp = new Point3D();
            pCenterTmp.X = 0;
            pCenterTmp.Y = 4500;
            pCenterTmp.Z = 0;
            vp.Camera.Position = pCenterTmp;
            vp.Camera.LookDirection = new Vector3D(0, -4501, 0);
            vp.Camera.UpDirection = new Vector3D(0, 0, -1);


            //显示模型
            if (!vp.Children.Contains(pointsVisualReal))
            {
                this.pointsVisualReal = new PointsVisual3D { Color = Colors.Yellow, Size = 2 };
                vp.Children.Add(this.pointsVisualReal);
            }
            //this.pointsVisualReal.Size = 2;
            this.PointsReal = new Point3DCollection(GeneratePointsReal());
            this.pointsVisualReal.Points = this.PointsReal;
        }



        private void ConvertPointShow(string path)
        {
            if (path == "")
            {
                return;
            }

            //ReadP3DTxt.ReadTxtPoint3DsOutZoom(ref rpListConvert, path, 13, 6, out Point3D pCenter);

            //显示鞋子的模型
            if (!vp.Children.Contains(pointsVisualConvert))
            {
                this.pointsVisualConvert = new PointsVisual3D { Color = Colors.OrangeRed, Size = 2 };
                vp.Children.Add(this.pointsVisualConvert);
            }

            this.PointsConvert = new Point3DCollection(GeneratePointsConvert());
            this.pointsVisualConvert.Points = this.PointsConvert;
            //create360();
            //显示别选择物框
            //this.DataContext = this.viewModel = new ViewModel();
            //this.viewModel.Select(this.pointsVisual);

            //model3DGroup = new Model3DGroup();
            //model3DGroup.Children.Add(this.pointsVisual);
        }

        #endregion

        #region 界面触发和按钮事件区

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //添加底部网格
            GridLines.Center = new Point3D(0, 0, 0);
            GridLines.Width = 5000;
            GridLines.Length = 5000;
            GridLines.Thickness = 1;
            GridLines.MajorDistance = 200;
            GridLines.MinorDistance = 50;
            //添加门处网格
            GridLinesCenter.Center = new Point3D(0, 0, 0);
            GridLinesCenter.Width = 1600;
            GridLinesCenter.Length = 1600;
            GridLinesCenter.Thickness = 2;
            GridLinesCenter.MajorDistance = 100;
            GridLinesCenter.MinorDistance = 25;
            //相机设置
            vp.Camera.Position = new Point3D(0, 0, -4501);
            vp.Camera.LookDirection = new Vector3D(0, 0, 4500);
            vp.Camera.UpDirection = new Vector3D(-1, -1, -1);
            //vp.CameraController.CameraTarget = new Point3D(548.769, 452.701, 51.000);
        }

        private void Grid2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mouseposition = e.GetPosition(vp);    //vp为目标Viewport3D对象 <Viewport3D x:Name="vp"> 
            Point3D testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
            Vector3D testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);
            PointHitTestParameters pointparams = new PointHitTestParameters(mouseposition);
            RayHitTestParameters rayparams = new RayHitTestParameters(testpoint3D, testdirection);
            VisualTreeHelper.HitTest(vp, null, HTResult, pointparams);
        }
        /// <summary>
        /// 点击模型事件
        /// </summary>
        /// <param name="rawresult"></param>
        /// <returns></returns>
        public HitTestResultBehavior HTResult(System.Windows.Media.HitTestResult rawresult)
        {
            RayHitTestResult rayResult = rawresult as RayHitTestResult;
            if (rayResult != null)
            {
                RayMeshGeometry3DHitTestResult rayMeshResult = rayResult as RayMeshGeometry3DHitTestResult;
                if (rayMeshResult != null)
                {
                    GeometryModel3D hitgeo = rayMeshResult.ModelHit as GeometryModel3D;
                    MeshGeometry3D hitmesh = hitgeo.Geometry as MeshGeometry3D;
                    //获取当前点击的点
                    Point3D po3D = rayMeshResult.PointHit;

                    Point3D pa3D = hitmesh.Positions[rayMeshResult.VertexIndex1];
                    Point3D pb3D = hitmesh.Positions[rayMeshResult.VertexIndex2];
                    Point3D pc3D = hitmesh.Positions[rayMeshResult.VertexIndex3];


                    //Point3D pTmp = Control.PositionCalculation.pointTransDoorCoordinate(po3D); //将View3D转成物体坐标系
                    Point3D pTmp = po3D; //将View3D转成物体坐标系

                    //在控件上显示坐标
                    //this.Title = " x=" + pTmp.X.ToString() + " y=" + pTmp.Y.ToString() + " z= " + pTmp.Z.ToString();
                    //PointInfoDisplayShow();
                    //labelPointInfoDisplay.Content = "Position:\r\n x=" + ((decimal)pTmp.X).ToString("F8") + "\r\n y=" + ((decimal)pTmp.Y).ToString("F8") + "\r\n z= " + ((decimal)pTmp.Z).ToString("F8");
                    //labelPointInfoDisplay.Content += "\r\n\r\nCamera:";
                    //labelPointInfoDisplay.Content += ("\r\n LookDirection=" + camera.LookDirection.ToString());
                    //labelPointInfoDisplay.Content += ("\r\n FarPlaneDistance=" + camera.FarPlaneDistance.ToString());
                    //labelPointInfoDisplay.Content += ("\r\n UpDirection=" + camera.UpDirection.ToString());
                    //labelPointInfoDisplay.Content += ("\r\n NearPlaneDistance=" + camera.NearPlaneDistance.ToString());
                    //labelPointInfoDisplay.Content += ("\r\n Position=" + camera.Position.ToString());
                    //labelPointInfoDisplay.Content += ("\r\n FieldOfView=" + camera.FieldOfView.ToString());
                    ////labelPointInfoDisplay.Content += "\r\n\r\nTrackballDecorator:";
                    ////labelPointInfoDisplay.Content += ("\r\n ActualHeight=" + toolsTrackballDecorator.ActualHeight.ToString());
                    ////labelPointInfoDisplay.Content += ("\r\n ActualWidth=" + toolsTrackballDecorator.ActualWidth.ToString());
                    ////labelPointInfoDisplay.Content += ("\r\n FlowDirection=" + toolsTrackballDecorator.FlowDirection.ToString());
                    ////labelPointInfoDisplay.Content += ("\r\n LayoutTransform.value=" + toolsTrackballDecorator.LayoutTransform.Value.ToString());
                    ////labelPointInfoDisplay.Content += ("\r\n Width=" + toolsTrackballDecorator.Width.ToString());
                    //labelPointInfoDisplay.Content += "\r\n\r\nClick Face Vertex A:";
                    //labelPointInfoDisplay.Content += ("\r\n A.x=" + pa3D.X.ToString() + "\r\n A.y=" + pa3D.Y.ToString() + "\r\n A.z= " + pa3D.Z.ToString());
                    //labelPointInfoDisplay.Content += "\r\n\r\nClick Face Vertex B:";
                    //labelPointInfoDisplay.Content += ("\r\n B.x=" + pb3D.X.ToString() + "\r\n B.y=" + pb3D.Y.ToString() + "\r\n B.z= " + pb3D.Z.ToString());
                    //labelPointInfoDisplay.Content += "\r\n\r\nClick Face Vertex C:";
                    //labelPointInfoDisplay.Content += ("\r\n C.x=" + pc3D.X.ToString() + "\r\n C.y=" + pc3D.Y.ToString() + "\r\n C.z= " + pc3D.Z.ToString());



                    //if (bPointPaintKey)
                    //    ShowClickPoint(pTmp, pa3D, pb3D, pc3D, TrajGlobalParam.dHigh);
                    ////ShowClickPoint(pTmp);

                    //labelPointInfoDisplay.Content += "\r\n\r\nPointXYZR:";
                    //labelPointInfoDisplay.Content += ("\r\n PointXYZR.x=" + pStart.pX.ToString() + "\r\n PointXYZR.y=" + pStart.pY.ToString() + "\r\n PointXYZR.z= " + pStart.pZ.ToString() + "\r\n PointXYZR.Rx=" + pStart.pRx.ToString() + "\r\n PointXYZR.Ry=" + pStart.pRy.ToString() + "\r\n PointXYZR.Rz= " + pStart.pRz.ToString());

                }
                var visual3D = rawresult.VisualHit;//使用visual3D作为命中的模型
                if (visual3D != null) //如果不为空，即确实命中了某一个模型
                {
                    //开启点选，选择当前点击的三角序号
                    //if (TrajGlobalParam.SelectClickPaint3D)
                    //{
                    //    for (int i = 0; i < 1500; i++)
                    //    {
                    //        if (MVpaint3Ds[i] == visual3D)
                    //        {
                    //            if (TrajGlobalParam.SelectClickCtrlDown)
                    //            {
                    //                //添加当前点选的序号到列表中
                    //                TrajGlobalParam.SelectedIndexList.Add(i);
                    //                ShowPointsSelectted(TrajGlobalParam.SelectedIndexList);

                    //                ListView.SelectedItems.Clear();
                    //                List<int> listPcTmp = new List<int>(TrajGlobalParam.SelectedIndexList);
                    //                for (int j = 0; j < listPcTmp.Count; j++)
                    //                {
                    //                    //listPcTmp[j] = listPcTmp[j];
                    //                    ListView.SelectedItems.Add(ListView.Items[listPcTmp[j]]);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                ListView.SelectedIndex = i;
                    //            }
                    //            //messageShow(i.ToString());
                    //        }
                    //    }
                    //    //button_SelectDoorPic.Content = "点选";
                    //    //GlobalParam.SelectClickPaint3D = false;

                    //    //if (vp.FindName("BoxTita10_1") == visual3D) //用FindName方法进行检索。如果相等，执行一些事件
                    //    //{
                    //    //    MessageBox.Show("BoxTita10_1");
                    //    //}

                    //    /*
                    //     * 可以根据自行添加
                    //     */
                    //}
                    return HitTestResultBehavior.Stop;
                }
            }
            return HitTestResultBehavior.Continue;
        }


        private void Grid2_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Grid2_KeyUp(object sender, KeyEventArgs e)
        {

        }


        #endregion

        private void btPCDRead_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog path = new System.Windows.Forms.OpenFileDialog();
            path.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "scan"; //设置初始路径为 当前程序路径 + scan目录
            path.RestoreDirectory = true;
            path.Filter = "ply 文件(*.ply,*.pcd)|*.ply;*.pcd|All files(*.*)|*.*";
            string strPath = "";
            string strFileName = "";
            if (path.ShowDialog() == System.Windows.Forms.DialogResult.OK)//选择pcd文件路径
            {
                strPath = path.FileName;
                strFileName = path.SafeFileName;
            }
            else return;

            string filePath = System.IO.Path.GetDirectoryName(strPath);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(strPath);
            //string file2Red = strPath.Trim().TrimEnd(strFileName.ToCharArray()) + strFileName.Replace(".pcd", ".txt");
            string file4Read = filePath + "pcd.txt";

            PCLApi.pcd2pcdtxtapi(strPath, file4Read);
            //PCLApi.transformtocenterzeroapi(strPath, file2Red);

            RealPointShow(file4Read);
        }

        private void btLoadModel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog path = new System.Windows.Forms.OpenFileDialog();
            path.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "scan"; //设置初始路径为 当前程序路径 + scan目录
            path.RestoreDirectory = true;
            path.Filter = "All files(*.*)|*.*";
            string strPath = "";
            string strFileName = "";
            if (path.ShowDialog() == System.Windows.Forms.DialogResult.OK)//选择pcd文件路径
            {
                strPath = path.FileName;
                strFileName = path.SafeFileName;
            }
            else return;

            ModelImporter importer =  new ModelImporter();
            Model3DGroup link = importer.Load(strPath);

        }

        private void btExportModel_Click(object sender, RoutedEventArgs e)
        {
            //exporter = new Exporter();
        }
    }
}
