using MoneyEntry.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace MoneyEntry.ViewModel
{
  public class ChartViewModel : WorkspaceViewModel
  {
    Person _Person;
    ObservableCollection<tdCategory> _categories;

    RelayCommand _testCommand;

    private string _Desc;
    private string _CurrentCategory;

    ExpensesEntities ee = new ExpensesEntities();

    public ChartViewModel(Person person)
    {
      _Person = person;                                  
    }

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

    private void Test()
    {
      MessageBox.Show("Test Call");
    }

    public override string DisplayName
    {
      get { return "Charting (" + _Person.FirstName + ")"; }
    }
                                                                
  }
}
