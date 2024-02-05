using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK.Tools
{
    public class RectangleTool : ToolBase
    {
        private Rectangle? _previewRectangle = null;
        private readonly Canvas _paintSurface;
        private Point startPoint;
        private readonly MainViewModel _viewModel;

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePosition = e.GetPosition(_paintSurface);
            if (_previewRectangle != null)
            {
                double width = currentMousePosition.X - startPoint.X;
                double height = currentMousePosition.Y - startPoint.Y;

                // Update the size and position of the preview rectangle
                _previewRectangle.Width = Math.Abs(width);
                _previewRectangle.Height = Math.Abs(height);

                // Ensure the rectangle is positioned correctly
                _previewRectangle.SetValue(Canvas.LeftProperty, width < 0 ? currentMousePosition.X : startPoint.X);
                _previewRectangle.SetValue(Canvas.TopProperty, height < 0 ? currentMousePosition.Y : startPoint.Y);
            }
        }

        public RectangleTool(Canvas paintSurface, MainViewModel viewModel) : base(paintSurface, viewModel)
        {
            _paintSurface = paintSurface;
            _viewModel = viewModel;
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (_previewRectangle == null)
            {
                startPoint = e.GetPosition(_paintSurface);
                _previewRectangle = new Rectangle
                {
                    Stroke = Brushes.Gray,
                    StrokeThickness = 2,
                    Fill = Brushes.Transparent
                };

                // Set initial position
                _previewRectangle.SetValue(Canvas.LeftProperty, startPoint.X);
                _previewRectangle.SetValue(Canvas.TopProperty, startPoint.Y);

                _paintSurface.Children.Add(_previewRectangle);

                _paintSurface.MouseMove += MouseMove;
            }
            else
            {
                Point endPoint = e.GetPosition(_paintSurface);
                _paintSurface.Children.Remove(_previewRectangle);

                // Create the final rectangle
                Rectangle finalRectangle = new Rectangle
                {
                    Stroke = _viewModel.SelectedColor,
                    StrokeThickness = _viewModel.BrushSize * 2,
                    Fill = Brushes.Transparent,
                    Width = Math.Abs(endPoint.X - startPoint.X),
                    Height = Math.Abs(endPoint.Y - startPoint.Y)
                };

                // Ensure the rectangle is positioned correctly
                finalRectangle.SetValue(Canvas.LeftProperty, Math.Min(startPoint.X, endPoint.X));
                finalRectangle.SetValue(Canvas.TopProperty, Math.Min(startPoint.Y, endPoint.Y));

                _paintSurface.Children.Add(finalRectangle);

                _previewRectangle = null;
                _paintSurface.MouseMove -= MouseMove;
            }
        }
    }
}
