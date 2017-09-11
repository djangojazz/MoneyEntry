using System;

namespace Controls.Charting
{
  public class PlotPoint<T> : PlotPoint
  {
    public PlotPoint(T point)
    {
      var typ = typeof(T);

      if (!(typeof(DateTime) == typ || typeof(double) == typ || typeof(decimal) == typ || typeof(int) == typ))
        throw new ArgumentException("Cannot have a plot point that is not Decimal, Double, Integer, Date or DateTime");

      this.Point = point;
    }
    public T Point { get; set; }

    public override Type PointType
    {
      get { return typeof(T); }
    }
  }

  public abstract class PlotPoint
  {
    public abstract Type PointType { get; }
  }
}
