﻿using MoneyEntry.Model;
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
using Controls.Converters.Instances;

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
      InstanceConverter = new InstanceInSetToStringConverter();
      _person = person;                                           
      _data = new List<spTransactionSummationByDuration_Result>();
      Start = DateTime.Now.Date.AddMonths(-2);
      End = DateTime.Now.Date;
      Ceiling = 100;
      SelectedFrequency = GroupingFrequency.Month;

      Categories = new ObservableCollectionContentNotifying<Category>();
      InitialCategorySet();
                         
      ChartData = new ObservableCollectionContentNotifying<PlotTrend>();
      UpdateChartDataForPlotTrends();
      _loaded = true;
    }

    #region Properties
    public GroupingFrequency[] Frequencies { get => Enum.GetValues(typeof(GroupingFrequency)).Cast<GroupingFrequency>().ToArray(); }

    #region SelectedItem
    private GroupingFrequency _selectedFrequency;
    public GroupingFrequency SelectedFrequency
    {
      get { return _selectedFrequency; }
      set
      {
        _selectedFrequency = value;
        UpdateChartDataForPlotTrends();                 
        OnPropertyChanged(nameof(SelectedFrequency));
      }
    }
    #endregion              
    public InstanceInSetToStringConverter InstanceConverter { get; }
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
          UpdateHeader();
          UpdateChartDataForPlotTrends();
        }
        OnPropertyChanged(nameof(Open));
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
        OnPropertyChanged(nameof(CategoryHeader));
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
        OnPropertyChanged(nameof(Start));
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
        OnPropertyChanged(nameof(End));
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
        OnPropertyChanged(nameof(Ceiling));
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
        OnPropertyChanged(nameof(ChartData));
      }
    } 
    #endregion
              
    #endregion
                       
    #region Methods
    private void UpdateChartDataForPlotTrends()
    {
      UpdateData();
      UpdatePlotTrendsFromData();
    }

    private void InitialCategorySet()
    {
      using (var context = new ExpensesEntities())
      {
        var results = context.spCategoryUseOverDuration(_start, _end, 2, _person.PersonId, _ceiling).ToList().Select(x => (int)x.CategoryID).ToArray();

        Categories.ClearAndAddRange(context.tdCategory.ToList().Select(x => new Category(x.CategoryID, x.Description, false)).ToList());

        Categories.Where(x => results.Contains(x.CategoryId))
                  .ToList()
                  .ForEach(x => x.IsUsed = true);
      }
    }

    private void UpdateData()
    {
      if (!_loaded) return;

      using (var context = new ExpensesEntities())
      {
        var results = Categories.Where(x => x.IsUsed == true).Select(x => (int)x.CategoryId).ToArray();

        var newInput = new TransactionSummationByDurationInput(_person.PersonId, _start, _end, _selectedFrequency, false, results);
        var serialization = newInput.SerializeToXml();

        //TODO: fix the positioning that must readjusted once a ceiling is applied or grouping will be screwed up.
        _data.ClearAndAddRange(context.spTransactionSummationByDuration(serialization).ToList());//.Where(x => x.Amount >= _ceiling));
      }
                     
      UpdateHeader();
    }

    private void UpdatePlotTrendsFromData()
    {
      if (!(_data.Any())) return;

      if (InstanceConverter != null)
      { 
        InstanceConverter.OptionalHeader = SelectedFrequency.ToString();
        InstanceConverter.FirstPosition = _data.Select(x => x.Position.Value).First();
              
        ChartData.ClearAndAddRange(_data.Select(cat => new { CategoryId = cat.CategoryId, CategoryName = cat.CategoryName })
          .Distinct()
          .OrderBy(x => x.CategoryName)
          .ToList()
          .Select((x, ind) => new { x.CategoryId, x.CategoryName, Index = ind })
          .ToList()
          .Select(cat => new PlotTrend(cat.CategoryName, ColorDictionary.Color[cat.Index + 1], new Thickness(2),
                      _data.Where(x => x.CategoryId == cat.CategoryId).Select(x => new PlotPoints(new PlotPoint<int>(x.Position.Value), new PlotPoint<decimal>(x.Amount.Value))))
          ));
      }
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
