using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK.Tools
{
    public class EllipseTool : ToolBase
    {
        private Ellipse? _previewEllipse = null;
        private readonly Canvas _paintSurface;
        private Point startPoint;
        private readonly MainViewModel _viewModel;

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePosition = e.GetPosition(_paintSurface);
            if (_previewEllipse != null)
            {
                double width = currentMousePosition.X - startPoint.X;
                double height = currentMousePosition.Y - startPoint.Y;

                _previewEllipse.Width = Math.Abs(width);
                _previewEllipse.Height = Math.Abs(height);

                _previewEllipse.SetValue(Canvas.LeftProperty, width < 0 ? currentMousePosition.X : startPoint.X);
                _previewEllipse.SetValue(Canvas.TopProperty, height < 0 ? currentMousePosition.Y : startPoint.Y);
            }
        }

        public EllipseTool(Canvas paintSurface, MainViewModel viewModel) : base(paintSurface, viewModel)
        {
            _paintSurface = paintSurface;
            _viewModel = viewModel;
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (_previewEllipse == null)
            {
                startPoint = e.GetPosition(_paintSurface);
                _previewEllipse = new Ellipse
                {
                    Stroke = Brushes.Gray,
                    StrokeThickness = 2,
                    Fill = Brushes.Transparent
                };

                // Set initial position
                _previewEllipse.SetValue(Canvas.LeftProperty, startPoint.X);
                _previewEllipse.SetValue(Canvas.TopProperty, startPoint.Y);

                _paintSurface.Children.Add(_previewEllipse);

                _paintSurface.MouseMove += MouseMove;
            }
            else
            {
                Point endPoint = e.GetPosition(_paintSurface);
                _paintSurface.Children.Remove(_previewEllipse);

                Ellipse finalEllipse = new Ellipse
                {
                    Stroke = _viewModel.SelectedColor,
                    StrokeThickness = _viewModel.BrushSize * 2,
                    Fill = Brushes.Transparent,
                    Width = Math.Abs(endPoint.X - startPoint.X),
                    Height = Math.Abs(endPoint.Y - startPoint.Y)
                };

                finalEllipse.SetValue(Canvas.LeftProperty, Math.Min(startPoint.X, endPoint.X));
                finalEllipse.SetValue(Canvas.TopProperty, Math.Min(startPoint.Y, endPoint.Y));

                _paintSurface.Children.Add(finalEllipse);

                _previewEllipse = null;
                _paintSurface.MouseMove -= MouseMove;
            }
        }
    }
}
