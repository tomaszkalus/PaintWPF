using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK
{
    interface IDrawingTool
    {
        void MouseDown();
        void MouseMove(MouseEventArgs e, Line previewLine);
        void MouseUp();
    }
}
