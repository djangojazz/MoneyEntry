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
      _person = person;
      _testCommand = new RelayCommand(param => this.Test());

      Categories = new ObservableCollectionContentNotifying<Category>();
      using (var context = new ExpensesEntities())
      {                                                                                                                               
        Categories.ClearAndAddRange(context.tdCategory.ToList().Select(x => new Category(x.CategoryID, x.Description, false)).ToList());
      }

      var newInput = new TransactionSummationByDurationInput(3, new DateTime(2017, 1, 1), DateTime.Now, Frequency.Month, false, new int[] { 2, 10 ,17 });
      var serialization = newInput.SerializeToXml();

      Categories.Skip(10).Take(1).First().IsUsed = true;
      Start = DateTime.Now.Date.AddMonths(-3);
      End = DateTime.Now;
      UpdateHeader();                                        
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
          UpdateHeader();
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
        OnPropertyChanged("End");
      }
    }
    #endregion
                                                                
    public ObservableCollectionContentNotifying<PlotTrend> ChartData;
               
    #endregion

    #region TestCommandAndMethod
    public ICommand TestCommand  { get => _testCommand; }

    private void Test()
    {
      var s = String.Empty;
      Categories.Where(x => x.IsUsed == true).ToList().ForEach(x => s += x + Environment.NewLine);
      MessageBox.Show(s);
    }
    #endregion

    #region Methods
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
