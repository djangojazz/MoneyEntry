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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation;

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
      MoneyEnts = new ItemObservableCollection<MoneyEntryModelViewModel>();
      MoneyEnts.ClearAndAddRange(Repository.MoneyEntryContainer);
      MoneyEnts.CollectionChanged += ModifyCollectionsBindings;
    }
    
    public ICommand CloseCommand { get => (_closeCommand == null) ? _closeCommand = new RelayCommand(param => OnRequestClose()) : _closeCommand; }
    
    public event EventHandler RequestClose;
    protected void OnRequestClose() => RequestClose?.Invoke(this, EventArgs.Empty);

    public string this[string PropertyName] { get => _errors.ContainsKey(PropertyName) ? _errors[PropertyName] : string.Empty; }


    public string ErrorList { get; private set; }
    public bool HasError { get; private set; }
    public ReadOnlyCollection<TypeTran> Types { get; }
    public ObservableCollection<Category> Categories { get; set; }
    public ItemObservableCollection<MoneyEntryModelViewModel> MoneyEnts { get; }

    public string Error => throw new NotImplementedException();

    protected void Refresh(DateTime start, DateTime end, int personId, bool lastReconciled = false)
    {
      var refreshStart = Repository.LastDateEnteredByPerson(personId, lastReconciled);
      Repository.Refresh(lastReconciled ? (refreshStart ?? start) : start, end, personId);
      MoneyEnts.ClearAndAddRange(Repository.MoneyEntryContainer);
    }

    private void ModifyCollectionsBindings(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      if (e?.OldItems != null)
      {
        foreach (var arg in e?.OldItems) {  ((INotifyPropertyChanged)arg).PropertyChanged -= CascadeEvent; base.OnPropertyChanged(arg.ToString()); }
      }

      if (e?.NewItems != null)
      {
        foreach (var arg in e?.NewItems) { ((INotifyPropertyChanged)arg).PropertyChanged += CascadeEvent; base.OnPropertyChanged(arg.ToString()); }
      }
    }

    private void CascadeEvent(object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e.PropertyName);

    public void SetError(string PropertyName, string PassedError)
    {
      if (PassedError.Length == 0 && _errors.ContainsKey(PropertyName)) { _errors.Remove(PropertyName.ToString()); }
      else if (PassedError.Length > 0 && !_errors.ContainsKey(PropertyName)) { _errors.Add(PropertyName, PassedError); }
      else if (PassedError.Length > 0 && _errors.ContainsKey(PropertyName)) { _errors[PropertyName] = PassedError; }

      if (HasError != (_errors.Count > 0)) { HasError = (_errors.Count > 0); }

      ErrorList = _errors.GetStringListings();
    }

    protected virtual void Validation() { }
  }
}