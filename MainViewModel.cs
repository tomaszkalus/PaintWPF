using Grafika_lab_1_TK.Shapes;
using Grafika_lab_1_TK.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using Shape = System.Windows.Shapes.Shape;

namespace Grafika_lab_1_TK
{
    public record HsvColor(double Hue, int Saturation, int Value) : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }


    public class MainViewModel : INotifyPropertyChanged
    {
        private SolidColorBrush _selectedColor;

        private int _brushSize;
        private int _colorR;
        private int _colorG;
        private int _colorB;

        private readonly ObservableCollection<Layer> _layers;

        public Layer[] Layers
        {
            get => _layers.ToArray();
        }

        private Layer _selectedLayer;
        public Layer SelectedLayer => _selectedLayer;
        private int _lastLayerId;

        public ObservableCollection<Shape> Shapes { get; private set; } = new();


        public enum Tools
        {
            Ellipse,
            Line,
            Path,
            Polygon,
            Rectangle,
            Brush,
            TriangleShape,
            RectangleShape,
            StarShape
        }

        private Dictionary<Tools, ToolBase> toolsDictionary;

        public Tools SelectedTool { get; set; } = Tools.Brush;
        private ToolBase _selectedTool;

        public SolidColorBrush SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    OnPropertyChanged();
                }
            }
        }

        public int BrushSize
        {
            get { return _brushSize; }
            set
            {
                if (_brushSize != value)
                {
                    _brushSize = value;
                    OnPropertyChanged();
                }
            }
        }

        public RelayCommand ChangeColorCommand { get; }
        public RelayCommand ChangeBrushSizeCommand { get; }
        public RelayCommand ToggleLayerCommand { get; }
        public RelayCommand SelectLayerCommand { get; }
        public RelayCommand ChangeToolCommand { get; }
        public RelayCommand AddLayerCommand { get; }
        public RelayCommand DeleteLayerCommand { get; }


        public int ColorR
        {
            get { return _colorR; }
            set
            {
                _colorR = value;
                SelectedColor = new SolidColorBrush(Color.FromRgb((byte)value, (byte)ColorG, (byte)ColorB));
                RefreshColor();
            }
        }

        public int ColorG
        {
            get { return _colorG; }
            set
            {
                _colorG = value;
                SelectedColor = new SolidColorBrush(Color.FromRgb((byte)ColorR, (byte)value, (byte)ColorB));
                RefreshColor();
            }
        }

        public int ColorB
        {
            get { return _colorB; }
            set
            {
                _colorB = value;
                SelectedColor = new SolidColorBrush(Color.FromRgb((byte)ColorR, (byte)ColorG, (byte)value));
                RefreshColor();
            }
        }

        public string ColorRgbLabel => $"RGB: ({ColorR.ToString()}, {ColorG.ToString()}, {ColorB.ToString()})";
        public string ColorHex => "#" + ColorR.ToString("X2") + ColorG.ToString("X2") + ColorB.ToString("X2");
        public string SelectedToolLabel => $"Selected tool: {SelectedTool.ToString()}";

        public string ColorHsvLabel =>
            $"HSV: ({Math.Round(HsvColorValue.Hue),2}, {HsvColorValue.Saturation}, {HsvColorValue.Value})";

        public HsvColor HsvColorValue => RgbToHsv(ColorR, ColorG, ColorB);

        public string SelectedLayerLabel => $"Selected layer: {_selectedLayer.Name}";
        

        public MainViewModel()
        {
            SelectedColor = Brushes.Black;
            BrushSize = 5;
            ChangeColorCommand = new RelayCommand(ChangeColor);
            ChangeBrushSizeCommand = new RelayCommand(ChangeBrushSize);
            ToggleLayerCommand = new RelayCommand(ToggleLayer);
            SelectLayerCommand = new RelayCommand(SelectLayer);
            ChangeToolCommand = new RelayCommand(ChangeTool);
            AddLayerCommand = new RelayCommand(AddLayer);
            DeleteLayerCommand = new RelayCommand(DeleteLayer);



            toolsDictionary = new Dictionary<Tools, ToolBase>()
            {
                { Tools.Ellipse, new EllipseTool(Shapes, this) },
                { Tools.Line, new LineTool(Shapes, this) },
                { Tools.Path, new PathTool(Shapes, this) },
                { Tools.Polygon, new PolygonTool(Shapes, this) },
                { Tools.Rectangle, new RectangleTool(Shapes, this) },
                { Tools.Brush, new BrushTool(Shapes, this) },
                { Tools.TriangleShape, new TriangleShape(Shapes, this) },
                { Tools.RectangleShape, new RectangleShape(Shapes, this) },
                { Tools.StarShape, new StarShape(Shapes, this) }
            };

            _selectedTool = toolsDictionary[Tools.Brush];

            _layers = new ObservableCollection<Layer>();
            _lastLayerId = 0;
            AddLayer(false);

            _selectedLayer = Layers[0];
        }


        private void DeleteLayer(object parameter)
        {
            if (parameter is int layerId)
            {
                var layerToRemove = _layers.FirstOrDefault(layer => layer.Id == layerId);

                if (layerToRemove != null)
                {
                    foreach (var element in layerToRemove.Elements)
                    {
                        Shapes.Remove(element);
                    }

                    _layers.Remove(layerToRemove);
                    OnPropertyChanged(nameof(Layers));
                }
            }
        }


        private void AddLayer(object obj)
        {
            int new_id = _lastLayerId;
            _lastLayerId++;

            _layers.Add(new Layer(new_id));
            OnPropertyChanged(nameof(Layers));
        }

        public void ToggleLayer(object parameter)
        {
            if (parameter is int layerId)
            {
                var layerToToggle = _layers.FirstOrDefault(layer => layer.Id == layerId);

                if (layerToToggle != null)
                {


                    layerToToggle.IsVisible = !layerToToggle.IsVisible;
                    OnPropertyChanged(nameof(Layers));
                }
            }

        }

        public void SelectLayer(object parameter)
        {

            if (parameter is int layerId)
            {
                var layerToSelect = _layers.FirstOrDefault(layer => layer.Id == layerId);

                if (layerToSelect != null)
                {
                    _selectedLayer = layerToSelect;
                    OnPropertyChanged(nameof(SelectedLayerLabel));
                }
            }
        }


        private void RefreshColor()
        {
            OnPropertyChanged(nameof(ColorHex));
            OnPropertyChanged(nameof(ColorRgbLabel));
            OnPropertyChanged(nameof(ColorR));
            OnPropertyChanged(nameof(ColorG));
            OnPropertyChanged(nameof(ColorB));
            OnPropertyChanged(nameof(HsvColorValue));
            OnPropertyChanged(nameof(ColorHsvLabel));
        }

        private void ChangeColor(object parameter)
        {
            var colorHex = parameter as string;
            Color color = (Color)ColorConverter.ConvertFromString(colorHex);

            _colorR = color.R;
            _colorG = color.G;
            _colorB = color.B;

            SelectedColor = new SolidColorBrush(color);

            RefreshColor();
        }

        private void ChangeBrushSize(object parameter)
        {
            if (parameter is int size)
            {
                BrushSize = size;
            }
        }

        

        public void ChangeTool(object parameter)
        {
            Tools tool;
            if (parameter is string toolName)
            {
                tool = (Tools)Enum.Parse(typeof(Tools), toolName);
                SelectedTool = tool;
                _selectedTool = toolsDictionary[tool];
                OnPropertyChanged(nameof(SelectedToolLabel));
            }
        }

        public void Canvas_MouseMove(object sender, MouseEventArgs e, Point position)
        {
            _selectedTool.MouseMove(sender, e, position);
        }

        public void Canvas_MouseDown(object sender, MouseButtonEventArgs e, Point position)
        {
            _selectedTool.MouseDown(sender, e, position);
        }

        public void Canvas_MouseUp(object sender, MouseButtonEventArgs e, Point position)
        {
            _selectedTool.MouseUp(sender, e, position);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }


        public static HsvColor RgbToHsv(int r, int g, int b)
        {
            double normR = r / 255.0;
            double normG = g / 255.0;
            double normB = b / 255.0;

            double min = Math.Min(Math.Min(normR, normG), normB);
            double max = Math.Max(Math.Max(normR, normG), normB);
            double delta = max - min;

            int value = (int)(max * 255);

            int saturation = (int)((max == 0) ? 0 : (delta / max) * 255);

            double hue = 0;

            if (delta != 0)
            {
                if (max == normR)
                {
                    hue = ((normG - normB) / delta) % 6;
                }
                else if (max == normG)
                {
                    hue = ((normB - normR) / delta) + 2;
                }
                else if (max == normB)
                {
                    hue = ((normR - normG) / delta) + 4;
                }
            }

            hue *= 60;
            if (hue < 0)
            {
                hue += 360;
            }

            return new HsvColor(hue, saturation, value);
        }
    }
}