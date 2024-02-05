using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK.Tools
{
    public class BrushTool : ToolBase
    {
        private Ellipse? _previewEllipse = null;
        private Point? _previousPoint = null;
        private readonly Canvas _paintSurface;
        private readonly MainViewModel _viewModel;
        private readonly Layer _layer;
        private readonly ObservableCollection<Shape> _shapes;

        public BrushTool(Canvas paintSurface, MainViewModel viewModel) : base(paintSurface, viewModel)
        {
            _paintSurface = paintSurface;
            _viewModel = viewModel;
            //_shapes = shapes;
        }

        public override void MouseMove(object sender, MouseEventArgs e)
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

                        double diameter = _viewModel.BrushSize * 2;
                        ellipse.Fill = _viewModel.SelectedColor;

                        ellipse.Width = diameter;
                        ellipse.Height = diameter;
                        ellipse.Margin = new Thickness(x - _viewModel.BrushSize, y - _viewModel.BrushSize, 0, 0);
                        //_layer.AddElement(ellipse);
                        _paintSurface.Children.Add(ellipse);

                    }
                }

                _previousPoint = currentPoint;
            }
            else
            {
                _previousPoint = null;
                if (_previewEllipse != null)
                {
                    _paintSurface.Children.Remove(_previewEllipse);
                    _previewEllipse = null;
                }

                _previewEllipse = new Ellipse()
                {
                    Fill = _viewModel.SelectedColor,
                    Width = _viewModel.BrushSize * 2,
                    Height = _viewModel.BrushSize * 2,
                    Margin = new Thickness(e.GetPosition(_paintSurface).X - _viewModel.BrushSize,
                        e.GetPosition(_paintSurface).Y - _viewModel.BrushSize, 0, 0)

                };
                _previewEllipse.Fill = _viewModel.SelectedColor;

                _paintSurface.Children.Add(_previewEllipse);
            }
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
