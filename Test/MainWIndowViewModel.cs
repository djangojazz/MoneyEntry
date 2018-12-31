using Controls;
using Controls.Types;
using MoneyEntry;
using MoneyEntry.Model;
using MoneyEntry.ViewModel;
using System.Collections.Generic;
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
    Person _currentUser;

    

    public MainWIndowViewModel()
    {
      People = new ObservableCollection<Person>(Repository.People);
      var personId = _people.FirstOrDefault(x => x.FirstName == "Test").PersonId;

      System.DateTime start = new System.DateTime(2018, 12, 1);
      System.DateTime end = new System.DateTime(2018, 12, 15);
      Refresh(start, end, personId);

      Types2 = new List<TypeTran>(new List<TypeTran> { new TypeTran(1, "Debit"), new TypeTran(2, "Credit") });
      Types3 = new Dictionary<byte, string> { { 1, "Debit" }, { 2, "Credit" } };

      MoneyEnts2 = new ItemObservableCollection<MoneyEntryObservable>();
      MoneyEnts2.ClearAndAddRange(Repository.GetModelObservables(start, end, personId));
      //MoneyEnts2.CollectionChanged += ModifyCollectionsBindings;

      MoneyEnts3 = new ObservableCollection<MoneyEntryObservable>(Repository.GetModelObservables(start, end, personId));
    }
    
    public List<TypeTran> Types2 { get; set; }
    public Dictionary<byte, string> Types3 { get; set; }

    public ItemObservableCollection<MoneyEntryObservable> MoneyEnts2 { get; set; }
    public ObservableCollection<MoneyEntryObservable> MoneyEnts3 { get; set; }

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

    //private void FakeRepo()
    //{
    //  var data = new List<TransactionSimple>
    //  {
    //    new TransactionSimple(1, "Got some money", 1, 1000m),
    //    new TransactionSimple(2, "spent some money", 2, 100m),
    //    new TransactionSimple(3, "spent some more money", 2, 300m)
    //  };
    //}

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
