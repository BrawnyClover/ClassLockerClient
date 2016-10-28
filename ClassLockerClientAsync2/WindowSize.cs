using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ClassLockerClientAsync2
{
    class WindowSize
    {
        MainWindow mainW;
        double orginalWidth, originalHeight;
        ScaleTransform scale = new ScaleTransform();

        public WindowSize(MainWindow mainW)
        {
            this.mainW = mainW;
            mainW.Loaded += new RoutedEventHandler(mainW_Loaded);
        }

        void Window1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ChangeSize(e.NewSize.Width, e.NewSize.Height);
        }

        void mainW_Loaded(object sender, RoutedEventArgs e)
        {
            orginalWidth = mainW.Width;
            originalHeight = mainW.Height;

            if (mainW.WindowState == WindowState.Maximized)
            {
                ChangeSize(mainW.ActualWidth, mainW.ActualHeight);
            }

            mainW.SizeChanged += new SizeChangedEventHandler(Window1_SizeChanged);
        }

        private void ChangeSize(double width, double height)
        {
            scale.ScaleX = width / orginalWidth;
            scale.ScaleY = height / originalHeight;

            FrameworkElement rootElement = mainW.Content as FrameworkElement;

            rootElement.LayoutTransform = scale;
        }
    }
}
