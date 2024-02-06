using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK.Tools
{
    public class PathTool : ToolBase
    {
        private Line? _previewLine = null;
        private Point? startPoint = null;

        public PathTool(ObservableCollection<Shape> shapes, MainViewModel viewModel) : base(shapes, viewModel)
        {
        }

        public override void MouseDown(object sender, MouseButtonEventArgs e, Point position)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (_previewLine != null && startPoint.HasValue)
                {
                    Line finalLine = new Line
                    {
                        Stroke = _viewModel.SelectedColor,
                        StrokeThickness = _viewModel.BrushSize * 2,
                        X1 = startPoint.Value.X,
                        Y1 = startPoint.Value.Y,
                        X2 = position.X,
                        Y2 = position.Y
                    };

                    DrawShape(finalLine);
                    _shapes.Remove(_previewLine);
                    _previewLine = null;
                }

                startPoint = position;

                _previewLine = new Line
                {
                    Stroke = Brushes.Gray,
                    StrokeThickness = _viewModel.BrushSize,
                    X1 = startPoint.Value.X,
                    Y1 = startPoint.Value.Y
                };

                _shapes.Add(_previewLine);
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                ResetTool();
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e, Point position)
        {
            if (startPoint.HasValue && _previewLine != null)
            {
                _previewLine.X2 = position.X;
                _previewLine.Y2 = position.Y;
            }
        }

        public override void MouseUp(object sender, MouseButtonEventArgs e, Point position)
        {
        }

        private void ResetTool()
        {
            _shapes.Remove(_previewLine);
            _previewLine = null;
            startPoint = null;
        }
    }
}
