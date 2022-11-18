using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace MaterialDesignWithLiveChartSample
{
    public static class MaterialDesignWindowCustom
    {
        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.RegisterAttached(
           "HeaderContent", typeof(object), typeof(MaterialDesignWindowCustom), new PropertyMetadata(default(object)));
        public static void SetHeaderContent(DependencyObject element, object value)
        {
            element.SetValue(HeaderContentProperty, value);
        }

        public static object GetHeaderContent(DependencyObject element)
        {
            return element.GetValue(HeaderContentProperty);
        }

        public static RoutedUICommand Close = new RoutedUICommand();
        public static RoutedUICommand ToggleMaximize = new RoutedUICommand();
        public static RoutedUICommand Minimize = new RoutedUICommand();

        public static void RegisterCommands(Window window)
        {
            window.CommandBindings.Add(new CommandBinding(Close, OnClose));
            window.CommandBindings.Add(new CommandBinding(ToggleMaximize, OnToggleMaximize));
            window.CommandBindings.Add(new CommandBinding(Minimize, OnMinimize));
        }
        public static void OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Window window) window.Close();
        }

        public static void OnToggleMaximize(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Window window)
            {
                window.WindowState = window.WindowState == WindowState.Normal
                    ? WindowState.Maximized
                    : WindowState.Normal;
            }
        }

        private static void OnMinimize(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Window window)
            {
                window.WindowState = WindowState.Minimized;
            }
        }
    }

    public static class WindowEX
    {
        public static readonly DependencyProperty ShowMinimizeButtonProperty = DependencyProperty.RegisterAttached(
            "ShowMinimizeButton", typeof(bool), typeof(WindowEX), new PropertyMetadata(true));

        public static void SetShowMinimizeButton(DependencyObject element, bool value)
        {
            element.SetValue(ShowMinimizeButtonProperty, value);
        }

        public static bool GetShowMinimizeButton(DependencyObject element)
        {
            return (bool)element.GetValue(ShowMinimizeButtonProperty);
        }

        public static readonly DependencyProperty ShowMaximizeButtonProperty = DependencyProperty.RegisterAttached(
            "ShowMaximizeButton", typeof(bool), typeof(WindowEX), new PropertyMetadata(true));

        public static void SetShowMaximizeButton(DependencyObject element, bool value)
        {
            element.SetValue(ShowMaximizeButtonProperty, value);
        }

        public static bool GetShowMaximizeButton(DependencyObject element)
        {
            return (bool)element.GetValue(ShowMaximizeButtonProperty);
        }
    }

}
