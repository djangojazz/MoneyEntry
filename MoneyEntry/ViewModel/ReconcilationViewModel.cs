using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Transactions;
using MoneyEntry.Model;
using MoneyEntry.DataAccess;
using MoneyEntry.Properties;


namespace MoneyEntry.ViewModel
{
  class ReconcilationViewModel : WorkspaceViewModel
  {
    Person _person;
    ReadOnlyCollection<Category> _categories;
    ReadOnlyCollection<Types> _types;
    ObservableCollection<MoneyEntryModelViewModel> _moneyentries;

    //RelayCommand _SaveCommand;
    RelayCommand _refreshCommand;
    
    #region Constructor

    public ReconcilationViewModel(Person person)
    {
      _person = person ?? throw new ArgumentNullException("person");
      CurrentType = "Debit";
      var lastreconciledate = GetLastDate(true);

      var lastdate = GetLastDate(false);

      if (lastreconciledate != DateTime.MinValue) RefreshStart = lastreconciledate; else RefreshStart = (DateTime)lastdate;

      RefreshEnd = DateTime.Now;
      _refreshCommand = new RelayCommand(param => this.Refresh());
    }

    #endregion // Constructor

    #region MoneyEntry Properties

    #region Desc
    string _desc;
    public string Desc
    {
      get { return _desc; }
      set
      {
        _desc = value;

        OnPropertyChanged("Desc");
      }
    }
    #endregion

    #region MoneyAmount
    decimal _moneyAmount;
    public decimal MoneyAmount
    {
      get { return _moneyAmount; }
      set
      {
        _moneyAmount = value;
        OnPropertyChanged("MoneyAmount");
      }
    }
    #endregion

    #region CurrentCategory
    string _currentCategory;
    public string CurrentCategory
    {
      get { return _currentCategory; }
      set
      {
        _currentCategory = value;
        OnPropertyChanged("CurrentCategory");
      }
    }
    #endregion

    #region RefreshStart
    DateTime _refreshStart;
    public DateTime RefreshStart
    {
      get { return _refreshStart; }
      set
      {
        _refreshStart = value;
        OnPropertyChanged("RefreshStart");
      }
    }

    #endregion

    #region RefreshEnd
    DateTime _refreshEnd;
    public DateTime RefreshEnd
    {
      get { return _refreshEnd; }
      set
      {
        _refreshEnd = value;
        OnPropertyChanged("RefreshEnd");
      }
    }
    #endregion

    #region IsSelected
    bool _isSelected;
    public bool IsSelected
    {
      get { return _isSelected; }
      set
      {
        if (value == _isSelected)
          return;

        _isSelected = value;
        base.OnPropertyChanged(nameof(IsSelected));
      }
    }

    private bool _onReconciled;

    public bool OnReconciled
    {
      get => _onReconciled; 
      set
      {
        _onReconciled = value;
        OnPropertyChanged(nameof(OnReconciled));
      }
    }


    #endregion

    #region MoneyEntry Collection
    
    public ObservableCollection<MoneyEntryModelViewModel> MoneyEnts
    {
      get
      {
        if (_moneyentries == null)
        {
          List<MoneyEntryModel> mems = this.GetMoneyEnts();
          List<MoneyEntryModelViewModel> mes = new List<MoneyEntryModelViewModel>();

          mems.ForEach(m => mes.Add(new MoneyEntryModelViewModel(m)));

          _moneyentries = new ObservableCollection<MoneyEntryModelViewModel>(mes);
        }

        return _moneyentries;
      }
      set
      {
        _moneyentries = value;

        OnPropertyChanged("MoneyEnts");
      }
    }

    List<MoneyEntryModel> GetMoneyEnts()
    {
      using (var ee = new ExpensesEntities())
      {
        return ee.vTrans.Where(d => d.CreatedDate >= RefreshStart && d.CreatedDate <= RefreshEnd && d.PersonID == _person.PersonId)
          .Select(m => new MoneyEntryModel()
          {
            PersonId = m.PersonID,
            Name = m.Name,
            TransactionId = m.TransactionID,
            Amount = Decimal.Round(m.Amount, 2),
            TransactionDesc = m.TransactionDesc,
            TypeId = m.TypeID,
            TypeName = m.Type,
            CategoryId = m.CategoryID,
            CategoryName = m.Category,
            CreatedDate = m.CreatedDate,
            RunningTotal = Decimal.Round(m.RunningTotal.Value, 2),
            Reconciled = m.reconciled
          }).ToList().OrderBy(d => d.CreatedDate).ToList();
      }
    }
    #endregion

    #region Category related
    public ReadOnlyCollection<Category> Categories
    {
      get
      {
        if (_categories == null)
        {
          List<Category> cats = this.GetCategories();
          _categories = new ReadOnlyCollection<Category>(cats);
        }
        return _categories;
      }
    }

    List<Category> GetCategories()
    {
      using (var ee = new ExpensesEntities())
      {
        return ee.tdCategory.Select(x => new Category()
        {
          CategoryId = x.CategoryID,
          CategoryName = x.Description
        }).ToList();
      }
    }

    #endregion

    #region Types Related
    public ReadOnlyCollection<Types> Types
    {
      get
      {
        if (_types == null)
        {
          List<Types> types = this.GetTypes();
          _types = new ReadOnlyCollection<Types>(types);
        }
        return _types;
      }
    }

    List<Types> GetTypes()
    {
      using (var ee = new ExpensesEntities())
      {
        return ee.tdType.Select(x => new Types()
        {
          TypeId = x.TypeID,
          TypeName = x.Description
        }).ToList();
      }
    }

    public string CurrentType { get; set; }
    #endregion

    #region Presentation Properties

    
    #endregion

    public override string DisplayName
    {
      get { return Strings.ReconciliationViewModel_DisplayName + "(" + _person.FirstName + ")"; }
    }

    public ICommand RefreshCommand { get => _refreshCommand; }

    #endregion // Presentation Properties
    
    #region Private Helpers

    private DateTime GetLastDate(bool reconciled)
    {
      using (var ee = new ExpensesEntities())
      {
        var trans = ee.vTrans.Where(n => n.PersonID == _person.PersonId).OrderBy(d => d.CreatedDate);
        return (reconciled) ? trans.Where(x => x.reconciled == true).Select(d => d.CreatedDate).ToList().LastOrDefault() ?? DateTime.MinValue : trans.Select(d => d.CreatedDate).ToList().FirstOrDefault() ?? DateTime.MinValue;
      }
    }

    private void Refresh()
    {
      if (_onReconciled) RefreshStart = GetLastDate(true);

      List<MoneyEntryModel> mems = GetMoneyEnts();
      List<MoneyEntryModelViewModel> mes = new List<MoneyEntryModelViewModel>();

      mems.ForEach(m => mes.Add(new MoneyEntryModelViewModel(m)));

      MoneyEnts = new ObservableCollection<MoneyEntryModelViewModel>(mes);
    }
    
    #endregion
  }
}
