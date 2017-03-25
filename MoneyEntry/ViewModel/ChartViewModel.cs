using MoneyEntry.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using Controls.Types;

namespace MoneyEntry.ViewModel
{
  public class ChartViewModel : WorkspaceViewModel
  {
    Person _person;                     
    RelayCommand _testCommand;

    private string _Desc;
    private string _CurrentCategory;

    ExpensesEntities ee = new ExpensesEntities();

    public ChartViewModel(Person person)
    {
      _person = person;
      Categories = ee.tdCategory.ToList().Select(x => new Category(x.CategoryID, x.Description, true)).ToList();
    }

    public Frequency[] Array
    { 
      get
      {
        return Enum.GetValues(typeof(Frequency)).Cast<Frequency>().ToArray();
      }
    }          

    public IEnumerable<Category> Categories { get; }
  
    public ICommand TestCommand
    {
      get
      {
        if (_testCommand == null)
        {
          _testCommand = new RelayCommand(param => this.Test());
        }
        return _testCommand;
      }
    }

    private bool _open;

    public bool Open
    {
      get { return _open; }
      set
      {
        _open = value;
        OnPropertyChanged("Open");
      }
    }

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


    private void UpdateHeader(object sender, ObservableCollectionContentChangedArgs e)
    {
      UpdateHeader();
    }

    private void UpdateHeader()
    {
      var itemsSelected = Categories.Where(x => x.IsUsed == true).Select(x => x.ToString());
      var headerUpdated = itemsSelected.Any() ? string.Join(", ", itemsSelected) : "No Items";
      CategoryHeader = headerUpdated;
    }

    private void Test()
    {
      MessageBox.Show("Test Call");
    }

    public override string DisplayName
    {
      get { return "Charting (" + _person.FirstName + ")"; }
    }
                                                                
  }
}
