using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK
{
    public class LineTool
    {
        private Line? _previewLine;
        private readonly Canvas _paintSurface;
        private Point startPoint;
        private SolidColorBrush _color;


        public void MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePosition = e.GetPosition(_paintSurface);
            if (_previewLine != null)
            {

                _previewLine.X2 = currentMousePosition.X;
                _previewLine.Y2 = currentMousePosition.Y;
            }
        }

        public LineTool(Line previewLine, Canvas paintSurface)
        {
            _previewLine = previewLine;
            _paintSurface = paintSurface;

        }



        public void MouseDown(object sender, MouseEventArgs e)
        {

            if (_previewLine == null)
            {
                startPoint = e.GetPosition(_paintSurface);
                _previewLine = new Line
                {
                    Stroke = _color,
                    StrokeThickness = 2
                };

                _previewLine.X1 = startPoint.X;
                _previewLine.Y1 = startPoint.Y;
                _paintSurface.Children.Add(_previewLine);

                _paintSurface.MouseMove += this.MouseMove;
            }
            else
            {
                Point endPoint = e.GetPosition(_paintSurface);
                _paintSurface.Children.Remove(_previewLine);
                Line finalLine = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    X1 = startPoint.X,
                    Y1 = startPoint.Y,
                    X2 = endPoint.X,
                    Y2 = endPoint.Y
                };

                _paintSurface.Children.Add(finalLine);

                _previewLine = null;
                _paintSurface.MouseMove -= this.MouseMove;
            }
        }


    }
}
