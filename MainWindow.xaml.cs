using System;
using System.ComponentModel;


using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace Grafika_lab_1_TK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point currentPoint = new Point();
        Point? previousPoint = null;

        //private Point startPoint;
        private Line previewLine;
        private LineTool lineTool;



        private MainViewModel mainViewModel;
        //private MainViewModel.Tools selectedTool;
        private MainViewModel.Tools selectedTool => mainViewModel.SelectedTool;
        
        public MainWindow()
        {

            mainViewModel = new MainViewModel();
            //selectedTool = mainViewModel.SelectedTool;

            


            InitializeComponent();
            this.DataContext = this.mainViewModel;

            lineTool = new LineTool(previewLine, paintSurface);
        }



        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            switch (selectedTool)
            {
                case MainViewModel.Tools.Brush:
                    if (selectedTool == MainViewModel.Tools.Brush)
                    {
                        if (e.LeftButton == MouseButtonState.Pressed)
                        {
                            Point currentPoint = e.GetPosition(paintSurface);

                            if (previousPoint != null)
                            {
                                Point startPoint = previousPoint.Value;
                                Point endPoint = currentPoint;

                                double distance = Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) + Math.Pow(endPoint.Y - startPoint.Y, 2));
                                int steps = (int)distance * 2;

                                for (int i = 0; i < steps; i++)
                                {
                                    double interpolation = (double)i / steps;
                                    double x = startPoint.X + (endPoint.X - startPoint.X) * interpolation;
                                    double y = startPoint.Y + (endPoint.Y - startPoint.Y) * interpolation;

                                    Ellipse ellipse = new Ellipse();
                                    ellipse.Fill = System.Windows.SystemColors.WindowFrameBrush;

                                    double diameter = this.mainViewModel.BrushSize * 2;
                                    ellipse.Fill = this.mainViewModel.SelectedColor;

                                    ellipse.Width = diameter;
                                    ellipse.Height = diameter;
                                    ellipse.Margin = new Thickness(x - this.mainViewModel.BrushSize, y - this.mainViewModel.BrushSize, 0, 0);
                                    paintSurface.Children.Add(ellipse);

                                }
                            }

                            previousPoint = currentPoint;
                        }

                    }

                    break;
                case MainViewModel.Tools.Line:
                    lineTool.MouseMove(sender, e);
                    break;
                case MainViewModel.Tools.Circle:
                    break;
                case MainViewModel.Tools.Rectangle:
                    break;
                default:
                    break;
            }

        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (selectedTool)
            {
                case MainViewModel.Tools.Brush:
                    previousPoint = null;
                    break;
                case MainViewModel.Tools.Line:
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

        private void Eraser_Btn_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void Clear_Btn_Clicked(object sender, RoutedEventArgs e)
        {
            paintSurface.Children.Clear();
        }


    }
}
