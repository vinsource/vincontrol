using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace vincontrol.Backend.Helper
{
    public class BackgroundConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (int)value;
            return visibility <= 0 ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (SolidColorBrush) value;
            return color.Color == Colors.Transparent ? 0 : 1;
        }
    }
}
