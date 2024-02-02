using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK.Tools
{
    public class PolygonTool
    {
        private Line? _previewLine = null;
        private readonly Canvas _paintSurface;
        private Point currentLineStartPoint;
        private Point polygonStartPoint;
        private readonly MainViewModel _viewModel;
        private const double Tolerance = 10.0;

        private bool IsCloseToStartingPoint(Point startPoint, double x, double y)
        {
            double distance = Math.Sqrt(Math.Pow(startPoint.X - x, 2) + Math.Pow(startPoint.Y - y, 2));
            return distance <= Tolerance;
        }

        private void StopDrawing()
        {
            _paintSurface.Children.Remove(_previewLine);
            _previewLine = null;
            _paintSurface.MouseMove -= MouseMove;
            return;
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePosition = e.GetPosition(_paintSurface);
            if (_previewLine != null)
            {
                _previewLine.X2 = currentMousePosition.X;
                _previewLine.Y2 = currentMousePosition.Y;
            }
        }

        public PolygonTool(Canvas paintSurface, MainViewModel viewModel)
        {
            _paintSurface = paintSurface;
            _viewModel = viewModel;
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                _paintSurface.Children.Remove(_previewLine);
                _previewLine = null;
                _paintSurface.MouseMove -= MouseMove;
                return;
            }
            if (_previewLine == null)
            {
                currentLineStartPoint = e.GetPosition(_paintSurface);
                polygonStartPoint = currentLineStartPoint;
                _previewLine = new Line
                {
                    Stroke = Brushes.Gray,
                    StrokeThickness = 2
                };

                _previewLine.X1 = currentLineStartPoint.X;
                _previewLine.Y1 = currentLineStartPoint.Y;
                _paintSurface.Children.Add(_previewLine);

                _paintSurface.MouseMove += MouseMove;
            }
            else
            {
                Point endPoint = e.GetPosition(_paintSurface);
                _paintSurface.Children.Remove(_previewLine);

                Line finalLine;

                if (IsCloseToStartingPoint(polygonStartPoint, endPoint.X, endPoint.Y))
                {
                    finalLine = new Line
                    {
                        Stroke = _viewModel.SelectedColor,
                        StrokeThickness = _viewModel.BrushSize * 2,
                        X1 = currentLineStartPoint.X,
                        Y1 = currentLineStartPoint.Y,
                        X2 = polygonStartPoint.X,
                        Y2 = polygonStartPoint.Y
                    };

                    _paintSurface.Children.Add(finalLine);

                    StopDrawing();

                    return;
                }

                finalLine = new Line
                {
                    Stroke = _viewModel.SelectedColor,
                    StrokeThickness = _viewModel.BrushSize * 2,
                    X1 = currentLineStartPoint.X,
                    Y1 = currentLineStartPoint.Y,
                    X2 = endPoint.X,
                    Y2 = endPoint.Y
                };

                _paintSurface.Children.Add(finalLine);

                

                _previewLine = new Line
                {
                    Stroke = Brushes.Gray,
                    StrokeThickness = 2
                };

                currentLineStartPoint = e.GetPosition(_paintSurface);

                _previewLine.X1 = currentLineStartPoint.X;
                _previewLine.Y1 = currentLineStartPoint.Y;
                _paintSurface.Children.Add(_previewLine);

                




            }
        }
    }
}
