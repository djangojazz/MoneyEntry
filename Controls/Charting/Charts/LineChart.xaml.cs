using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Controls.Charting
{
  /// <summary>
  /// Interaction logic for TestUC.xaml
  /// </summary>
  public partial class LineChart
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

    private double Ratio
    {
      get { return PART_CanvasBorder.ActualHeight / PART_CanvasBorder.ActualWidth; }
    }

    public LineChart()
    {
      InitializeComponent();
      Part_Layout.DataContext = this;
    }

    #region "XNumberOfTicks"

    public static readonly DependencyProperty XNumberOfTicksProperty = DependencyProperty.Register(nameof(XNumberOfTicks), typeof(int), typeof(LineChart), new UIPropertyMetadata(0));
    public int XNumberOfTicks
    {
      get { return (int)GetValue(XNumberOfTicksProperty); }
      set { SetValue(XNumberOfTicksProperty, value); }
    }
    #endregion

    #region "X"

    public static readonly DependencyProperty XTicksDynamicProperty = DependencyProperty.Register(nameof(XTicksDynamic), typeof(bool), typeof(LineChart), new UIPropertyMetadata(false));
    public bool XTicksDynamic
    {
      get { return (bool)GetValue(XTicksDynamicProperty); }
      set { SetValue(XTicksDynamicProperty, value); }
    }
    #endregion

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
      CalculatePlotTrends(o, null);
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

        var xTicks = XTicksDynamic ? ChartData.SelectMany(x => x.Points).Select(x => x.XAsDouble).Distinct().Count() - 1 : XNumberOfTicks;

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
          if (XNumberOfTicks == 0)
          {
            //want at the very least to see a beginning and an end and redraw to show this.
            XNumberOfTicks = 1;
            var pt = ChartData[0].Points[0];
            ChartData[0].Points.Add(pt);
            DrawTrends(PART_CanvasPoints, _viewWidth, _viewHeight, _xCeiling, _xFloor, _yCeiling, _yFloor);
          }
        }

        DrawXAxis(PART_CanvasXAxisTicks, PART_CanvasXAxisLabels, _xCeiling, _xFloor, xTicks, _viewWidth, _labelHeight);
        DrawYAxis(PART_CanvasYAxisTicks, PART_CanvasYAxisLabels, _yCeiling, _yFloor, _viewHeight, _labelHeight);
      }
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
