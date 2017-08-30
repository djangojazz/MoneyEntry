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

namespace MoneyEntry.ViewModel
{
  public abstract class WorkspaceViewModel : ViewModelBase
  {
    public ExpensesRepo Repository;
    RelayCommand _closeCommand;

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
    
    public ReadOnlyCollection<TypeTran> Types { get; }
    public ObservableCollection<Category> Categories { get; set; }
    public ItemObservableCollection<MoneyEntryModelViewModel> MoneyEnts { get; }
    
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
  }
}