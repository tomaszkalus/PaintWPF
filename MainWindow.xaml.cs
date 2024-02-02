using System;
using System.ComponentModel;


using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Grafika_lab_1_TK.Shapes;
using Grafika_lab_1_TK.Tools;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace Grafika_lab_1_TK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LineTool lineTool;
        private readonly BrushTool brushTool;
        private readonly EllipseTool ellipseTool;
        private readonly PathTool pathTool;
        private readonly PolygonTool polygonTool;
        private readonly RectangleTool rectangleTool;
        private readonly TriangleShape triangleShape;
        private readonly RectangleShape rectangleShape;
        private readonly StarShape starShape;

        private double[,] kernelMatrix = new double[3, 3];

        private readonly MainViewModel mainViewModel;
        private MainViewModel.Tools selectedTool => mainViewModel.SelectedTool;
        
        public MainWindow()
        {

            mainViewModel = new MainViewModel();
            InitializeComponent();
            this.DataContext = this.mainViewModel;

            lineTool = new LineTool(paintSurface, mainViewModel);
            brushTool = new BrushTool(paintSurface, mainViewModel);
            ellipseTool = new EllipseTool(paintSurface, mainViewModel);
            pathTool = new PathTool(paintSurface, mainViewModel);
            polygonTool = new PolygonTool(paintSurface, mainViewModel);
            rectangleTool = new RectangleTool(paintSurface, mainViewModel);
            triangleShape = new TriangleShape(paintSurface, mainViewModel);
            rectangleShape = new RectangleShape(paintSurface, mainViewModel);
            starShape = new StarShape(paintSurface, mainViewModel);

            SetIdentityKernel();
            //ApplyFilter();
        }



        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            switch (selectedTool)
            {
                case MainViewModel.Tools.Brush:
                    brushTool.MouseMove(sender, e);
                    break;
                case MainViewModel.Tools.Line:
                    lineTool.MouseMove(sender, e);
                    break;
                case MainViewModel.Tools.Ellipse:
                    ellipseTool.MouseMove(sender, e);
                    break;
                case MainViewModel.Tools.Path:
                    pathTool.MouseMove(sender, e);
                    break;
                case MainViewModel.Tools.Polygon:
                    polygonTool.MouseMove(sender, e);
                    break;
                case MainViewModel.Tools.Rectangle:
                    rectangleTool.MouseMove(sender, e);
                    break;
                case MainViewModel.Tools.TriangleShape:
                    triangleShape.MouseMove(sender, e);
                    break;
                case MainViewModel.Tools.RectangleShape:
                    rectangleShape.MouseMove(sender, e);
                    break;
                case MainViewModel.Tools.StarShape:
                    starShape.MouseMove(sender, e);
                    break;
                
            }

        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (selectedTool)
            {
                case MainViewModel.Tools.Brush:
                    break;
                case MainViewModel.Tools.TriangleShape:
                    triangleShape.MouseUp(sender, e);
                    break;
                case MainViewModel.Tools.RectangleShape:
                    rectangleShape.MouseUp(sender, e);
                    break;
                case MainViewModel.Tools.StarShape:
                    starShape.MouseUp(sender, e);
                    break;
            }

        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (selectedTool)
            {
                case MainViewModel.Tools.Brush:
                    break;
                case MainViewModel.Tools.Line:
                    lineTool.MouseDown(sender, e);
                    break;
                case MainViewModel.Tools.Ellipse:
                    ellipseTool.MouseDown(sender, e);
                    break;
                case MainViewModel.Tools.Path:
                    pathTool.MouseDown(sender, e);
                    break;
                case MainViewModel.Tools.Polygon:
                    polygonTool.MouseDown(sender, e);
                    break;
                case MainViewModel.Tools.Rectangle:
                    rectangleTool.MouseDown(sender, e);
                    break;
                case MainViewModel.Tools.TriangleShape: 
                    triangleShape.MouseDown(sender, e);
                    break;
                case MainViewModel.Tools.RectangleShape:
                    rectangleShape.MouseDown(sender, e);
                    break;
                case MainViewModel.Tools.StarShape:
                    starShape.MouseDown(sender, e);
                    break;
            }
        }   


        private void Brush_Btn_Clicked(object sender, RoutedEventArgs e)
        {
            mainViewModel.ChangeTool(MainViewModel.Tools.Brush);

        }

        private void Line_Btn_Clicked(object sender, RoutedEventArgs e)
        {
            mainViewModel.ChangeTool(MainViewModel.Tools.Line);

        }

        private void Clear_Btn_Clicked(object sender, RoutedEventArgs e)
        {
            paintSurface.Children.Clear();
        }


        private void Ellipse_Btn_Clicked(object sender, RoutedEventArgs e)
        {
            mainViewModel.ChangeTool(MainViewModel.Tools.Ellipse);
        }

        private void Path_Btn_Clicked(object sender, RoutedEventArgs e)
        {
            mainViewModel.ChangeTool(MainViewModel.Tools.Path);
        }

        private void Polygon_Btn_Clicked(object sender, RoutedEventArgs e)
        {
            mainViewModel.ChangeTool(MainViewModel.Tools.Polygon);
        }

        private void Rectangle_Btn_Clicked(object sender, RoutedEventArgs e)
        {
            mainViewModel.ChangeTool(MainViewModel.Tools.Rectangle);
        }

        private void Shape_Triangle_Btn_Clicked(object sender, RoutedEventArgs e)
        {
            mainViewModel.ChangeTool(MainViewModel.Tools.TriangleShape);
        }

        private void Shape_Rectangle_Btn_Clicked(object sender, RoutedEventArgs e)
        {
            mainViewModel.ChangeTool(MainViewModel.Tools.RectangleShape);
        }

        private void Shape_Star_Btn_Clicked(object sender, RoutedEventArgs e)
        {
            mainViewModel.ChangeTool(MainViewModel.Tools.StarShape);
        }


        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateKernelMatrix())
            {
                ApplyFilter();
            }
            else
            {
                MessageBox.Show("Invalid kernel matrix. Please enter valid numeric values.");
            }
        }

        private void ApplyFilter()
        {

            CanvasExtensions canvasExtensions = new CanvasExtensions(paintSurface);

            BitmapSource bmp = canvasExtensions.ToBitmapSource();

            WriteableBitmap canvasBitmap = new WriteableBitmap(bmp);

            CanvasExtensions.ApplyConvolutionFilter(canvasBitmap, kernelMatrix);

            FilterWindow filterWindow = new FilterWindow(canvasBitmap);
            filterWindow.Show();
        }

        private bool ValidateKernelMatrix()
        {
            bool isValid = true;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!double.TryParse(GetTextBoxValue($"tb{i}{j}"), out kernelMatrix[i, j]))
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            return isValid;
        }

        private string GetTextBoxValue(string textBoxName)
        {
            var textBox = (TextBox)FindName(textBoxName);
            return textBox.Text;
        }

        private void SetIdentityKernel()
        {
            kernelMatrix = new double[,] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };
            UpdateTextBoxValues();
        }

        private void UpdateTextBoxValues()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    SetTextBoxValue($"tb{i}{j}", kernelMatrix[i, j].ToString());
                }
            }
        }

        private void SetTextBoxValue(string textBoxName, string value)
        {
            var textBox = (TextBox)FindName(textBoxName);
            textBox.Text = value;
        }


        private void ToggleLayer(object sender, RoutedEventArgs e)
        {
            foreach (UIElement paintSurfaceChild in paintSurface.Children)
            {
                paintSurfaceChild.Visibility = paintSurfaceChild.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
                
            }
        }
    }
}
