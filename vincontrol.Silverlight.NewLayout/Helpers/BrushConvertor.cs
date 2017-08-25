using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace vincontrol.Silverlight.NewLayout.Helpers
{
    public class BrushConvertor : IValueConverter
    {
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            var uploadStatus = (UploadStatus)value;
            switch (uploadStatus)
            {
                case UploadStatus.Existed:
                    return new SolidColorBrush() { Color = ColorConverter.Convert("#7f8388") };
                case UploadStatus.Finish:
                    return new SolidColorBrush() { Color = ColorConverter.Convert("#6770f0") };
                default:
                    return new SolidColorBrush() { Color = ColorConverter.Convert("#FFADABA8") };
            }
        }

        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return ((Visibility)value == Visibility.Visible);
        }
    }

}