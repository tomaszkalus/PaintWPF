using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK.Shapes
{
    abstract class Shape
    {
        protected readonly Canvas _paintSurface;
        protected readonly MainViewModel _viewModel;
        protected int _size;
        protected bool _isDrawing = false;
        protected Polygon? _previewShape = null;
        public Shape(Canvas paintSurface, MainViewModel viewModel)
        {
            _paintSurface = paintSurface;
            _viewModel = viewModel;
            _size = 50;
        }

        protected Polygon DrawShape(Point point)
        {
            Polygon polygon = new Polygon
            {
                Stroke = _viewModel.SelectedColor,
                StrokeThickness = 8,
                Points = GetPoints(point),
            };

            _paintSurface.Children.Add(polygon);
            return polygon;
        }

        protected abstract PointCollection GetPoints(Point point);

        public void MouseMove(object sender, MouseEventArgs e)
        {
            _paintSurface.Children.Remove(_previewShape);
            Point currentMousePosition = e.GetPosition(_paintSurface);

            _previewShape = DrawShape(currentMousePosition);
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            Point currentMousePosition = e.GetPosition(_paintSurface);
            DrawShape(currentMousePosition);
        }
    }
}
