using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Grafika_lab_1_TK
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        public FilterWindow(BitmapSource bitmap)
        {
            InitializeComponent();

            if (bitmap != null)
            {
                imageDisplay.Source = bitmap;
            }
            else
            {
                MessageBox.Show("Invalid bitmap source. The image cannot be displayed.");
                Close();
            }
        }
    }
}
