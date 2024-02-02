using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK.Shapes
{
    class RectangleShape : Shape
    {
        public RectangleShape(Canvas paintSurface, MainViewModel viewModel) : base(paintSurface, viewModel)
        {
        }

        protected override PointCollection GetPoints(Point point)
        {
            return new PointCollection
            {
                new(point.X + _size, point.Y + _size),
                new(point.X + _size, point.Y - _size),
                new(point.X - _size, point.Y - _size),
                new(point.X - _size, point.Y + _size)
            };
        }
    }
}
