using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace vincontrol.Backend.Helper
{
    class RoleToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public RoleToVisibilityConverter()
        {

        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var principal = value as User;
            var isValidUser = false;
            if (principal != null)
            {
                if (parameter.ToString().Split(';').Any(role => principal.Roles.Contains(role)))
                {
                    isValidUser = true;
                }
                return isValidUser ? Visibility.Visible : Visibility.Collapsed;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
