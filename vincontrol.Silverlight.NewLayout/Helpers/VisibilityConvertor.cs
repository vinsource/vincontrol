﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace vincontrol.Silverlight.NewLayout.Helpers
{
  public class VisibilityConverter : IValueConverter  
  {  
      public object Convert(  
          object value,  
          Type targetType,  
          object parameter,  
          CultureInfo culture)  
      {  
          var visibility = (bool)value;  
          return visibility ? Visibility.Collapsed : Visibility.Visible;  
      }  
  
      public object ConvertBack(  
          object value,  
          Type targetType,  
          object parameter,  
          CultureInfo culture)  
      {  
          var visibility = (Visibility)value;  
          return (visibility == Visibility.Collapsed);  
      }  
  }
}