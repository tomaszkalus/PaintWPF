using System.Collections.ObjectModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK.Tools
{
    public class LineTool : ToolBase
    {
        private Line? _previewLine = null;
        private Point? startPoint = null;

        public LineTool(ObservableCollection<Shape> shapes, MainViewModel viewModel) : base(shapes, viewModel)
        {
        }

        public override void MouseDown(object sender, MouseButtonEventArgs e, Point position)
        {
            if (!startPoint.HasValue)
            {
                startPoint = position;

                _previewLine = new Line
                {
                    Stroke = Brushes.Gray,
                    StrokeThickness = _viewModel.BrushSize
                };

                _previewLine.X1 = position.X;
                _previewLine.Y1 = position.Y;

                _shapes.Add(_previewLine);
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e, Point position)
        {
            if (e.LeftButton == MouseButtonState.Pressed && startPoint.HasValue && _previewLine != null)
            {
                _previewLine.X2 = position.X;
                _previewLine.Y2 = position.Y;
            }
        }

        public override void MouseUp(object sender, MouseButtonEventArgs e, Point position)
        {
            _shapes.Remove(_previewLine);
            if (startPoint.HasValue)
            {
                Line finalLine = new Line
                {
                    Stroke = _viewModel.SelectedColor,
                    StrokeThickness = _viewModel.BrushSize * 2
                };

                finalLine.X1 = startPoint.Value.X;
                finalLine.Y1 = startPoint.Value.Y;
                finalLine.X2 = position.X;
                finalLine.Y2 = position.Y;
                
                _shapes.Add(finalLine);

                _previewLine = null;
                startPoint = null;
            }
        }
    }
}
