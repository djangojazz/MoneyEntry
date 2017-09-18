using MoneyEntry;
using MoneyEntry.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Test
{
  public sealed class MainWIndowViewModel : ViewModelBase
  {
    RelayCommand _get;
    ObservableCollection<Person> _people;
    private Person _currentUser;

    public MainWIndowViewModel()
    {
      People = new ObservableCollection<Person>(Repository.People);
      _currentUser = _people.FirstOrDefault(x => x.FirstName == "Shared");
    }

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
  }
}
