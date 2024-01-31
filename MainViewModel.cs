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
    public class MainViewModel : INotifyPropertyChanged 
    {
        private SolidColorBrush _selectedColor;

        private Point startPoint;
        private Line currentLine;
        private LineTool lineTool;

        private int _brushSize;
        private int _colorR = 0;
        private int _colorG = 0;
        private int _colorB = 0;
        public enum Tools
        {
            Line,
            Circle,
            Rectangle,
            Brush
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

        public MainViewModel()
        {
            // Set default values
            SelectedColor = Brushes.Black;
            BrushSize = 5;

            // Initialize commands
            
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



    }


}
