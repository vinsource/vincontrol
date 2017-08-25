using System;
using System.Globalization;
using System.Windows.Data;

namespace Vincontrol.Vinsell.WPFLibrary.Converters
{
    public class FormatPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double content;
            if (value != null && double.TryParse(value.ToString(), out content))
            {
                return "$" + (content / 1000) + "K";
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
