using System;  
using System.Globalization;
using System.Windows.Data;


namespace vincontrol.Backend.Helper
{
    public class RevertConverter : IValueConverter  
  {  
      public object Convert(  
          object value,  
          Type targetType,  
          object parameter,  
          CultureInfo culture)
      {
          return !(bool) value;
      }  
  
      public object ConvertBack(  
          object value,  
          Type targetType,  
          object parameter,  
          CultureInfo culture)  
      {
          return !(bool)value;
      }  
  }
}