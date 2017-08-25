using MoneyEntry.DataAccess;
using MoneyEntry.Model;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Controls;

namespace MoneyEntry.ViewModel
{
  public abstract class WorkspaceViewModel : ViewModelBase
  {
    //ReadOnlyCollection<Category> _categories;

    public ExpensesRepo Repository;
    RelayCommand _closeCommand;
    

    protected WorkspaceViewModel()
    {
      Repository = ExpensesRepo.Instance;
      Types = new ReadOnlyCollection<TypeTran>(Repository.Types);
      Categories = new ObservableCollection<Category>(Repository.Categories);
      MoneyEnts = new ObservableCollection<MoneyEntryModelViewModel>(Repository.MoneyEntryContainer);
    }
    
    public ICommand CloseCommand { get => (_closeCommand == null) ? _closeCommand = new RelayCommand(param => this.OnRequestClose()) : _closeCommand; }
    
    public event EventHandler RequestClose;
    void OnRequestClose() => RequestClose?.Invoke(this, EventArgs.Empty);
    
    public ReadOnlyCollection<TypeTran> Types { get; }
    public ObservableCollection<Category> Categories { get; set; }
    public ObservableCollection<MoneyEntryModelViewModel> MoneyEnts { get; }

    

    protected void Refresh(DateTime start, DateTime end, int personId)
    {
      Repository.Refresh(start, end, personId);
      MoneyEnts.ClearAndAddRange(Repository.MoneyEntryContainer);
    }
  }
}