using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Controls.Charting
{
  /// <summary>
  /// Interaction logic for TestUC.xaml
  /// </summary>
  public partial class LineChart : BaseChart
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

    #region "DataChangedAndTimingEvents"
    public override void OnTick(object o)
    {
      Timer.Stop();
      ResizeAndPlotPoints(o);
    }

    public override void Resized()
    {
      Timer.Stop();
      Timer.Start();
    }
    #endregion

    #region "ResivingAndPlotPoints"
    public override void ResizeAndPlotPoints(object o)
    {
      SetupInternalHeightAndWidths();
      SetupHeightAndWidthsOfObjects();
      CalculatePlotTrends();
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
      dynamic margin = 0.99;

      //True for Height having a larger aspect, False for Width having a larger aspect.  I am setting a floor for 25, anything smaller and it looks like crap.
      Func<int, bool, double> SetHeightOrWidth = (x, y) =>
      {
        dynamic val = y ? ((x / Ratio) * margin) : ((x * Ratio) * margin);
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

    public override void CalculatePlotTrends()
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

      if (ChartData.Count > 1)
      {
        //Uniformity check of X and Y types.  EG: You cannot have a DateTime and a Number for different X axis or Y axis sets.
      }

      //If ChartData.Count > 1 Then
      //  
      //  If ChartData.ToList().Select(Function(x) x.Points(0).X.GetType).Distinct.GroupBy(Function(x) x).Count > 1 Or ChartData.ToList().Select(Function(x) x.Points(0).Y.GetType).Distinct.GroupBy(Function(x) x).Count > 1 Then
      //    Me.PART_CanvasPoints.LayoutTransform = New ScaleTransform(1, 1)
      //    Me.PART_CanvasPoints.UpdateLayout()
      //    Dim fontFamily = If(Me.FontType IsNot Nothing, Me.FontType, New FontFamily("Segoe UI"))
      //    Dim stackPanel = New StackPanel
      //    stackPanel.Children.Add(New TextBlock With {.Text = "Type Mismatch cannot render!", .FontSize = 54, .FontFamily = fontFamily})
      //    stackPanel.Children.Add(New TextBlock With {.Text = "Either the X or Y plot points are of different types.", .FontSize = 32, .FontFamily = fontFamily})
      //    Me.PART_CanvasPoints.Children.Add(stackPanel)
      //    Return
      //  End If
      //End If

      //Me.PART_CanvasPoints.LayoutTransform = New ScaleTransform(1, -1)
      //Me.PART_CanvasPoints.UpdateLayout()

      //Me._xFloor = ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.XAsDouble).OrderBy(Function(x) x).FirstOrDefault()
      //Me._xCeiling = ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.XAsDouble).OrderByDescending(Function(x) x).FirstOrDefault()
      //Me._yFloor = ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.YAsDouble).OrderBy(Function(x) x).FirstOrDefault()
      //Me._yCeiling = ChartData.SelectMany(Function(x) x.Points).Select(Function(x) x.YAsDouble).OrderByDescending(Function(x) x).FirstOrDefault()

      //Me.PART_CanvasPoints.Children.RemoveRange(0, Me.PART_CanvasPoints.Children.Count)
      //Me.DrawTrends(PART_CanvasPoints, _viewWidth, _viewHeight, _xCeiling, _xFloor, _yCeiling, _yFloor)

      //If Me.PART_CanvasXAxisTicks IsNot Nothing And Me.PART_CanvasYAxisTicks IsNot Nothing Then
      //  If Me.XNumberOfTicks = 0 Then
      //    'I want at the very least to see a beginning and an end and redraw to show this.
      //    Me.XNumberOfTicks = 1
      //    Dim pt = ChartData(0).Points(0)
      //    ChartData(0).Points.Add(pt)
      //    Me.DrawTrends(PART_CanvasPoints, _viewWidth, _viewHeight, _xCeiling, _xFloor, _yCeiling, _yFloor)
      //  End If
      //  Me.DrawXAxis(PART_CanvasXAxisTicks, PART_CanvasXAxisLabels, _xCeiling, _xFloor, XNumberOfTicks, _viewWidth, _labelHeight)
      //  Me.DrawYAxis(PART_CanvasYAxisTicks, PART_CanvasYAxisLabels, _yCeiling, _yFloor, _viewHeight, _labelHeight)
      //End If
    }

    //public override void OnTick(object o)
    //{
    //  throw new NotImplementedException();
    //}

    //public override void ResizeAndPlotPoints(object o)
    //{
    //  throw new NotImplementedException();
    //}

    //public override void Resized()
    //{
    //  throw new NotImplementedException();
    //}
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
