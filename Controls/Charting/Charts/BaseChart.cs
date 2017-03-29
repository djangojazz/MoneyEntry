using Controls.Types;
using System;      
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Controls.Charting
{
  public abstract class BaseChart : UserControl
  {
    //public delegate void CollectionChangedEventHandler(object sender, ObservableCollectionContentChangedArgs e);
    //public event CollectionChangedEventHandler OnCollectionChanged;

    //public delegate void LoadedEventHandler(object sender, EventArgs e);
    //public event LoadedEventHandler OnLoaded;

    private TimeSpan _defaultTimeSpan = new TimeSpan(1000);

    internal DispatcherTimer Timer = new DispatcherTimer();
    public BaseChart()
    {
      Timer.Interval = _defaultTimeSpan;
      FontType = FontType ?? new FontFamily("Segoe UI");
    }

    #region "Dependent Properties"

    #region "ChartTitle"

    public static readonly DependencyProperty ChartTitleProperty = DependencyProperty.Register(nameof(ChartTitle), typeof(string), typeof(BaseChart), new UIPropertyMetadata(string.Empty));
    public string ChartTitle
    {
      get { return Convert.ToString(GetValue(ChartTitleProperty)); }
      set { SetValue(ChartTitleProperty, value); }
    }
    #endregion

    #region "ChartForeground"

    public static readonly DependencyProperty ChartForegroundProperty = DependencyProperty.Register(nameof(ChartForeground), typeof(Brush), typeof(BaseChart), new UIPropertyMetadata(Brushes.Black));
    public Brush ChartForeground
    {
      get { return (Brush)GetValue(ChartForegroundProperty); }
      set { SetValue(ChartForegroundProperty, value); }
    }
    #endregion

    #region "ChartTitleVisibility"

    public static readonly DependencyProperty ChartTitleVisibilityProperty = DependencyProperty.Register(nameof(ChartTitleVisibility), typeof(Visibility), typeof(BaseChart), new PropertyMetadata(Visibility.Visible));
    public Visibility ChartTitleVisibility
    {
      get { return (Visibility)GetValue(ChartTitleVisibilityProperty); }

      set { SetValue(ChartTitleVisibilityProperty, value); }
    }
    #endregion

    #region "ChartTitleHidden"

    public static readonly DependencyProperty ChartTitleHiddenProperty = DependencyProperty.Register(nameof(ChartTitleHidden), typeof(bool), typeof(BaseChart), new UIPropertyMetadata(false, ChartTitleHiddenChanged));
    public bool ChartTitleHidden
    {
      get { return (bool)GetValue(ChartTitleHiddenProperty); }
      set { SetValue(ChartTitleHiddenProperty, value); }
    }

    public static void ChartTitleHiddenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      dynamic o = (BaseChart)d;

      if (Convert.ToBoolean(e.NewValue))
      {
        o.ChartTitleVisibility = Visibility.Collapsed;
      }
      else
      {
        o.ChartTitleVisibility = Visibility.Visible;
      }

    }
    #endregion

    #region "BackGroundColor"

    public static readonly DependencyProperty BackGroundColorProperty = DependencyProperty.Register(nameof(BackGroundColor), typeof(Brush), typeof(BaseChart), new UIPropertyMetadata(Brushes.Black));
    public Brush BackGroundColor
    {
      get { return (Brush)GetValue(BackGroundColorProperty); }
      set { SetValue(BackGroundColorProperty, value); }
    }
    #endregion

    #region "BackGroundColorCanvas"

    public static readonly DependencyProperty BackGroundColorCanvasProperty = DependencyProperty.Register(nameof(BackGroundColorCanvas), typeof(Brush), typeof(BaseChart), new UIPropertyMetadata(Brushes.Black));
    public Brush BackGroundColorCanvas
    {
      get { return (Brush)GetValue(BackGroundColorCanvasProperty); }
      set { SetValue(BackGroundColorCanvasProperty, value); }
    }
    #endregion

    #region "BackGroundColorLegend"

    public static readonly DependencyProperty BackGroundColorLegendProperty = DependencyProperty.Register(nameof(BackGroundColorLegend), typeof(Brush), typeof(BaseChart), new UIPropertyMetadata(Brushes.Black));
    public Brush BackGroundColorLegend
    {
      get { return (Brush)GetValue(BackGroundColorLegendProperty); }
      set { SetValue(BackGroundColorLegendProperty, value); }
    }
    #endregion

    #region "LegendVisibility"

    public static readonly DependencyProperty LegendVisibilityProperty = DependencyProperty.Register(nameof(LegendVisibility), typeof(Visibility), typeof(BaseChart), new PropertyMetadata(Visibility.Visible));
    public Visibility LegendVisibility
    {
      get { return (Visibility)GetValue(LegendVisibilityProperty); }

      set { SetValue(LegendVisibilityProperty, value); }
    }
    #endregion

    #region "LegendTextVisibility"

    public static readonly DependencyProperty LegendTextVisibilityProperty = DependencyProperty.Register(nameof(LegendTextVisibility), typeof(Visibility), typeof(BaseChart), new PropertyMetadata(Visibility.Visible));
    public Visibility LegendTextVisibility
    {
      get { return (Visibility)GetValue(LegendTextVisibilityProperty); }

      set { SetValue(LegendTextVisibilityProperty, value); }
    }
    #endregion

    #region "LegendForeground"

    public static readonly DependencyProperty LegendForegroundProperty = DependencyProperty.Register(nameof(LegendForeground), typeof(Brush), typeof(BaseChart), new UIPropertyMetadata(Brushes.Black));
    public Brush LegendForeground
    {
      get { return (Brush)GetValue(LegendForegroundProperty); }
      set { SetValue(LegendForegroundProperty, value); }
    }

    #endregion

    #region "LegendHidden"

    public static readonly DependencyProperty LegendHiddenProperty = DependencyProperty.Register(nameof(LegendHidden), typeof(bool), typeof(BaseChart), new UIPropertyMetadata(false, ChartLegendHiddenChanged));
    public bool LegendHidden
    {
      get { return (bool)GetValue(LegendHiddenProperty); }
      set { SetValue(LegendHiddenProperty, value); }
    }

    public static void ChartLegendHiddenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      dynamic o = (BaseChart)d;

      if (Convert.ToBoolean(e.NewValue))
      {
        o.LegendVisibility = Visibility.Collapsed;
        o.LegendTextVisibility = Visibility.Collapsed;
      }
      else
      {
        o.LegendVisibility = Visibility.Visible;
        o.LegendTextVisibility = Visibility.Visible;
      }
    }
    #endregion

    #region "YNumberOfTicks"

    public static readonly DependencyProperty YNumberOfTicksProperty = DependencyProperty.Register(nameof(YNumberOfTicks), typeof(int), typeof(BaseChart), new UIPropertyMetadata(0));
    public int YNumberOfTicks
    {
      get { return (int)GetValue(YNumberOfTicksProperty); }
      set { SetValue(YNumberOfTicksProperty, value); }
    }
    #endregion

    #region "XValueConverter"

    public static readonly DependencyProperty XValueConverterProperty = DependencyProperty.Register(nameof(XValueConverter), typeof(IValueConverter), typeof(BaseChart), null);
    public IValueConverter XValueConverter
    {
      get { return (IValueConverter)GetValue(XValueConverterProperty); }
      set { SetValue(XValueConverterProperty, value); }
    }
    #endregion

    //#Region "XValueMultiConverter"
    //  Public Shared ReadOnly XValueMultiConverterProperty As DependencyProperty = DependencyProperty.Register(nameof(XValueMultiConverter), GetType(IMultiValueConverter), GetType(BaseChart), Nothing)

    //  Public Property XValueMultiConverter As IMultiValueConverter
    //    Get
    //      Return CType(GetValue(XValueMultiConverterProperty), IMultiValueConverter)
    //    End Get
    //    Set
    //      SetValue(XValueMultiConverterProperty, Value)
    //    End Set
    //  End Property
    //#End Region

    #region "YValueConverter"

    public static readonly DependencyProperty YValueConverterProperty = DependencyProperty.Register(nameof(YValueConverter), typeof(IValueConverter), typeof(BaseChart), null);
    public IValueConverter YValueConverter
    {
      get { return (IValueConverter)GetValue(YValueConverterProperty); }
      set { SetValue(YValueConverterProperty, value); }
    }
    #endregion

    #region "FontType"

    public static readonly DependencyProperty FontTypeProperty = DependencyProperty.Register(nameof(FontType), typeof(FontFamily), typeof(BaseChart), null);
    public FontFamily FontType
    {
      get { return (FontFamily)GetValue(FontTypeProperty); }
      set { SetValue(FontTypeProperty, value); }
    }
    #endregion

    #region "ChartData"

    public static readonly DependencyProperty ChartDataProperty = DependencyProperty.Register("ChartData", typeof(ObservableCollectionContentNotifying<PlotTrend>), typeof(BaseChart), new UIPropertyMetadata(new ObservableCollectionContentNotifying<PlotTrend>(), ChartDataChanged));
    public ObservableCollectionContentNotifying<PlotTrend> ChartData
    {
      get { return (ObservableCollectionContentNotifying<PlotTrend>)GetValue(ChartDataProperty); }
      set { SetValue(ChartDataProperty, value); }
    }
    #endregion
    #endregion

    #region "Override Methods"


    //public abstract void NotifyCollectionChanged(object sender, ObservableCollectionContentChangedArgs e);

    public static void ChartDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      var o = (BaseChart)d;

      if ((e.OldValue != null))
      {
        var OldCollection = e.OldValue as ObservableCollectionContentNotifying<PlotTrend>;
        OldCollection.OnCollectionItemChanged -= o.CalculatePlotTrends;
        OldCollection.CollectionChanged -= o.CalculatePlotTrends;

      }

      if ((e.NewValue != null))
      {
        var NewCollection = e.NewValue as ObservableCollectionContentNotifying<PlotTrend>;
        NewCollection.OnCollectionItemChanged += o.CalculatePlotTrends;
        NewCollection.CollectionChanged += o.CalculatePlotTrends;
        o.Loaded += o.ResizeAndPlotPoints;
        o.SizeChanged += o.Resized;
        o.Timer.Tick += o.OnTick;
      }
    }

    public abstract void OnTick(object o, EventArgs e);

    public abstract void Resized(object o, EventArgs e);

    public abstract void ResizeAndPlotPoints(object o, EventArgs e);

    public abstract void CalculatePlotTrends(object sender, EventArgs e);


    #endregion

    #region "Protected Methods to be used by inheriting classes"
    protected string GetXSegmentText(string input)
    {
      return XValueConverter != null ? (string)XValueConverter.Convert(input, typeof(string), null, System.Globalization.CultureInfo.InvariantCulture) : input.ToString();
    }

    protected virtual void DrawYAxis(Canvas partCanvasYTicks, Canvas partCanvasYLabels, double yCeiling, double yFloor, double viewHeight, double labelHeight)
    {
      var segment = ((yCeiling - yFloor) / YNumberOfTicks);
      partCanvasYTicks.Children.RemoveRange(0, partCanvasYTicks.Children.Count);
      partCanvasYLabels.Children.RemoveRange(0, partCanvasYLabels.Children.Count);

      partCanvasYTicks.Children.Add(new Line
      {
        X1 = 0,
        X2 = 0,
        Y1 = 0,
        Y2 = viewHeight,
        StrokeThickness = 2,
        Stroke = Brushes.Black
      });

      //Sizing should be done from the ceiling
      var lastText = YValueConverter == null ? yCeiling.ToString() : (string)YValueConverter.Convert(yCeiling, typeof(string), null, System.Globalization.CultureInfo.InvariantCulture);
      var spacingForText = lastText.Length;
      var fontSize = 0.0;
      var finalSpacing = 0.0;
      var lastSpaceFactor = 0.0;

      if (spacingForText <= 7)
      {
        fontSize = 30;
        finalSpacing = spacingForText * 0.3;
        lastSpaceFactor = finalSpacing * 1.2;
      }
      else if (spacingForText <= 9)
      {
        fontSize = 24;
        finalSpacing = spacingForText * 0.5;
        lastSpaceFactor = finalSpacing * 1.5;
      }
      else if (spacingForText <= 11)
      {
        fontSize = 18;
        finalSpacing = spacingForText * 0.68;
        lastSpaceFactor = finalSpacing * 1.45;
      }
      else if (spacingForText <= 13)
      {
        fontSize = 16;
        finalSpacing = spacingForText * 0.7;
        lastSpaceFactor = finalSpacing * 1.44;
      }
      else
      {
        fontSize = 14;
        finalSpacing = spacingForText * 0.7;
        lastSpaceFactor = finalSpacing * 1.44;
      }

      for (int i = 0; i <= YNumberOfTicks; i++)
      {
        var ySegment = i == 0 ? 0 : i * (viewHeight / YNumberOfTicks);
        var ySegmentLabel = i == 0 ? yFloor : yFloor + (i * segment);
        var textForLabel = YValueConverter == null ? ySegmentLabel.ToString() : (string)YValueConverter.Convert(ySegmentLabel, typeof(string), null, System.Globalization.CultureInfo.InvariantCulture);

        var lineSegment = new Line
        {
          X1 = 0,
          X2 = labelHeight,
          Y1 = ySegment,
          Y2 = ySegment,
          StrokeThickness = 2,
          Stroke = Brushes.Black
        };
        partCanvasYTicks.Children.Add(lineSegment);

        var labelSegment = new TextBlock
        {
          Text = textForLabel,
          FontSize = fontSize,
          Margin = new Thickness(0, viewHeight - 20 - (ySegment - (i == 0 ? 0 : (i == YNumberOfTicks) ? lastSpaceFactor : finalSpacing)), 0, 0)
        };

        partCanvasYLabels.Children.Add(labelSegment);
      }
    }

    protected virtual void DrawXAxis(Canvas partCanvasXTicks, Canvas partCanvasXLabels, double xCeiling, double xFloor, int xTicks, double viewWidth, double labelHeight)
    {
      var segment = ((xCeiling - xFloor) / xTicks);
      partCanvasXTicks.Children.RemoveRange(0, partCanvasXTicks.Children.Count);
      partCanvasXLabels.Children.RemoveRange(0, partCanvasXLabels.Children.Count);

      partCanvasXTicks.Children.Add(new Line
      {
        X1 = 0,
        X2 = viewWidth,
        Y1 = 0,
        Y2 = 0,
        StrokeThickness = 2,
        Stroke = Brushes.Black
      });

      //Sizing should be done from the ceiling
      var lastText = GetXSegmentText(xCeiling.ToString());

      var spacingForText = lastText.Length * 6;
      var totalLength = spacingForText * xTicks;
      var fontSize = 0;
      var finalSpacing = 0.0;
      var lastSpaceFactor = 0.0;

      if (totalLength <= 200)
      {
        fontSize = 30;
        finalSpacing = spacingForText * 1.2;
        lastSpaceFactor = finalSpacing * 2;
      }
      else if (totalLength <= 250)
      {
        fontSize = 20;
        finalSpacing = spacingForText * 0.9;
        lastSpaceFactor = finalSpacing * 1.75;
      }
      else if (totalLength <= 500)
      {
        fontSize = 16;
        finalSpacing = spacingForText * 0.6;
        lastSpaceFactor = finalSpacing * 2;
      }
      else if (totalLength <= 750)
      {
        fontSize = 12;
        finalSpacing = spacingForText * 0.45;
        lastSpaceFactor = finalSpacing * 1.8;
      }
      else
      {
        fontSize = 8;
        finalSpacing = spacingForText * 0.3;
        lastSpaceFactor = finalSpacing * 2;
      }

      for (int i = 0; i <= xTicks; i++)
      {
        var xSegment = i == 0 ? 0 : i * (viewWidth / xTicks);
        var xSegmentLabel = i == 0 ? xFloor : xFloor + (i * segment);
        var textForLabel = GetXSegmentText(xSegmentLabel.ToString());

        var lineSegment = new Line
        {
          X1 = xSegment,
          X2 = xSegment,
          Y1 = 0,
          Y2 = labelHeight,
          StrokeThickness = 2,
          Stroke = Brushes.Black
        };
        partCanvasXTicks.Children.Add(lineSegment);

        var labelSegment = new TextBlock
        {
          Text = textForLabel,
          FontSize = fontSize,
          Margin = new Thickness(xSegment - (i == 0 ? 0 : (i == xTicks) ? lastSpaceFactor : finalSpacing), 0, 0, 0)
        };

        partCanvasXLabels.Children.Add(labelSegment);
      }
    }

    protected virtual void DrawTrends(Canvas partCanvas, double viewWidth, double viewHeight, double xCeiling, double xFloor, double yCeiling, double yFloor)
    {
      foreach (PlotTrend t in ChartData)
      {
        if (t.Points != null)
        {
          var xFactor = (viewWidth / (xCeiling - xFloor));
          var yFactor = (viewHeight / (yCeiling - yFloor));

          xFactor = double.IsNaN(xFactor) || double.IsInfinity(xFactor) ? 1 : xFactor;
          yFactor = double.IsNaN(yFactor) || double.IsInfinity(yFactor) ? 1 : yFactor;

          for (int i = 1; i <= t.Points.Count - 1; i++)
          {
            var toDraw = new Line
            {
              X1 = (t.Points[i - 1].XAsDouble - xFloor) * xFactor,
              Y1 = (t.Points[i - 1].YAsDouble - yFloor) * yFactor,
              X2 = (t.Points[i].XAsDouble - xFloor) * xFactor,
              Y2 = (t.Points[i].YAsDouble - yFloor) * yFactor,
              StrokeThickness = 2,
              Stroke = t.LineColor
            };
            partCanvas.Children.Add(toDraw);
          }
        }
      }
    }
    #endregion
  }

}
