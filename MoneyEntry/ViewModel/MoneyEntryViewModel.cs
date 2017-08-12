using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using MoneyEntry.Model;
using MoneyEntry.Properties;
using MoneyEntry.DataAccess;

namespace MoneyEntry.ViewModel
{
  public class MoneyEntryViewModel : WorkspaceViewModel, IDataErrorInfo
  {
    Person _person;
    //ReadOnlyCollection<Category> _categories;
    RelayCommand _SaveCommand;
    RelayCommand _RefreshCommand;

    #region Constructor

    public MoneyEntryViewModel(Person person)
    {
      _person = person ?? throw new ArgumentNullException("person");
      CurrentType = Repository.Types.Single(x => x.TypeId == 2); //Debit
      CurrentCategory = Repository.Categories.Single(x => x.CategoryId == 2); //Food
      DateEntry = DateTime.Now.Subtract(TimeSpan.FromDays(28));

      RefreshStart = Repository.LastDateEnteredByPerson(_person.PersonId) ?? DateTime.Now.Date.AddDays(-7);
      RefreshEnd = DateTime.Now;
      MoneyEnts = new ReadOnlyCollection<MoneyEntryModelViewModel>(Repository.MoneyEntryContainer);
    }

    #endregion // Constructor

    #region MoneyEntry Properties
    public ReadOnlyCollection<MoneyEntryModelViewModel> MoneyEnts { get; }

    #region CurrentType
    TypeTran _currentType;
    public TypeTran CurrentType
    {
      get => _currentType; 
      set
      {
        _currentType = value;
        OnPropertyChanged("CurrentType");
      }
    }
    #endregion

    #region CurrentCategory
    Category _CurrentCategory;
    public Category CurrentCategory
    {
      get => _CurrentCategory; 
      set
      {
        _CurrentCategory = value;
        OnPropertyChanged("CurrentCategory");
      }
    }
    #endregion

    #region Desc
    string _Desc;
    public string Desc
    {
      get => _Desc; 
      set
      {
        _Desc = value;
        OnPropertyChanged("Desc");
      }
    }
    #endregion

    #region MoneyAmount
    decimal _MoneyAmount;
    public decimal MoneyAmount
    {
      get => _MoneyAmount; 
      set
      {
        _MoneyAmount = value;
        OnPropertyChanged("MoneyAmount");
      }
    }
    #endregion

    #region DateEntry
    DateTime _DateEntry;
    public DateTime DateEntry
    {
      get => _DateEntry; 
      set
      {
        _DateEntry = value;
        OnPropertyChanged("DateEntry");
      }
    }
    #endregion

    #region RefreshStart
    DateTime _RefreshStart;

    public DateTime RefreshStart
    {
      get => _RefreshStart; 
      set
      {
        _RefreshStart = value;
        OnPropertyChanged("RefreshStart");
      }
    }
    #endregion

    #region RefreshEnd
    DateTime _RefreshEnd;

    public DateTime RefreshEnd
    {
      get => _RefreshEnd; 
      set
      {
        _RefreshEnd = value;
        OnPropertyChanged("RefreshEnd");
      }
    }
    #endregion

    #region IsSelected
    bool _isSelected;
    public bool IsSelected
    {
      get => _isSelected; 
      set
      {
        if (value == _isSelected)
          return;

        _isSelected = value;
        base.OnPropertyChanged("IsSelected");
      }
    }
    #endregion
    
    #endregion

    #region Presentation Properties
    public override string DisplayName { get => Strings.MoneyEntryViewModel_DisplayName + "(" + _person.FullName + ")"; }
    
    public ICommand SaveCommand { get => (_SaveCommand == null) ? _SaveCommand = new RelayCommand(param => SaveAndResetAmount()) : _SaveCommand; }

    public ICommand RefreshCommand { get => (_RefreshCommand == null) ? _RefreshCommand = new RelayCommand(param => Refresh()) : _RefreshCommand; }

    #endregion // Presentation Properties

    #region Public Methods

    #endregion // Public Methods

    #region Private Helpers

    private void Refresh() => Repository.Refresh(RefreshStart, RefreshEnd, _person.PersonId);
    
    private void SaveAndResetAmount()
    {
      Repository.InsertOrUpdateTransaction(new TransactionView(_person, CurrentType, CurrentCategory, MoneyAmount, Desc, DateEntry));
      MoneyAmount = 0;
      Desc = String.Empty;
    }

    #endregion // Private Helpers

    #region IDataErrorInfo Members

    //TODO: Hookup IDataError properly for description, amount, category to be selected first.
    string IDataErrorInfo.Error { get => (_person as IDataErrorInfo).Error; }

    string IDataErrorInfo.this[string propertyName]
    {
      get
      {
        string error = null;
        return error;
      }
    }

    #endregion // IDataErrorInfo Members
  }
}
