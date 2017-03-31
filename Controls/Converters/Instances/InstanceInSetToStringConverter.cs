using Microsoft.VisualBasic;
using System;                  
using System.Globalization; 
using System.Windows.Controls;
using System.Windows.Data;

namespace Controls.Converters.Instances
{
  public sealed class InstanceInSetToStringConverter : Control, IValueConverter
  {
    public int FirstPosition { get; set; }
    public string OptionalHeader { get; set; }


    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!Information.IsNumeric(value) | FirstPosition <= 0)
        return string.Empty;
      var val = System.Convert.ToDouble(value);

      var final = (val == FirstPosition) ? "First" : ((int)(val - FirstPosition + 1)).DisplayNumberWithStringSuffix();

      return $"{final} {OptionalHeader}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
