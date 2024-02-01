using System;
using System.ComponentModel;


using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
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
            }

        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (selectedTool)
            {
                case MainViewModel.Tools.Brush:
                    brushTool.MouseUp(sender, e);
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

    }
}
