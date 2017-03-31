using Controls.Charting;
using System;                  
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Controls.Converters.Instances
{
  public sealed class ChartTypeToVisibilityConverter : Control, IValueConverter
  {
    public Visibility VisibilityTrue { get; set; } = Visibility.Visible;
    public Visibility VisibilityFalse { get; set; } = Visibility.Collapsed;

    public ChartTypeToVisibilityConverter() {}
    public ChartTypeToVisibilityConverter(Visibility trueVisibility, Visibility falseVisibility)
    {
      VisibilityTrue = trueVisibility;
      VisibilityFalse = falseVisibility;
    }
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      try
      {
        var current = Enum.Parse(typeof(ChartType), value.ToString());
        var toMatch = Enum.Parse(typeof(ChartType), parameter.ToString());
        var type = (current.Equals(toMatch)) ? VisibilityTrue : VisibilityFalse;
        return type;
      }
      catch (Exception)
      {
        return VisibilityFalse;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
