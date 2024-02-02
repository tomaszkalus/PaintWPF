using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

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
        private int _colorR = 0;
        private int _colorG = 0;
        private int _colorB = 0;

        private int[] _layersOpacity;
        private int _selectedLayer;
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
        public Tools SelectedTool { get; set; } = Tools.Brush;

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
        public string ColorHsvLabel => $"HSV: ({Math.Round(HsvColorValue.Hue), 2}, {HsvColorValue.Saturation}, {HsvColorValue.Value})";
        public HsvColor HsvColorValue => RgbToHsv(ColorR, ColorG, ColorB);

        public MainViewModel()
        {
            SelectedColor = Brushes.Black;
            BrushSize = 5;
            ChangeColorCommand = new RelayCommand(ChangeColor);
            ChangeBrushSizeCommand = new RelayCommand(ChangeBrushSize);
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
            if (parameter is Tools tool)
            {
                SelectedTool = tool;
                OnPropertyChanged(nameof(SelectedToolLabel));
            }
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

        public void ResetTool()
        {
            //this.SelectedTool.Reset();
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
