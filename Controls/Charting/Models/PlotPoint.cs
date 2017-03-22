using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.Charting
{
  public class PlotPoint<T> : PlotPoint
  {
    public PlotPoint(T point)
    {
      if ((typeof(T) != typeof(decimal)) || (typeof(T) != typeof(double)) || (typeof(T) != typeof(int)) || (typeof(T) != typeof(DateTime)) || (typeof(T) != typeof(DateTime)))
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
