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

        }
    }

    public class ViewModel: BaseViewModel
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
