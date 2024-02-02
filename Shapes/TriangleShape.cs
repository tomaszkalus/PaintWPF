using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK.Shapes
{
    class TriangleShape : Shape
    {
        protected override PointCollection GetPoints(Point point)
        {
            return new PointCollection
            {
                new(point.X, point.Y - _size),
                new(point.X + _size, point.Y + _size),
                new(point.X - _size, point.Y + _size)
            };
        }

        public TriangleShape(Canvas paintSurface, MainViewModel viewModel) : base(paintSurface, viewModel)
        {
        }

    }
}
