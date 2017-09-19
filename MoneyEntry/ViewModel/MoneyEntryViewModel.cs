using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using MoneyEntry.Model;
using MoneyEntry.Properties;

namespace MoneyEntry.ViewModel
{
  public class MoneyEntryViewModel : WorkspaceViewModel, IDataErrorInfo
  {
    Person _person;
    RelayCommand _saveCommand;
    RelayCommand _refreshCommand;
    TypeTran _currentType;
    Category _currentCategory;
    string _desc;
    decimal _moneyAmount;
    DateTime _dateEntry;
    DateTime _refreshStart;
    DateTime _refreshEnd;
    bool _isSelected;

    public MoneyEntryViewModel(Person person)
      : base(DateTime.Now.Date.AddYears(-1), DateTime.Now.Date, person.PersonId)
    {
      _person = person ?? throw new ArgumentNullException("person");
      CurrentType = Repository.Types.Single(x => x.TypeId == 2); //Debit
      CurrentCategory = Repository.Categories.Single(x => x.CategoryId == 2); //Food
      DateEntry = Repository.LastDateEnteredByPerson(_person.PersonId)?.AddDays(1) ?? DateTime.Now.Date.AddDays(-7);
      RefreshStart = DateEntry.AddDays(-7);
      RefreshEnd = DateTime.Now;
      Refresh(RefreshStart, RefreshEnd, _person.PersonId);
      Validation();
    }
    
    #region MoneyEntry Properties
    
    public TypeTran CurrentType
    {
      get => _currentType;
      set
      {
        _currentType = value;
        OnPropertyChanged(nameof(CurrentType));
      }
    }

    public Category CurrentCategory
    {
      get => _currentCategory;
      set
      {
        _currentCategory = value;
        OnPropertyChanged(nameof(CurrentCategory));
      }
    }
    
    public string Desc
    {
      get => _desc;
      set
      {
        _desc = value;
        Validation();
        OnPropertyChanged(nameof(Desc));
      }
    }
    
    public decimal MoneyAmount
    {
      get => _moneyAmount;
      set
      {
        _moneyAmount = value;
        Validation();
        OnPropertyChanged(nameof(MoneyAmount));
      }
    }
    
    public DateTime DateEntry
    {
      get => _dateEntry;
      set
      {
        _dateEntry = value;
        OnPropertyChanged(nameof(DateEntry));
      }
    }
    
    public DateTime RefreshStart
    {
      get => _refreshStart;
      set
      {
        _refreshStart = value;
        OnPropertyChanged(nameof(RefreshStart));
      }
    }

    public DateTime RefreshEnd
    {
      get => _refreshEnd;
      set
      {
        _refreshEnd = value;
        OnPropertyChanged(nameof(RefreshEnd));
      }
    }

    #region IsSelected
    public bool IsSelected
    {
      get => _isSelected;
      set
      {
        if (value == _isSelected)
          return;

        _isSelected = value;
        base.OnPropertyChanged(nameof(IsSelected));
      }
    }
    #endregion

    #endregion

    #region Presentation Properties
    public override string DisplayName { get => Strings.MoneyEntryViewModel_DisplayName + "(" + _person.FullName + ")"; }

    public ICommand SaveCommand { get => (_saveCommand == null) ? _saveCommand = new RelayCommand(param => SaveAndResetAmount(), can => !HasError) : _saveCommand; }

    public ICommand RefreshCommand { get => (_refreshCommand == null) ? _refreshCommand = new RelayCommand(param => Refresh(RefreshStart, RefreshEnd, _person.PersonId)) : _refreshCommand; }

    #endregion // Presentation Properties

    private void SaveAndResetAmount()
    {
      Repository.InsertOrUpdateTransaction(new MoneyEntryObservable(_person.PersonId, Desc, DateEntry, CurrentType.TypeId, CurrentCategory.CategoryId, MoneyAmount));
      Refresh(RefreshStart, RefreshEnd, _person.PersonId);
      Desc = String.Empty;
      MoneyAmount = 0;
    }

    protected override void OnPropertyChanged(string propertyName)
    {
      if (propertyName == "Amount") { Refresh(RefreshStart, RefreshEnd, _person.PersonId); }
      else { base.OnPropertyChanged(propertyName); }
    }

    protected override void Validation()
    {
      SetError("Amount:", (MoneyAmount <= 0) ? "Need an amount." : String.Empty);
      SetError("Description:", (String.IsNullOrEmpty(Desc)) ? "Need a description for the transaction" : String.Empty);
    }
  }
}
