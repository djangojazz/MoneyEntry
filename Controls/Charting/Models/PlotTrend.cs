using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Controls.Charting
{
  public sealed class PlotTrend : INotifyPropertyChanged
  {

    private string _seriesName;
    public string SeriesName
    {
      get { return _seriesName; }
      set
      {
        _seriesName = value;
        OnPropertyChanged(nameof(SeriesName));
      }
    }

    private Brush _lineColor;
    public Brush LineColor
    {
      get { return _lineColor; }
      set
      {
        _lineColor = value;
        OnPropertyChanged(nameof(LineColor));
      }
    }

    private Thickness _pointThickness;

    public event PropertyChangedEventHandler PropertyChanged;

    public Thickness PointThickness
    {
      get { return _pointThickness; }
      set
      {
        _pointThickness = value;
        OnPropertyChanged(nameof(PointThickness));
      }
    }

    public ObservableCollection<PlotPoints> Points { get; }

    public PlotTrend(string seriesName, Brush lineColor, Thickness pointThickness, IEnumerable<PlotPoints> points)
    {
      Points = new ObservableCollection<PlotPoints>();

      this.SeriesName = seriesName;
      this.LineColor = lineColor;
      this.PointThickness = pointThickness;
      this.Points.ClearAndAddRange(points);

      this.Points.CollectionChanged += NotifyChangedCollection;
    }

    private void NotifyChangedCollection(object sender, EventArgs e)
    {
      this.OnPropertyChanged(nameof(Points));
    }
              
    public void OnPropertyChanged(string info)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
    }
  }
}
