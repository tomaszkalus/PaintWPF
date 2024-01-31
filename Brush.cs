using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Grafika_lab_1_TK
{
    class Brush
    {
        public Color Color { get; set; }
        public int Diameter { get; set; }

        public Brush(Color color, int diameter)
        {
            Color = color;
            Diameter = diameter;
        }
    }
}
