using MoneyEntry.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using Controls.Types;
using Controls;
using Controls.Charting;

namespace MoneyEntry.ViewModel
{
  public class ChartViewModel : WorkspaceViewModel
  {
    Person _person;
    RelayCommand _testCommand;
    private bool _loaded = false;
    private List<spTransactionSummationByDuration_Result> _data;

    public ChartViewModel(Person person)
    {
      //Setup
      _person = person;
      _testCommand = new RelayCommand(param => this.Test());
      _data = new List<spTransactionSummationByDuration_Result>();

      Categories = new ObservableCollectionContentNotifying<Category>();
      ChartData = new ObservableCollectionContentNotifying<PlotTrend>();
      Start = DateTime.Now.Date.AddMonths(-2);
      End = DateTime.Now.Date;
      Ceiling = 100;

      UpdateChartDataForPlotTrends();
      _loaded = true;
    }

    #region Properties
    public Frequency[] Array { get => Enum.GetValues(typeof(Frequency)).Cast<Frequency>().ToArray(); }

    public ObservableCollectionContentNotifying<Category> Categories { get; }

    #region OpenProperty
    private bool _open;

    public bool Open
    {
      get { return _open; }
      set
      {
        _open = value;
        if (_loaded  && !_open)
        {                            
          UpdateChartDataForPlotTrends();
        }
        OnPropertyChanged("Open");
        _loaded = true;
      }
    }
    #endregion

    #region CategoryHeader
    private string _categoryHeader;

    public string CategoryHeader
    {
      get { return _categoryHeader; }
      set
      {
        _categoryHeader = value;
        OnPropertyChanged("CategoryHeader");
      }
    }
    #endregion

    #region Start
    private DateTime _start;

    public DateTime Start
    {
      get { return _start; }
      set
      {
        _start = value;
        UpdateChartDataForPlotTrends();
        OnPropertyChanged("Start");
      }
    }
    #endregion

    #region End
    private DateTime _end;

    public DateTime End
    {
      get { return _end; }
      set
      {
        _end = value;
        UpdateChartDataForPlotTrends();
        OnPropertyChanged("End");
      }
    }
    #endregion

    #region Ceiling
    private int _ceiling;

    public int Ceiling
    {
      get { return _ceiling; }
      set
      {
        _ceiling = value;
        UpdateChartDataForPlotTrends();
        OnPropertyChanged("Ceiling");
      }
    }
    #endregion

    #region ChartData
    private ObservableCollectionContentNotifying<PlotTrend> _chartData;

    public ObservableCollectionContentNotifying<PlotTrend> ChartData
    {
      get { return _chartData; }
      set
      {
        _chartData = value;
        OnPropertyChanged("ChartData");
      }
    } 
    #endregion



    #endregion

    #region TestCommandAndMethod
    public ICommand TestCommand { get => _testCommand; }

    private void Test()
    {
      var s = String.Empty;
      Categories.Where(x => x.IsUsed == true).ToList().ForEach(x => s += x + Environment.NewLine);
      MessageBox.Show(s);
    }
    #endregion

    #region Methods
    private void UpdateChartDataForPlotTrends()
    {
      UpdateData();
      UpdatePlotTrendsFromData();
    }

    private void UpdateData()
    {
      if (!_loaded) return;

      using (var context = new ExpensesEntities())
      { 
        var results = context.spCategoryUseOverDuration(_start, _end, 2, _person.PersonId, _ceiling).ToList().Select(x => (int)x.CategoryID).ToArray();
        Categories.ClearAndAddRange(context.tdCategory.ToList().Select(x => new Category(x.CategoryID, x.Description, false)).ToList());
        Categories.Where(x => results.Contains(x.CategoryId))
                  .ToList()
                  .ForEach(x => x.IsUsed = true);

        var newInput = new TransactionSummationByDurationInput(_person.PersonId, _start, _end, Frequency.Month, false, results);
        var serialization = newInput.SerializeToXml();

        _data.ClearAndAddRange(context.spTransactionSummationByDuration(serialization).ToList());
      }
                     
      UpdateHeader();
    }

    private void UpdatePlotTrendsFromData()
    {
      if (!(_data.Any())) return;

      //Dim demands = Selects.GetDemandTrends(New DemandTrendInput(2278, New Date(2017, 2, 25), New Date(2017, 5, 1), SelectedItem.ToString, New List(Of Integer)({ 2, 25})))

      //Dim demand = demands.Select(Function(x) New PlotPoints(New PlotPoint(Of Double)(x.Grouping), New PlotPoint(Of Double)(x.DemandQty)))
      //Dim ad = demands.Select(Function(x) New PlotPoints(New PlotPoint(Of Double)(x.Grouping), New PlotPoint(Of Double)(x.DemandAdQty)))

      //  XTicks = If(demands.Count > 0, demands.Count - 1, 1)
      //  DecimalConverter.OptionalHeader = SelectedItem.ToString
      //  DecimalConverter.FirstPosition = demands.Select(Function(x) x.Grouping).First()

      //  ChartData.ClearAndAddRange({ New PlotTrend("Demand", Brushes.Blue, New Thickness(2), demand), New PlotTrend("Ad", Brushes.Red, New Thickness(2), ad)})

      var plotTrends = new List<PlotTrend>();

      _data.Select(cat => new { CategoryId = cat.CategoryId, CategoryName = cat.CategoryName })
        .Distinct()
        .ToList()
        .ForEach(cat =>
        {
          var itemsForCategory = _data.Where(x => x.CategoryId == cat.CategoryId);
          var points = itemsForCategory.Select(x => new PlotPoints(new PlotPoint<DateTime>(x.Grouping.Value), new PlotPoint<decimal>(x.Amount.Value)));

          plotTrends.Add(new PlotTrend(cat.CategoryName, ColorDictionary.Color[cat.CategoryId.Value], new Thickness(2), points));        
        });

      ChartData.ClearAndAddRange(plotTrends);

      var zzz = "hello";
    }

    private void UpdateHeader()
    {
      var itemsSelected = Categories.Where(x => x.IsUsed == true).Select(x => x.ToString());
      var headerUpdated = itemsSelected.Any() ? string.Join(", ", itemsSelected) : "No Items";
      CategoryHeader = headerUpdated;
    }

    public override string DisplayName
    {
      get { return "Charting (" + _person.FirstName + ")"; }
    }
    #endregion
  }
}
