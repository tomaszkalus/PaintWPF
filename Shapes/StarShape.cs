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
    class StarShape : Shape
    {
        public StarShape(Canvas paintSurface, MainViewModel viewModel) : base(paintSurface, viewModel)
        {
        }

        protected override PointCollection GetPoints(Point point)
        {
            double angle = -Math.PI / 2; // Start from the top point
            double angleIncrement = 2 * Math.PI / 5; // Angle between each point

            PointCollection points = new PointCollection();

            for (int i = 0; i < 5; i++)
            {
                double outerX = point.X + _size * Math.Cos(angle);
                double outerY = point.Y + _size * Math.Sin(angle);

                points.Add(new Point(outerX, outerY));

                angle += angleIncrement;

                double innerX = point.X + _size * Math.Cos(angle);
                double innerY = point.Y + _size * Math.Sin(angle);

                points.Add(new Point(innerX, innerY));

                angle += angleIncrement;
            }
            return points;
        }
    }

}
