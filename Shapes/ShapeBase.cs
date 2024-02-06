using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Grafika_lab_1_TK.Tools;

namespace Grafika_lab_1_TK.Shapes
{
    abstract class ShapeBase : ToolBase
    {
        protected readonly MainViewModel _viewModel;
        protected int _size;
        protected bool _isDrawing = false;
        protected Polygon? _previewShape = null;
        private ObservableCollection<Shape> _shapes;
        public ShapeBase(ObservableCollection<Shape> shapes, MainViewModel viewModel) : base(shapes, viewModel)
        {
            _shapes = shapes;
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

            _shapes.Add(polygon);
            _viewModel.SelectedLayer.AddElement(polygon);
            return polygon;
        }

        protected abstract PointCollection GetPoints(Point point);


        public override void MouseDown(object sender, MouseButtonEventArgs e, Point position)
        {
            DrawShape(position);
        }

        public override void MouseMove(object sender, MouseEventArgs e, Point position)
        {
            _shapes.Remove(_previewShape);

            _previewShape = DrawShape(position);
        }

        public override void MouseUp(object sender, MouseButtonEventArgs e, Point position)
        {
            
        }
    }
}
