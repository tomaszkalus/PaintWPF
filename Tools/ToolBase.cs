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

        protected ToolBase(Canvas paintSurface, MainViewModel viewModel)
        {
            _paintSurface = paintSurface;
            _viewModel = viewModel;
        }

        public abstract void MouseMove(object sender, MouseEventArgs e);

        public abstract void MouseDown(object sender, MouseEventArgs e);
    }
}
