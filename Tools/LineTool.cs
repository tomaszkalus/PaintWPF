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
        private readonly Canvas _paintSurface;
        private Point startPoint;
        private readonly MainViewModel _viewModel;


        public override void MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePosition = e.GetPosition(_paintSurface);
            if (_previewLine != null)
            {
                _previewLine.X2 = currentMousePosition.X;
                _previewLine.Y2 = currentMousePosition.Y;
            }
        }

        public LineTool(Canvas paintSurface, MainViewModel viewModel) : base(paintSurface, viewModel)
        {
            _paintSurface = paintSurface;
            _viewModel = viewModel;
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {

            if (_previewLine == null)
            {
                startPoint = e.GetPosition(_paintSurface);
                _previewLine = new Line
                {
                    Stroke = Brushes.Gray,
                    StrokeThickness = 2
                };

                _previewLine.X1 = startPoint.X;
                _previewLine.Y1 = startPoint.Y;
                _paintSurface.Children.Add(_previewLine);

                _paintSurface.MouseMove += MouseMove;
            }
            else
            {
                Point endPoint = e.GetPosition(_paintSurface);
                _paintSurface.Children.Remove(_previewLine);
                Line finalLine = new Line
                {
                    Stroke = _viewModel.SelectedColor,
                    StrokeThickness = _viewModel.BrushSize * 2,
                    X1 = startPoint.X,
                    Y1 = startPoint.Y,
                    X2 = endPoint.X,
                    Y2 = endPoint.Y
                };

                _paintSurface.Children.Add(finalLine);

                _previewLine = null;
                _paintSurface.MouseMove -= MouseMove;
            }
        }


    }
}
