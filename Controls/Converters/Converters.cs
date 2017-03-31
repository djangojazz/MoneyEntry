using Controls.Converters.Instances;

namespace Controls.Converters
{
  public static class Converters
  {
    public static DateFormatConverter DateFormatConverter = new DateFormatConverter();
    public static DecimalConverter DecimalConverter = new DecimalConverter();
    public static ChartAdditionalInfoAndPointMultiConverter ChartMetaAndPointMultiConverter = new ChartAdditionalInfoAndPointMultiConverter();
    public static ChartTypeToVisibilityConverter ChartTypeToVisibilityConverter = new ChartTypeToVisibilityConverter();
  }
}