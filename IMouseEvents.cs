using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Grafika_lab_1_TK
{
    internal interface IMouseEvents
    {
        public void MouseDown(object sender, MouseButtonEventArgs e, Point position);
        public void MouseMove(object sender, MouseEventArgs e, Point position);
        public void MouseUp(object sender, MouseButtonEventArgs e, Point position);
    }
}
