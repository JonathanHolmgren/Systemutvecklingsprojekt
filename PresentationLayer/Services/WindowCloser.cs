using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PresentationLayer.Services
{
   public class WindowCloser
    {


        public static bool GetEnableWindowClosing(DependencyObject obj)
        {
            return (bool)obj.GetValue(MyPropertyProperty);
        }

        public static void SetEnableWindowClosing(DependencyObject obj, bool value)
        {
            obj.SetValue(MyPropertyProperty, value);
        }
     
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.RegisterAttached("MyProperty", typeof(bool), typeof(WindowCloser), new PropertyMetadata(false, OnEnableWindowClosingChanged));

        private static void OnEnableWindowClosingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                window.Loaded += (s, e) =>
                {
                    if (window.DataContext is ICloseWindows vm)
                    {
                        vm.Close += () =>
                        {
                            window.Close();
                        };
                        
                    }
                };
            }
        }
    }
}
