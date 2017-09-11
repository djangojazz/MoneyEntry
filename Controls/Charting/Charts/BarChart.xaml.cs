using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Controls.Charting
{
  /// <summary>
  /// Interaction logic for BarChart.xaml
  /// </summary>
  public partial class BarChart
  {
    //VARIABLES
    private double _xFloor = 0;
    private double _xCeiling = 0;
    private double _yFloor = 0;
    private double _yCeiling = 0;
    private double _viewHeight = 0;
    private double _viewWidth = 0;
    private double _tickWidth = 0;
    private double _tickHeight = 0;
    private double _labelWidth = 0;
    private double _labelHeight = 0;
    private int _xNumberOfTicks = 0;
    private List<double> _explicitTicks;

    private double Ratio
    {
      get => PART_CanvasBorder.ActualHeight / PART_CanvasBorder.ActualWidth;
    }

    private double SegmentLength
    {
      get => _viewWidth / _xNumberOfTicks;
    }

    public BarChart()
    {
      InitializeComponent();
      Part_Layout.DataContext = this;
    }

    #region "DataChangedAndTimingEvents"
    public override void OnTick(object o, EventArgs e)
    {
      Timer.Stop();
      ResizeAndPlotPoints(o, null);
    }

    public override void Resized(object o, EventArgs e)
    {
      Timer.Stop();
      Timer.Start();
    }
    #endregion

    #region "ResivingAndPlotPoints"
    public override void ResizeAndPlotPoints(object o, EventArgs e)
    {
      SetupInternalHeightAndWidths();
      SetupHeightAndWidthsOfObjects();
      ResetTicksForSpecificDateRange();
      CalculatePlotTrends(this, null);
    }

    private void ResetTicksForSpecificDateRange()
    {
      _explicitTicks = ChartData.SelectMany(x => x.Points).Select(x => x.XAsDouble).Distinct().OrderBy(x => x).ToList();
      _xNumberOfTicks = _explicitTicks.Count();
    }

    private void SetupHeightAndWidthsOfObjects()
    {
      PART_CanvasYAxisLabels.Height = _viewHeight;
      PART_CanvasYAxisLabels.Width = _labelWidth;
      PART_CanvasYAxisTicks.Height = _viewHeight;
      PART_CanvasYAxisTicks.Width = _tickWidth;

      PART_CanvasXAxisLabels.Height = _labelHeight;
      PART_CanvasXAxisLabels.Width = _viewWidth;
      PART_CanvasXAxisTicks.Height = _tickHeight;
      PART_CanvasXAxisTicks.Width = _viewWidth;

      PART_CanvasPoints.Height = _viewHeight;
      PART_CanvasPoints.Width = _viewWidth;
    }

    private void SetupInternalHeightAndWidths()
    {
      var margin = 0.99;

      //True for Height having a larger aspect, False for Width having a larger aspect.  I am setting a floor for 25, anything smaller and it looks like crap.
      Func<int, bool, double> SetHeightOrWidth = (x, y) =>
      {
        var val = y ? ((x / Ratio) * margin) : ((x * Ratio) * margin);
        return val <= 25 ? 25 : val;
      };

      if (Ratio > 1)
      {
        _viewHeight = 1000;
        _viewWidth = SetHeightOrWidth(1000, true);
        _tickHeight = 50;
        _tickWidth = SetHeightOrWidth(50, true);
        _labelHeight = 30;
        _labelWidth = SetHeightOrWidth(75, true);
      }
      else
      {
        _viewHeight = SetHeightOrWidth(1000, false);
        _viewWidth = 1000;
        _tickHeight = SetHeightOrWidth(50, false);
        _tickWidth = 50;
        _labelHeight = SetHeightOrWidth(30, false);
        _labelWidth = 75;
      }
    }

    public override void CalculatePlotTrends(object sender, EventArgs e)
    {
      if (PART_CanvasPoints == null) return;

      if (!ChartData.Any())
      {
        ClearCanvasOfAllData();
        PART_CanvasPoints.LayoutTransform = new ScaleTransform(1, 1);
        PART_CanvasPoints.UpdateLayout();
        var fontFamily = FontType ?? new FontFamily("Segoe UI");
        var stackPanel = new StackPanel();
        stackPanel.Children.Add(new TextBlock { Text = "No data to display", FontSize = 54, FontFamily = fontFamily });
        PART_CanvasPoints.Children.Add(stackPanel);
        return;
      }
      else
      {
        PART_CanvasPoints.LayoutTransform = new ScaleTransform(1, -1);
        PART_CanvasPoints.UpdateLayout();
        ResetTicksForSpecificDateRange();

        //Uniformity check of X and Y types.  EG: You cannot have a DateTime and a Number for different X axis or Y axis sets.
        if (ChartData.ToList().Select(x => x.Points[0].X.GetType()).Distinct().GroupBy(x => x).Count() > 1 || ChartData.ToList().Select(x => x.Points[0].Y.GetType()).Distinct().GroupBy(x => x).Count() > 1)
        {
          PART_CanvasPoints.LayoutTransform = new ScaleTransform(1, 1);
          PART_CanvasPoints.UpdateLayout();
          var fontFamily = FontType ?? new FontFamily("Segoe UI");
          var stackPanel = new StackPanel();
          stackPanel.Children.Add(new TextBlock { Text = "Type Mismatch cannot render!", FontSize = 54, FontFamily = fontFamily });
          stackPanel.Children.Add(new TextBlock { Text = "Either the X or Y plot points are of different types.", FontSize = 32, FontFamily = fontFamily });
          PART_CanvasPoints.Children.Add(stackPanel);
          return;
        }

        PART_CanvasPoints.LayoutTransform = new ScaleTransform(1, -1);
        PART_CanvasPoints.UpdateLayout();

        _xFloor = ChartData.SelectMany(x => x.Points).Select(x => x.XAsDouble).OrderBy(x => x).FirstOrDefault();
        _xCeiling = ChartData.SelectMany(x => x.Points).Select(x => x.XAsDouble).OrderByDescending(x => x).FirstOrDefault();
        _yFloor = 0;
        _yCeiling = ChartData.SelectMany(x => x.Points).Select(x => x.YAsDouble).OrderByDescending(x => x).FirstOrDefault();

        PART_CanvasPoints.Children.RemoveRange(0, PART_CanvasPoints.Children.Count);
        DrawTrends(PART_CanvasPoints, _viewWidth, _viewHeight, _xCeiling, _xFloor, _yCeiling, _yFloor);

        if (PART_CanvasXAxisTicks != null && PART_CanvasYAxisTicks != null)
        {
          if (_xNumberOfTicks == 0)
          {
            //want at the very least to see a beginning and an end and redraw to show this.
            _xNumberOfTicks = 1;
          }
        }

        DrawXAxis(PART_CanvasXAxisTicks, PART_CanvasXAxisLabels, _xCeiling, 0, _xNumberOfTicks, _viewWidth, _labelHeight);
        DrawYAxis(PART_CanvasYAxisTicks, PART_CanvasYAxisLabels, _yCeiling, _yFloor, _viewHeight, _labelHeight);
      }   
    }
    #endregion

    #region "Drawing Methods"

    protected override void DrawXAxis(Canvas partCanvasXTicks, Canvas partCanvasXLabels, double xCeiling, double xFloor, int xTicks, double viewWidth, double labelHeight)
    {
      partCanvasXTicks.Children.RemoveRange(0, partCanvasXTicks.Children.Count);
      partCanvasXLabels.Children.RemoveRange(0, partCanvasXLabels.Children.Count);

      partCanvasXTicks.Children.Add(new Line
      {
        X1 = 0,
        X2 = _viewWidth,
        Y1 = 0,
        Y2 = 0,
        StrokeThickness = 2,
        Stroke = Brushes.Black
      });

      //Sizing should be done from the ceiling
      var lastText = XValueConverter == null ? xCeiling.ToString() : (string)XValueConverter.Convert(xCeiling, typeof(string), null, System.Globalization.CultureInfo.InvariantCulture);
      var spacingForText = lastText.Length * 6;
      var totalLength = spacingForText * xTicks;
      var fontSize = 0;
      var spacing = 0;

      if (totalLength <= 200)
      {
        fontSize = 30;
        spacing = (int)(spacingForText * 1.2);
      }
      else if (totalLength <= 250)
      {
        fontSize = 20;
        spacing = (int)(spacingForText * 0.9);
      }
      else if (totalLength <= 500)
      {
        fontSize = 16;
        spacing = (int)(spacingForText * 0.6);
      }
      else if (totalLength <= 750)
      {
        fontSize = 12;
        spacing = (int)(spacingForText * 0.45);
      }
      else
      {
        fontSize = 8;
        spacing = (int)(spacingForText * 0.3);
      }

      for (int i = 0; i <= xTicks - 1; i++)
      {
        var segment = GetSegment(i);

        var xSegmentLabel = _explicitTicks[i];
        var textForLabel = (XValueConverter == null) ? xSegmentLabel.ToString() : (string)XValueConverter.Convert(xSegmentLabel, typeof(string), null, System.Globalization.CultureInfo.InvariantCulture);

        var lineSegment = new Line
        {
          X1 = segment,
          X2 = segment,
          Y1 = 0,
          Y2 = labelHeight,
          StrokeThickness = 2,
          Stroke = Brushes.Black
        };
        partCanvasXTicks.Children.Add(lineSegment);

        dynamic labelSegment = new TextBlock
        {
          Text = textForLabel,
          FontSize = fontSize,
          Margin = new Thickness(segment - spacing, 0, 0, 0)
        };

        partCanvasXLabels.Children.Add(labelSegment);
      }
    }

    protected override void DrawTrends(Canvas partCanvas, double viewWidth, double viewHeight, double xCeiling, double xFloor, double yCeiling, double yFloor)
    {
      var widthOfBar = viewWidth / ((_xNumberOfTicks + 2) * ChartData.Count);
      var yFactor = (viewHeight / (yCeiling - yFloor));

      yFactor = (double.IsNaN(yFactor) || double.IsInfinity(yFactor)) ? 1 : yFactor;

      for (int i = 0; i <= _xNumberOfTicks - 1; i++)
      {
        var xValue = _explicitTicks[i];

        DrawTrendsFromDate(xValue, widthOfBar, yFactor, i);
      }
    }

    private void DrawTrendsFromDate(double xSegmentValue, double widthOfBar, double yFactor, int segmentIndex)
    {
      ChartData.SelectMany(x => x.Points).Where(x => x.XAsDouble == xSegmentValue).Select((pnt, ind) => new {
        YAsDouble = pnt.YAsDouble,
        XAsDouble = pnt.XAsDouble,
        Index = ind
      }).ToList().ForEach(t =>
      {
        var segment = GetSegment(segmentIndex);

        //If I have two sets or more on the same day I need to see that
        segment = segment + (t.Index > 0 ? (t.Index) * widthOfBar : 0);

        var matches = ChartData.Where((x, ind) => x.Points.ToList().Exists(y => y.XAsDouble == t.XAsDouble & y.YAsDouble == t.YAsDouble));
        var color = (matches.Count() > 1 ? matches.Skip(t.Index).Take(1).First().LineColor : matches.First().LineColor);

        var toDraw = new Line
        {
          X1 = segment,
          Y1 = 0,
          X2 = segment,
          Y2 = (t.YAsDouble - _yFloor) * yFactor,
          StrokeThickness = widthOfBar,
          Stroke = color
        };
        PART_CanvasPoints.Children.Add(toDraw);
      });
    }

    private double GetSegment(int index)
    {
      return (index * SegmentLength) + (SegmentLength / 2);
    }

    private void ClearCanvasOfAllData()
    {
      PART_CanvasPoints.Children.Clear();
      PART_CanvasXAxisTicks.Children.Clear();
      PART_CanvasXAxisLabels.Children.Clear();
      PART_CanvasYAxisLabels.Children.Clear();
      PART_CanvasYAxisTicks.Children.Clear();
    }
    #endregion
  }
}
