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
        // DependencyProperty för att möjliggöra fönsterstängning via DataContext
        public static readonly DependencyProperty EnableWindowClosingProperty =
            DependencyProperty.RegisterAttached(
                "EnableWindowClosing",
                typeof(bool),
                typeof(WindowCloser),
                new PropertyMetadata(false, OnEnableWindowClosingChanged)
            );

        public static bool GetEnableWindowClosing(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableWindowClosingProperty);
        }

        public static void SetEnableWindowClosing(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableWindowClosingProperty, value);
        }

        private static void OnEnableWindowClosingChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            if (d is Window window)
            {
                window.Loaded += (s, e) =>
                {
                    if (window.DataContext is ICloseWindows vm)
                    {
                        // Lyssnar på Close-händelsen för att stänga fönstret
                        vm.Close += () => window.Close();
                    }
                };
            }
        }
    }
}
