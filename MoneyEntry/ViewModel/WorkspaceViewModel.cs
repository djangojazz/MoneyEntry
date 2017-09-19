using MoneyEntry.DataAccess;
using MoneyEntry.Model;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Controls;
using System.ComponentModel;
using System.Collections.Generic;
using Controls.Types;
using System.Windows;

namespace MoneyEntry.ViewModel
{
  public abstract class WorkspaceViewModel : ViewModelBase, IDataErrorInfo
  {
    public ExpensesRepo Repository;
    RelayCommand _closeCommand;
    private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();
    
    protected WorkspaceViewModel()
    {
      Repository = ExpensesRepo.Instance;
      Types = new ReadOnlyCollection<TypeTran>(Repository.Types);
      Categories = new ObservableCollection<Category>(Repository.Categories);
      MoneyEnts.ClearAndAddRange(Repository.MoneyEntryContainer);
      MoneyEnts.CollectionChanged += ModifyCollectionsBindings;
    }

    protected WorkspaceViewModel(DateTime start, DateTime end, int personId)
    {
      Repository = ExpensesRepo.Instance;
      Types = new ReadOnlyCollection<TypeTran>(Repository.Types);
      Categories = new ObservableCollection<Category>(Repository.Categories);
      TextCollection = new ReadOnlyCollection<string>(Repository.TextEntryAcrossRange(start, end, personId));
      MoneyEnts.ClearAndAddRange(Repository.MoneyEntryContainer);
      MoneyEnts.CollectionChanged += ModifyCollectionsBindings;
    }

    #region Properties
    private string _errorList;

    public string ErrorList
    {
      get => _errorList;
      set
      {
        _errorList = value;
        OnPropertyChanged(nameof(ErrorList));
      }
    }

    private System.Windows.Visibility _errorVisible;

    public System.Windows.Visibility ErrorVisible
    {
      get => _errorVisible;
      set
      {
        _errorVisible = value;
        OnPropertyChanged(nameof(ErrorVisible));
      }
    }

    private bool _hasError;

    public bool HasError
    {
      get => _hasError;
      set
      {
        _hasError = value;
        ErrorVisible = !_hasError ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
        OnPropertyChanged(nameof(HasError));
      }
    }

    public string Error => throw new NotImplementedException();
    public ReadOnlyCollection<TypeTran> Types { get; }
    public ObservableCollection<Category> Categories { get; set; }
    public ObservableCollection<MoneyEntryObservable> MoneyEnts { get; } = new ObservableCollection<MoneyEntryObservable>();
    public ReadOnlyCollection<string> TextCollection { get; }
    #endregion

    public ICommand CloseCommand { get => (_closeCommand == null) ? _closeCommand = new RelayCommand(param => OnRequestClose()) : _closeCommand; }

    public event EventHandler RequestClose;
    protected void OnRequestClose() => RequestClose?.Invoke(this, EventArgs.Empty);
    
    protected void Refresh(DateTime start, DateTime end, int personId, bool lastReconciled = false)
    {
      var refreshStart = Repository.LastDateEnteredByPerson(personId, lastReconciled);
      Repository.Refresh(lastReconciled ? (refreshStart ?? start) : start, end, personId);
      MoneyEnts.ClearAndAddRange(Repository.MoneyEntryContainer);
    }

    #region CollectionChanges
    private void ModifyCollectionsBindings(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      if (e?.OldItems != null)
      {
        foreach (var arg in e?.OldItems) { ((INotifyPropertyChanged)arg).PropertyChanged -= CascadeEvent; base.OnPropertyChanged(arg.ToString()); }
      }

      if (e?.NewItems != null)
      {
        foreach (var arg in e?.NewItems) { ((INotifyPropertyChanged)arg).PropertyChanged += CascadeEvent; base.OnPropertyChanged(arg.ToString()); }
      }
    }
    
    private void CascadeEvent(object sender, PropertyChangedEventArgs e)
    {
      if(sender.GetType() == typeof(MoneyEntryObservable))
      {
        var entry = (MoneyEntryObservable)sender;
        Repository.InsertOrUpdateTransaction(entry);
      }

      //This will refresh the data grid views
      OnPropertyChanged(e.PropertyName);
    }
    #endregion

    #region DataErrorValidation
    public void SetError(string PropertyName, string PassedError)
    {
      if (PassedError.Length == 0 && _errors.ContainsKey(PropertyName)) { _errors.Remove(PropertyName.ToString()); }
      else if (PassedError.Length > 0 && !_errors.ContainsKey(PropertyName)) { _errors.Add(PropertyName, PassedError); }
      else if (PassedError.Length > 0 && _errors.ContainsKey(PropertyName)) { _errors[PropertyName] = PassedError; }

      if (HasError != (_errors.Count > 0)) { HasError = (_errors.Count > 0); }

      ErrorList = _errors.GetStringListings();
    }

    public string this[string PropertyName] { get => _errors.ContainsKey(PropertyName) ? _errors[PropertyName] : string.Empty; }

    protected virtual void Validation() { } 
    #endregion
  }
}