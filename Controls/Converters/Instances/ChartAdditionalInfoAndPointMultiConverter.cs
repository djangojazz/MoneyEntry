using System;                   
using System.Globalization;
using System.Linq;         
using System.Windows.Controls;
using System.Windows.Data;

namespace Controls.Converters.Instances
{
  public sealed class ChartAdditionalInfoAndPointMultiConverter : Control, IMultiValueConverter
  {

    public int DecimalPositions { get; set; }
    public bool IncludeComma { get; set; }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values.Count() != 2)
        return 0;

      return $"{values[0]} {values[1]}";
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

  }
}
