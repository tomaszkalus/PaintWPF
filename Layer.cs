using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Grafika_lab_1_TK
{
    public class Layer
    {
        private int _opacity;
        private List<UIElement> _elements;
        private bool _isVisible;
        private readonly int _number;

        public int Opacity
        {
            get => _opacity;
            set => _opacity = value;
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (value)
                {
                    ShowLayer();
                }
                else
                {
                    HideLayer();
                }
                _isVisible = value;
            }
        }

        public void ShowLayer()
        {
            if (_isVisible)
            {
                return;
            }
            foreach (var uiElement in _elements)
            {
                uiElement.Visibility = Visibility.Visible;
            }
            _isVisible = true;
        }

        public void HideLayer()
        {
            if (!_isVisible)
            {
                return;
            }
            foreach (var uiElement in _elements)
            {
                uiElement.Visibility = Visibility.Hidden;
                
            }
            _isVisible = false;
        }

        public void AddElement(UIElement element)
        {
            _elements.Add(element);
        }

        public Layer(int number)
        {
            _elements = new List<UIElement>();
            _opacity = 100;
            _isVisible = true;
            _number = number;
            
        }

        public override string ToString()
        {
            return _number.ToString();
        }
    }
}
