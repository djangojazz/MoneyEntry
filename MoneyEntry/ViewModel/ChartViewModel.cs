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

    public ChartViewModel(Person person)
    {
      //Setup
      _person = person;
      _testCommand = new RelayCommand(param => this.Test());
      Categories = new ObservableCollectionContentNotifying<Category>();
      Start = DateTime.Now.Date.AddMonths(-2);
      End = DateTime.Now.Date;
      Ceiling = 100;
      
      UpdateDataForCharting();
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
        if (_loaded)
        {
          //UpdateHeader();
          UpdateDataForCharting();
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
        UpdateDataForCharting();
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
        UpdateDataForCharting();
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
        UpdateDataForCharting();
        OnPropertyChanged("Ceiling");
      }
    }
#endregion

    public ObservableCollectionContentNotifying<PlotTrend> ChartData;

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
    private void UpdateDataForCharting()
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

        var data = context.spTransactionSummationByDuration(serialization).ToList();
        var more = data.Select(x => x.Amount).ToList().First();
      }
                     
      UpdateHeader();
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
