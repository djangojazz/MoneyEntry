using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MoneyEntry.Model;
using MoneyEntry.Properties;
using Controls;

namespace MoneyEntry.ViewModel
{
  class ReconcilationViewModel : WorkspaceViewModel
  {
    Person _person;
    ObservableCollection<MoneyEntryModelViewModel> _moneyEnts;
    RelayCommand _refreshCommand;
    string _desc;
    decimal _moneyAmount;
    private TypeTran _currentType;
    private Category _currentCategory;
    DateTime _refreshStart;
    DateTime _refreshEnd;
    bool _isSelected;
    private bool _onReconciled;

    public ReconcilationViewModel(Person person)
    {
      _person = person;
      _currentType = Types.Single(x => x.TypeName == "Debit");
      var lastreconciledate = Repository.LastDateEnteredByPerson(_person.PersonId, true);
      var lastdate = Repository.LastDateEnteredByPerson(_person.PersonId, false);
      RefreshStart = (lastreconciledate != null) ? lastreconciledate.Value : lastdate ?? DateTime.MinValue;
      RefreshEnd = DateTime.Now;
      Repository.Refresh(RefreshStart, RefreshEnd, _person.PersonId);
      MoneyEnts = new ObservableCollection<MoneyEntryModelViewModel>(Repository.MoneyEntryContainer);
    }

    #region MoneyEntry Properties
    public ObservableCollection<MoneyEntryModelViewModel> MoneyEnts
    {
      get => _moneyEnts;
      set
      {
        _moneyEnts = value;
        OnPropertyChanged(nameof(MoneyEnts));
      }
    }

    public string Desc
    {
      get => _desc;
      set
      {
        _desc = value;
        OnPropertyChanged(nameof(Desc));
      }
    }

    public decimal MoneyAmount
    {
      get => _moneyAmount;
      set
      {
        _moneyAmount = value;
        OnPropertyChanged(nameof(MoneyAmount));
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

    public bool OnReconciled
    {
      get => _onReconciled;
      set
      {
        _onReconciled = value;
        OnPropertyChanged(nameof(OnReconciled));
      }
    }

    public override string DisplayName { get => Strings.ReconciliationViewModel_DisplayName + "(" + _person.FirstName + ")"; }
    #endregion
   
    public ICommand RefreshCommand { get => (_refreshCommand == null) ? _refreshCommand = new RelayCommand(param => Refresh()) : _refreshCommand; }
    
    private void Refresh()
    {
      RefreshStart = _onReconciled ? Repository.LastDateEnteredByPerson(_person.PersonId, true) ?? DateTime.MinValue : RefreshStart;
      Repository.Refresh(RefreshStart, RefreshEnd, _person.PersonId);
      MoneyEnts.ClearAndAddRange(Repository.MoneyEntryContainer);
    }
    
  }
}
