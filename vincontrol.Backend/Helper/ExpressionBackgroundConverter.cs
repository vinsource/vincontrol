using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using vincontrol.DataFeed.Model;

namespace vincontrol.Backend.Helper
{
    class ExpressionBackgroundConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = (Expression)value;
            if (visibility == null) return new SolidColorBrush(Colors.Transparent);
            return string.IsNullOrEmpty(visibility.DBField1) && string.IsNullOrEmpty(visibility.DBField2) && string.IsNullOrEmpty(visibility.DBField3)&& String.IsNullOrEmpty(visibility.Operator1) && String.IsNullOrEmpty(visibility.Operator2) ? new SolidColorBrush(Colors.Transparent) : new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
