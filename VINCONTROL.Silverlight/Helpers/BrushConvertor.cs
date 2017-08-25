using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;


namespace VINCONTROL.Silverlight.Helpers
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
                    return new SolidColorBrush() { Color = ColorConverter.Convert("#AB1D1D") };
                case UploadStatus.Finish:
                    return new SolidColorBrush() { Color = ColorConverter.Convert("#FF4ED93F") };
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