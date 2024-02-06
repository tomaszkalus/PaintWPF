using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK.Tools
{
    public class BrushTool : ToolBase
    {
        private PathFigure _pathFigure;
        private PathGeometry _pathGeometry;
        private Path _path;
        private ObservableCollection<Shape> _shapes;
        private MainViewModel _viewModel;
        private Ellipse _previewEllipse;

        public BrushTool(ObservableCollection<Shape> shapes, MainViewModel viewModel) : base(shapes, viewModel)
        {
            _shapes = shapes;
            _viewModel = viewModel;
            _previewEllipse = new Ellipse
            {
                Fill = Brushes.Gray,
                Width = _viewModel.BrushSize * 2,
                Height = _viewModel.BrushSize * 2,
                Visibility = Visibility.Collapsed
            };
            _shapes.Add(_previewEllipse);
        }

        public override void MouseMove(object sender, MouseEventArgs e, Point position)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (_pathFigure != null)
                {
                    _pathFigure.Segments.Add(new LineSegment(position, true));
                    _previewEllipse.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (_pathFigure == null)
                {
                    _previewEllipse.Margin = new Thickness(position.X - _viewModel.BrushSize,
                        position.Y - _viewModel.BrushSize, 0, 0);
                    _previewEllipse.Visibility = Visibility.Visible;
                }
            }
        }

        public override void MouseDown(object sender, MouseButtonEventArgs e, Point position)
        {
            _pathFigure = new PathFigure
            {
                StartPoint = position
            };

            _pathFigure.Segments.Add(new LineSegment(position, true));

            _pathGeometry = new PathGeometry();
            _pathGeometry.Figures.Add(_pathFigure);

            _path = new Path
            {
                Stroke = _viewModel.SelectedColor,
                StrokeThickness = _viewModel.BrushSize,
                Data = _pathGeometry
            };

            DrawShape(_path);

            _previewEllipse.Visibility = Visibility.Collapsed;
        }

        public override void MouseUp(object sender, MouseButtonEventArgs e, Point position)
        {
            _pathFigure = null;
            _pathGeometry = null;
            _path = null;
        }
    }

}


