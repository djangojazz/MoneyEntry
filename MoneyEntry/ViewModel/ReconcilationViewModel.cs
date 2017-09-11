using System;
using System.Windows.Input;
using MoneyEntry.Model;
using MoneyEntry.Properties;

namespace MoneyEntry.ViewModel
{
  class ReconcilationViewModel : WorkspaceViewModel
  {
    Person _person;
    RelayCommand _refreshCommand;
    DateTime _refreshStart;
    DateTime _refreshEnd;
    private bool _onReconciled;

    public ReconcilationViewModel(Person person)
    {
      _person = person;
      var lastreconciledate = Repository.LastDateEnteredByPerson(_person.PersonId, true);
      var lastdate = Repository.LastDateEnteredByPerson(_person.PersonId, false);
      RefreshStart = (lastreconciledate != null) ? lastreconciledate.Value : lastdate ?? DateTime.MinValue;
      RefreshEnd = DateTime.Now;
      Refresh(RefreshStart, RefreshEnd, _person.PersonId);
    }

    #region MoneyEntry Properties

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
   
    public ICommand RefreshCommand { get => (_refreshCommand == null) ? _refreshCommand = new RelayCommand(param => Refresh(RefreshStart, RefreshEnd, _person.PersonId, OnReconciled)) : _refreshCommand; }

    protected override void OnPropertyChanged(string propertyName)
    {
      if (propertyName == "Amount") { Refresh(RefreshStart, RefreshEnd, _person.PersonId); }
      else { base.OnPropertyChanged(propertyName); }
    }
  }
}
