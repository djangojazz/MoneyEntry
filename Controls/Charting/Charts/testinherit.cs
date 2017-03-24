using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.Charting.Charts
{
  class testinherit     : BaseChart
  {
    public testinherit()
    {
      var thing = ChartData;
    }

    public override void CalculatePlotTrends()
    {
      throw new NotImplementedException();
    }

    public override void OnTick(object o)
    {
      throw new NotImplementedException();
    }

    public override void ResizeAndPlotPoints(object o)
    {
      throw new NotImplementedException();
    }

    public override void Resized()
    {
      throw new NotImplementedException();
    }
  }
}
