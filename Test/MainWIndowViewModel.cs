using Controls;
using Controls.Types;
using MoneyEntry;
using MoneyEntry.Model;
using MoneyEntry.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Test
{
  public sealed class MainWIndowViewModel : WorkspaceViewModel
  {
    RelayCommand _get;
    ObservableCollection<Person> _people;
    private Person _currentUser;

    public MainWIndowViewModel()
    {
      People = new ObservableCollection<Person>(Repository.People);
      _currentUser = _people.FirstOrDefault(x => x.FirstName == "Test");
      System.DateTime start = new System.DateTime(2017, 9, 1);
      System.DateTime end = new System.DateTime(2017, 9, 15);
      Refresh(start, end, _currentUser.PersonId);
      
      MoneyEnts2 = new ItemObservableCollection<MoneyEntryObservable>();
      MoneyEnts2.ClearAndAddRange(Repository.GetModelObservables(start, end, _currentUser.PersonId));
      MoneyEnts2.CollectionChanged += ModifyCollectionsBindings;
    }
    

    public ItemObservableCollection<MoneyEntryObservable> MoneyEnts2 { get; set; }

    public ObservableCollection<Person> People
    {
      get => _people;
      set
      {
        _people = value;
        OnPropertyChanged(nameof(People));
      }
    }

    public Person CurrentUser
    {
      get { return _currentUser; }
      set
      {
        _currentUser = value;
        OnPropertyChanged(nameof(CurrentUser));
      }
    }

    public ICommand GetCommand { get => (_get == null) ? _get = new RelayCommand(param => MessageBox.Show(CurrentUser.FirstName)) : _get; }

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

    private void CascadeEvent(object sender, PropertyChangedEventArgs e) => OnPropertyChanged(e.PropertyName);
    #endregion
  }
}
