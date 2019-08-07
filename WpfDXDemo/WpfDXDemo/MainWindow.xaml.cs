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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpDX;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Model.Scene;
using WpfDXDemo.DemoCore;
//using System.Windows.Media.Media3D;
//using HelixToolkit.Wpf;
using AxisAngleRotation3D = System.Windows.Media.Media3D.AxisAngleRotation3D;
using Point3DCollection = System.Windows.Media.Media3D.Point3DCollection;
using Point3D = System.Windows.Media.Media3D.Point3D;
using Rect3D = System.Windows.Media.Media3D.Rect3D;
using Size3D = System.Windows.Media.Media3D.Size3D;
using RotateTransform3D = System.Windows.Media.Media3D.RotateTransform3D;
using Transform3D = System.Windows.Media.Media3D.Transform3D;
using Transform3DGroup = System.Windows.Media.Media3D.Transform3DGroup;
using TranslateTransform3D = System.Windows.Media.Media3D.TranslateTransform3D;
using Vector3D = System.Windows.Media.Media3D.Vector3D;
using Color = System.Windows.Media.Color;
using Plane = SharpDX.Plane;
using Vector3 = SharpDX.Vector3;
using Colors = System.Windows.Media.Colors;
using Color4 = SharpDX.Color4;

namespace WpfDXDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private EffectsManager manager;
        private Viewport3DX viewport;
        private Models models = new Models();
        private ViewModel viewmodel = new ViewModel();
        private SceneNodeGroupModel3D sceneNodeGroup;


        public MainWindow()
        {
            InitializeComponent();
            manager = new DefaultEffectsManager();
            DataContext = viewmodel;
        }

        private void buttonInit_Click(object sender, RoutedEventArgs e)
        {
            //初始化viewpoint
            viewport = new Viewport3DX();
            viewport.Background = Brushes.Transparent;//设置背景
            viewport.ShowCoordinateSystem = true;//显示坐标系
            viewport.ShowFrameRate = true;//显示帧率
            viewport.CameraRotationMode = CameraRotationMode.Trackball;//相机模式
            viewport.InfiniteSpin = false;//是否启用了无限旋转
            viewport.InfoBackground = Brushes.Transparent; //信息显示框的背景
            viewport.InfoForeground = Brushes.DarkGray;

            //viewport.Camera.LookDirection = new Vector3D(3.210, -3000.933, 4994.820);
            //viewport.Camera.UpDirection = new Vector3D(0.004, -0.857, -0.515);
            //viewport.Camera.Position = new Point3D(545.559, 3453.634, -4943.820);
            //viewport.ShowCameraTarget = true;


            viewport.EffectsManager = manager;
            viewport.Items.Add(new DirectionalLight3D() { Direction = new Vector3D(-1, -1, -1) });
            viewport.Items.Add(new DirectionalLight3D() { Direction = new Vector3D(1, 1, 1) });
            //viewport.Items.Add(new AmbientLight3D() { Color = Color.FromArgb(255, 50, 50, 50) });
            //viewport.Items.Add(new AmbientLight3D() { Color = Color.FromRgb(3,60,98) });
            //sceneNodeGroup = new SceneNodeGroupModel3D();
            //viewport.Items.Add(sceneNodeGroup);
            //viewport.MouseDown3D += Viewport_MouseDown3D;


            Grid.SetColumn(viewport, 0);
            mainGrid.Children.Add(viewport);
            buttonInit.IsEnabled = false;
            //buttonRemoveViewport.IsEnabled = true;
            //viewmodel.EnableButtons = true;
        }

        /// <summary>
        /// 鼠标按下触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Viewport_MouseDown3D(object sender, RoutedEventArgs e)
        {
            if (e is MouseDown3DEventArgs args && args.HitTestResult != null)
            {
                var model = args.HitTestResult.ModelHit;
            }
        }

        private void buttonDoorBlock_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(DateTime.Now + ":" + DateTime.Now.Millisecond);
            CreateDoorBlock();
            Console.WriteLine(DateTime.Now + ":" + DateTime.Now.Millisecond);
        }

        /// <summary>
        /// 用正方形点组成门
        /// </summary>
        //private PointsVisual3D[,] DoorpointsVisuals = new PointsVisual3D[200, 80];
        //private MeshBuilder[,] DoorpointsVisuals = new MeshBuilder[200, 80];
        private MeshGeometryModel3D[,] DoorpointsVisuals = new MeshGeometryModel3D[200, 80];
        //MeshElement3D

        /// <summary>
        /// 创建覆盖门的正方体
        /// </summary>
        /// <param name="size"></param>
        private void CreateDoorBlock(double size = 1)
        {
            for (int i = 0; i < 200 / size; i++)
            {
                for (int j = 0; j < 80 / size; j++)
                {
                    MeshGeometry3D p3dc = new MeshGeometry3D();
                    Point3D p3d = new Point3D();
                    p3d.X = size * i + size / 2;
                    p3d.Y = size * j + size / 2;
                    p3d.Z = 0;
                    //DoorpointsVisuals[i, j] = new MeshBuilder();
                    MeshBuilder meshBr = new MeshBuilder();
                    Rect3D rect3D = new Rect3D(p3d, new Size3D(size, size, size));
                    meshBr.AddBox(rect3D);

                    MeshGeometryModel3D mgm3d = new MeshGeometryModel3D();
                    mgm3d.Geometry = meshBr.ToMesh();
                    mgm3d.Material = PhongMaterials.Red;
                    mgm3d.Transform = new TranslateTransform3D(0, 0, 0);

                    DoorpointsVisuals[i, j] = new MeshGeometryModel3D();
                    DoorpointsVisuals[i, j] = mgm3d;

                    if (!viewport.Items.Contains(DoorpointsVisuals[i, j]))
                    {
                        viewport.Items.Add(DoorpointsVisuals[i, j]);
                        //Element3D.
                    }

                }
            }

            //if (!vp.Children.Contains(pointsVisual))
            //{
            //    vp.Children.Add(this.pointsVisual);
            //}

            //this.pointsVisual.Size = size - 1;
            //this.Points = new Point3DCollection(GeneratePoints());
            //this.pointsVisual.Points = this.Points;
        }

    }

    /// <summary>
    /// 自定义ViewModel
    /// </summary>
    public class ViewModel : BaseViewModel
    {
        private bool enableButtons = false;
        public bool EnableButtons
        {
            set
            {
                if (SetValue(ref enableButtons, value))
                {
                    OnPropertyChanged(nameof(EnableEnvironmentButtons));
                }
            }
            get { return enableButtons; }
        }

        private bool enableEnvironmentButtons = true;
        public bool EnableEnvironmentButtons
        {
            set
            {
                SetValue(ref enableEnvironmentButtons, value);
            }
            get { return enableEnvironmentButtons && enableButtons; }
        }
    }

    public class Models
    {
        private IList<Geometry3D> models { get; } = new List<Geometry3D>();
        private PhongMaterialCollection materials = new PhongMaterialCollection();
        private Random rnd = new Random();
        public Models()
        {
            var builder = new MeshBuilder();
            builder.AddSphere(Vector3.Zero, 1);
            models.Add(builder.ToMesh());

            builder = new MeshBuilder();
            builder.AddBox(Vector3.Zero, 1, 1, 1);
            models.Add(builder.ToMesh());

            builder = new MeshBuilder();
            builder.AddDodecahedron(Vector3.Zero, new Vector3(1, 0, 0), new Vector3(0, 1, 0), 1);
            models.Add(builder.ToMesh());
        }

        public MeshGeometryModel3D GetModelRandom()
        {
            var idx = rnd.Next(0, models.Count);
            MeshGeometryModel3D model = new MeshGeometryModel3D() { Geometry = models[idx], CullMode = SharpDX.Direct3D11.CullMode.Back };
            var scale = new System.Windows.Media.Media3D.ScaleTransform3D(rnd.NextDouble(1, 5), rnd.NextDouble(1, 5), rnd.NextDouble(1, 5));
            var translate = new System.Windows.Media.Media3D.TranslateTransform3D(rnd.NextDouble(-20, 20), rnd.NextDouble(-20, 20), rnd.NextDouble(-20, 20));
            var group = new System.Windows.Media.Media3D.Transform3DGroup();
            group.Children.Add(scale);
            group.Children.Add(translate);
            model.Transform = group;
            var material = materials[rnd.Next(0, materials.Count - 1)];
            model.Material = material;
            if (material.DiffuseColor.Alpha < 1)
            {
                model.IsTransparent = true;
            }
            return model;
        }

        public MeshNode GetSceneNodeRandom()
        {
            var idx = rnd.Next(0, models.Count);
            MeshNode model = new MeshNode() { Geometry = models[idx], CullMode = SharpDX.Direct3D11.CullMode.Back };
            var scale = SharpDX.Matrix.Scaling((float)rnd.NextDouble(1, 5), (float)rnd.NextDouble(1, 5), (float)rnd.NextDouble(1, 5));
            var translate = SharpDX.Matrix.Translation((float)rnd.NextDouble(-20, 20), (float)rnd.NextDouble(-20, 20), (float)rnd.NextDouble(-20, 20));
            model.ModelMatrix = scale * translate;
            var material = materials[rnd.Next(0, materials.Count - 1)];
            model.Material = material;
            if (material.DiffuseColor.Alpha < 1)
            {
                model.IsTransparent = true;
            }
            return model;
        }
    }

}
