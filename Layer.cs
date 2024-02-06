using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK
{
    public class Layer
    {

        private int _opacity;
        private List<Shape> _elements;
        private bool _isVisible;
        private readonly int _id;
        public string Name => "Layer " + (_id + 1);
        public int Id => _id;
        public List<Shape> Elements => _elements;

        public string ButtonContent => _isVisible ? "Hide" : "Show";

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

        public void AddElement(Shape element)
        {
            _elements.Add(element);
        }

        public Layer(int id)
        {
            _elements = new List<Shape>();
            _opacity = 100;
            _isVisible = true;
            _id = id;

        }
    }
}
