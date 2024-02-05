using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK.Tools
{
    public abstract class ToolBase
    {
        private Shape? _previewShape = null;
        protected readonly Canvas _paintSurface;
        protected readonly MainViewModel _viewModel;
        protected readonly ObservableCollection<Shape> _shapes;

        protected ToolBase(ObservableCollection<Shape> shapes, MainViewModel viewModel)
        {
            _viewModel = viewModel;
            _shapes = shapes;
        }

        public abstract void MouseMove(object sender, MouseEventArgs e, Point position);

        public abstract void MouseDown(object sender, MouseButtonEventArgs e, Point position);

        public abstract void MouseUp(object sender, MouseButtonEventArgs e, Point position);
    }
}
