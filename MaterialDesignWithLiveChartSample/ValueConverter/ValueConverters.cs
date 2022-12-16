using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MaterialDesignWithLiveChartSample.ValueConverter
{
    public class BoolToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = value is bool && (bool)value;
            bool collapsed = parameter as string == "collapsed";
            if (flag)
                return Visibility.Visible;
            return collapsed ? Visibility.Collapsed : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = (value is bool) && (bool)value;
            bool collapsed = (parameter as string) == "collapsed";
            if (flag)
                return Visibility.Visible;
            return collapsed ? Visibility.Collapsed : Visibility.Hidden;
        }
    }
}
