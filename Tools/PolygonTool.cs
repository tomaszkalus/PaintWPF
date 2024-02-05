using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK.Tools
{
    public class PolygonTool : ToolBase
    {
        private Line? _previewLine = null;
        private Point? firstPoint = null;
        private Point? startPoint = null;
        private const double Tolerance = 10.0;

        public PolygonTool(ObservableCollection<Shape> shapes, MainViewModel viewModel) : base(shapes, viewModel)
        {
        }

        private bool IsCloseToStartingPoint(Point startPoint, Point endPoint)
        {
            double distance = Math.Sqrt(Math.Pow(startPoint.X - endPoint.X, 2) + Math.Pow(startPoint.Y - endPoint.Y, 2));
            return distance <= Tolerance;
        }

        public override void MouseDown(object sender, MouseButtonEventArgs e, Point position)
        {
            
            if (e.ChangedButton == MouseButton.Left)
            {

                if (firstPoint == null)
                {
                    firstPoint = position;
                    startPoint = position;

                    _previewLine = new Line
                    {
                        Stroke = Brushes.Gray,
                        StrokeThickness = _viewModel.BrushSize,
                        X1 = startPoint.Value.X,
                        Y1 = startPoint.Value.Y
                    };

                    _shapes.Add(_previewLine);
                    return;

                }


                if (IsCloseToStartingPoint(firstPoint.Value, position))
                {
                    Line finalLine = new Line
                    {
                        Stroke = _viewModel.SelectedColor,
                        StrokeThickness = _viewModel.BrushSize * 2,
                        X1 = startPoint.Value.X,
                        Y1 = startPoint.Value.Y,
                        X2 = firstPoint.Value.X,
                        Y2 = firstPoint.Value.Y
                    };

                    _shapes.Add(finalLine);
                    ResetTool();
                    return;
                }

                Line nextLine = new Line
                {
                    Stroke = _viewModel.SelectedColor,
                    StrokeThickness = _viewModel.BrushSize * 2,
                    X1 = startPoint.Value.X,
                    Y1 = startPoint.Value.Y,
                    X2 = position.X,
                    Y2 = position.Y
                };
                _shapes.Add(nextLine);
                startPoint = position;

                _previewLine.X1 = startPoint.Value.X;
                _previewLine.Y1 = startPoint.Value.Y;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                ResetTool();
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e, Point position)
        {
            if (startPoint.HasValue && _previewLine != null)
            {
                _previewLine.X2 = position.X;
                _previewLine.Y2 = position.Y;
            }
        }

        public override void MouseUp(object sender, MouseButtonEventArgs e, Point position)
        {
        }

        private void ResetTool()
        {
            _shapes.Remove(_previewLine);
            _previewLine = null;
            startPoint = null;
            firstPoint = null;
        }
    }
}
