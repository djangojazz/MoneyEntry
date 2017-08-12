using MoneyEntry.DataAccess;
using MoneyEntry.Model;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MoneyEntry.ViewModel
{
  public abstract class WorkspaceViewModel : ViewModelBase
  {
    ReadOnlyCollection<Category> _categories;
    ReadOnlyCollection<TypeTran> _types;

    public ExpensesRepo Repository;
    RelayCommand _closeCommand;
    
    protected WorkspaceViewModel()
    {
      Repository = ExpensesRepo.Instance;
    }
    
    public ICommand CloseCommand { get => (_closeCommand == null) ? _closeCommand = new RelayCommand(param => this.OnRequestClose()) : _closeCommand; }
    
    public event EventHandler RequestClose;
    void OnRequestClose() => RequestClose?.Invoke(this, EventArgs.Empty);


    public ReadOnlyCollection<TypeTran> Types { get => (_types == null) ? _types = new ReadOnlyCollection<TypeTran>(Repository.Types.ToList()) : _types; }

    public ReadOnlyCollection<Category> Categories { get => (_categories == null) ? _categories = new ReadOnlyCollection<Category>(Repository.Categories) : _categories; }

  }
}