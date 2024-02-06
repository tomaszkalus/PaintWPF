using System;
using System.Collections.ObjectModel;
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
        private Point? startPoint = null;

        public EllipseTool(ObservableCollection<Shape> shapes, MainViewModel viewModel) : base(shapes, viewModel)
        {
        }

        public override void MouseDown(object sender, MouseButtonEventArgs e, Point position)
        {
            if (!startPoint.HasValue)
            {
                startPoint = position;

                _previewEllipse = new Ellipse
                {
                    Stroke = Brushes.Gray,
                    StrokeThickness = _viewModel.BrushSize,
                    Fill = Brushes.Transparent
                };

                _shapes.Add(_previewEllipse);
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e, Point position)
        {
            if (e.LeftButton == MouseButtonState.Pressed && startPoint.HasValue && _previewEllipse != null)
            {
                double width = position.X - startPoint.Value.X;
                double height = position.Y - startPoint.Value.Y;

                _previewEllipse.Width = Math.Abs(width);
                _previewEllipse.Height = Math.Abs(height);

                _previewEllipse.SetValue(Canvas.LeftProperty, width < 0 ? position.X : startPoint.Value.X);
                _previewEllipse.SetValue(Canvas.TopProperty, height < 0 ? position.Y : startPoint.Value.Y);
            }
        }

        public override void MouseUp(object sender, MouseButtonEventArgs e, Point position)
        {
            if (startPoint.HasValue)
            {
                Ellipse finalEllipse = new Ellipse
                {
                    Stroke = _viewModel.SelectedColor,
                    StrokeThickness = _viewModel.BrushSize * 2,
                    Width = Math.Abs(position.X - startPoint.Value.X),
                    Height = Math.Abs(position.Y - startPoint.Value.Y)
                };

                finalEllipse.SetValue(Canvas.LeftProperty, Math.Min(startPoint.Value.X, position.X));
                finalEllipse.SetValue(Canvas.TopProperty, Math.Min(startPoint.Value.Y, position.Y));

                DrawShape(finalEllipse);

                _previewEllipse = null;
                startPoint = null;
            }
        }
    }
}
