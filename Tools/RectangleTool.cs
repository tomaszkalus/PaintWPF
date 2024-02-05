using System;
using System.Collections.ObjectModel;
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
        private Point? startPoint = null;

        public RectangleTool(ObservableCollection<Shape> shapes, MainViewModel viewModel) : base(shapes, viewModel)
        {
        }

        public override void MouseDown(object sender, MouseButtonEventArgs e, Point position)
        {
            if (!startPoint.HasValue)
            {
                startPoint = position;

                _previewRectangle = new Rectangle
                {
                    Stroke = Brushes.Gray,
                    StrokeThickness = _viewModel.BrushSize,
                    Fill = Brushes.Transparent
                };

                _shapes.Add(_previewRectangle);
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e, Point position)
        {
            if (e.LeftButton == MouseButtonState.Pressed && startPoint.HasValue && _previewRectangle != null)
            {
                double width = position.X - startPoint.Value.X;
                double height = position.Y - startPoint.Value.Y;

                _previewRectangle.Width = Math.Abs(width);
                _previewRectangle.Height = Math.Abs(height);

                _previewRectangle.SetValue(Canvas.LeftProperty, width < 0 ? position.X : startPoint.Value.X);
                _previewRectangle.SetValue(Canvas.TopProperty, height < 0 ? position.Y : startPoint.Value.Y);
            }
        }

        public override void MouseUp(object sender, MouseButtonEventArgs e, Point position)
        {
            if (startPoint.HasValue)
            {
                Rectangle finalRectangle = new Rectangle
                {
                    Stroke = _viewModel.SelectedColor,
                    StrokeThickness = _viewModel.BrushSize * 2,
                    Width = Math.Abs(position.X - startPoint.Value.X),
                    Height = Math.Abs(position.Y - startPoint.Value.Y)
                };

                finalRectangle.SetValue(Canvas.LeftProperty, Math.Min(startPoint.Value.X, position.X));
                finalRectangle.SetValue(Canvas.TopProperty, Math.Min(startPoint.Value.Y, position.Y));

                _shapes.Add(finalRectangle);

                _previewRectangle = null;
                startPoint = null;
            }
        }
    }

}
