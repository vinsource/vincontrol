﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace vincontrol.WPF.Helpers.Convertors
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
          return visibility ? Visibility.Visible : Visibility.Collapsed;  
      }  
  
      public object ConvertBack(  
          object value,  
          Type targetType,  
          object parameter,  
          CultureInfo culture)  
      {  
          var visibility = (Visibility)value;  
          return (visibility == Visibility.Visible);  
      }  
  }
}