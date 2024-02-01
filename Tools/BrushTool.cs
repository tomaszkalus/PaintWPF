using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK.Tools
{
    public class BrushTool
    {
        private Point? _previousPoint = null;
        private readonly Canvas _paintSurface;
        private readonly MainViewModel _viewModel;

        public BrushTool(Canvas paintSurface, MainViewModel viewModel)
        {
            _paintSurface = paintSurface;
            _viewModel = viewModel;
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            _previousPoint = null;
        }
        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPoint = e.GetPosition(_paintSurface);

                if (_previousPoint != null)
                {
                    Point startPoint = _previousPoint.Value;
                    Point endPoint = currentPoint;

                    double distance = Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) + Math.Pow(endPoint.Y - startPoint.Y, 2));
                    int steps = (int)distance * 2;

                    for (int i = 0; i < steps; i++)
                    {
                        double interpolation = (double)i / steps;
                        double x = startPoint.X + (endPoint.X - startPoint.X) * interpolation;
                        double y = startPoint.Y + (endPoint.Y - startPoint.Y) * interpolation;

                        Ellipse ellipse = new Ellipse();
                        ellipse.Fill = SystemColors.WindowFrameBrush;

                        double diameter = _viewModel.BrushSize * 2;
                        ellipse.Fill = _viewModel.SelectedColor;

                        ellipse.Width = diameter;
                        ellipse.Height = diameter;
                        ellipse.Margin = new Thickness(x - _viewModel.BrushSize, y - _viewModel.BrushSize, 0, 0);
                        _paintSurface.Children.Add(ellipse);

                    }
                }

                _previousPoint = currentPoint;
            }
        }

    }
}
