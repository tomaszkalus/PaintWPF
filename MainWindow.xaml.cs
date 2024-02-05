using System;
using System.ComponentModel;


using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Grafika_lab_1_TK.Shapes;
using Grafika_lab_1_TK.Tools;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace Grafika_lab_1_TK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private Canvas _paintSurface;

        private double[,] kernelMatrix = new double[3, 3];

        

        private readonly MainViewModel mainViewModel;
        
        public MainWindow()
        {


            mainViewModel = new MainViewModel();
            InitializeComponent();
            _paintSurface = null;
            this.DataContext = this.mainViewModel;


            SetIdentityKernel();

            this.Loaded += MainWindow_Loaded;
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _paintSurface = FindChild<Canvas>(this, "paintSurface");
        }

        public static T FindChild<T>(DependencyObject parent, string childName)
            where T : DependencyObject
        {
            // Direct match?
            if (parent is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                return (T)parent;

            // Search children
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                // Not the right type, ignore
                if (!(child is T))
                    continue;

                // Found?
                if (child is FrameworkElement frameworkElementChild && frameworkElementChild.Name == childName)
                    return (T)child;

                // Search within
                var result = FindChild<T>(child, childName);
                if (result != null)
                    return result;
            }

            return null;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point position = e.GetPosition(_paintSurface);

            mainViewModel.Canvas_MouseMove(sender, e, position);

        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Point position = e.GetPosition(_paintSurface);

            mainViewModel.Canvas_MouseDown(sender, e, position);

        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(_paintSurface);

            mainViewModel.Canvas_MouseUp(sender, e, position);

        }


        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateKernelMatrix())
            {
                ApplyFilter();
            }
            else
            {
                MessageBox.Show("Invalid kernel matrix. Please enter valid numeric values.");
            }
        }

        private void ApplyFilter()
        {

            CanvasExtensions canvasExtensions = new CanvasExtensions(_paintSurface);

            BitmapSource bmp = canvasExtensions.ToBitmapSource();

            WriteableBitmap canvasBitmap = new WriteableBitmap(bmp);

            CanvasExtensions.ApplyConvolutionFilter(canvasBitmap, kernelMatrix);

            FilterWindow filterWindow = new FilterWindow(canvasBitmap);
            filterWindow.Show();
        }

        private bool ValidateKernelMatrix()
        {
            bool isValid = true;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!double.TryParse(GetTextBoxValue($"tb{i}{j}"), out kernelMatrix[i, j]))
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            return isValid;
        }

        private string GetTextBoxValue(string textBoxName)
        {
            var textBox = (TextBox)FindName(textBoxName);
            return textBox.Text;
        }

        private void SetIdentityKernel()
        {
            kernelMatrix = new double[,] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, 0 } };
            UpdateTextBoxValues();
        }

        private void UpdateTextBoxValues()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    SetTextBoxValue($"tb{i}{j}", kernelMatrix[i, j].ToString());
                }
            }
        }

        private void SetTextBoxValue(string textBoxName, string value)
        {
            var textBox = (TextBox)FindName(textBoxName);
            textBox.Text = value;
        }



        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            _paintSurface = sender as Canvas;
        }
    }
}
