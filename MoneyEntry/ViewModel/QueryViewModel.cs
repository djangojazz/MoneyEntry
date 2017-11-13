using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MoneyEntry.Model;
using MoneyEntry.Properties;
using Controls;

namespace MoneyEntry.ViewModel
{
  class QueryViewModel : WorkspaceViewModel
  {
    Person _Person;
    RelayCommand _Find;
    string _Desc;
    decimal _MoneyAmount;
    TypeTran _CurrentType;
    Category _CurrentCategory;
    DateTime _RefreshStart;
    DateTime _RefreshEnd;
    
    public QueryViewModel(Person person)
    {
      _Person = person;
      CurrentType = Types.Single(x => x.TypeName == "Debit");
      CurrentCategory = Categories.FirstOrDefault(x => x.CategoryName == "Food");
      var lastReconciledDate = Repository.LastDateEnteredByPerson(_Person.PersonId, true);
      var lastDate = Repository.LastDateEnteredByPerson(_Person.PersonId);
      RefreshStart = (lastReconciledDate != null) ? lastReconciledDate.Value : lastDate ?? DateTime.Now.Date.AddDays(-7);
      RefreshEnd = DateTime.Now;
      MoneyEntries.ClearAndAddRange(Repository.QueryMoneyEntries(RefreshStart, RefreshEnd, _Person.PersonId, CurrentCategory.CategoryId, CurrentType.TypeId));
    }

    #region Properties

    public ObservableCollection<MoneyEntryObservable> MoneyEntries { get; } = new ObservableCollection<MoneyEntryObservable>();

    public TypeTran CurrentType
    {
      get => _CurrentType;
      set
      {
        _CurrentType = value;
        OnPropertyChanged(nameof(CurrentType));
      }
    }

    public Category CurrentCategory
    {
      get => _CurrentCategory;
      set
      {
        _CurrentCategory = value;
        OnPropertyChanged(nameof(CurrentCategory));
      }
    }

    public string Desc
    {
      get => _Desc; 
      set
      {
        _Desc = value;
        OnPropertyChanged("Desc");
      }
    }

    public decimal MoneyAmount
    {
      get => _MoneyAmount; 
      set
      {
        _MoneyAmount = value;
        OnPropertyChanged("MoneyAmount");
      }
    }

    public DateTime RefreshStart
    {
      get => _RefreshStart; 
      set
      {
        _RefreshStart = value;
        OnPropertyChanged("RefreshStart");
      }
    }

    public DateTime RefreshEnd
    {
      get => _RefreshEnd; 
      set
      {
        _RefreshEnd = value;
        OnPropertyChanged("RefreshEnd");
      }
    }

    public override string DisplayName { get => Strings.QueryViewModel_DisplayName + "(" + _Person.FirstName + ")";  }

    public ICommand FindCommand { get => _Find ?? (_Find = new RelayCommand(param => Find())); }

    #endregion
    
    private void Find() => MoneyEntries.ClearAndAddRange(Repository.QueryMoneyEntries(RefreshStart, RefreshEnd, _Person.PersonId, CurrentCategory.CategoryId, CurrentType.TypeId, Desc, MoneyAmount));
  }
}
