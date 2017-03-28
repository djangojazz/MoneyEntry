using System;                  
using System.Globalization; 
using System.Windows.Controls;
using System.Windows.Data;

namespace Controls.Converters.Instances
{
  public sealed class DateFormatConverter : Control, IValueConverter
  {

    public string DateFormat { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var dt = (DateTime)value;

      return dt.ToString(DateFormat);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
