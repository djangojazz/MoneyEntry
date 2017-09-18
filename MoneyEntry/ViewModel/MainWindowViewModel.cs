using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MoneyEntry.Model;
using MoneyEntry.Properties;
using System.Configuration;
using MoneyEntry.DataAccess;

namespace MoneyEntry.ViewModel
{
  public class MainWindowViewModel : WorkspaceViewModel
  {
    ReadOnlyCollection<CommandViewModel> _commands;
    RelayCommand _Exit;
    RelayCommand _OpenLoc;
    RelayCommand _BackUp;
    RelayCommand _Restore;
    ObservableCollection<Person> _people;
    private Person _currentUser;
    ObservableCollection<WorkspaceViewModel> _workspaces;

    private string _BackupLocation { get; set; }
    private string _InitialBackupLocation { get; set; }
    SQLTalker s = new SQLTalker();
   
    public MainWindowViewModel()
    {
      base.DisplayName = Strings.MainWindowViewModel_DisplayName;
      People = new ObservableCollection<Person>(Repository.GetPeople());

      _currentUser = _people.FirstOrDefault(x => x.FirstName == "Test");
      
      _BackupLocation = ConfigurationManager.AppSettings["DatabaseBackupsLocation"]; // Start the initial backup

      if (!Directory.Exists(_BackupLocation)) { Directory.CreateDirectory(_BackupLocation); }

      DateTime dt = DateTime.Now;
      _InitialBackupLocation = _BackupLocation + "\\ExpensesStart_" + dt.Year + "-" + dt.Month + "-" + dt.Day + ".bak";

      if (!File.Exists(_InitialBackupLocation)) { BackUpDB(true); }  // initial backup on startup
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

    public ReadOnlyCollection<CommandViewModel> Commands { get => (_commands == null)  ? _commands = new ReadOnlyCollection<CommandViewModel>(CreateCommands()) : _commands;  }
    public ICommand ExitCommand { get => (_Exit == null) ? _Exit = new RelayCommand(param => Exit()) : _Exit; }
    public ICommand OpenLocationCommand { get => (_OpenLoc == null) ? _OpenLoc = new RelayCommand(param => OpenBackupLocation()) : _OpenLoc; }
    public ICommand BackupDBCommand { get => (_BackUp == null) ? _BackUp = new RelayCommand(param => this.BackUpDB(false)) : _BackUp; }
    public ICommand RestoreDBCommand { get => (_Restore == null) ? _Restore = new RelayCommand(param => this.RestoreDB()) : _Restore; }
    

    #region Workspaces
    
    public ObservableCollection<WorkspaceViewModel> Workspaces
    {
      get
      {
        if (_workspaces == null)
        {
          _workspaces = new ObservableCollection<WorkspaceViewModel>();
          _workspaces.CollectionChanged += OnWorkspacesChanged;
        }
        return _workspaces;
      }
    }

    void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.NewItems != null && e.NewItems.Count != 0) 
        foreach (WorkspaceViewModel workspace in e.NewItems)
          workspace.RequestClose += this.OnWorkspaceRequestClose;

      if (e.OldItems != null && e.OldItems.Count != 0)
        foreach (WorkspaceViewModel workspace in e.OldItems)
          workspace.RequestClose -= this.OnWorkspaceRequestClose;
    }

    void OnWorkspaceRequestClose(object sender, EventArgs e)
    {
      WorkspaceViewModel workspace = sender as WorkspaceViewModel;
      workspace.Dispose();
      Workspaces.Remove(workspace);
    }


    #endregion // Workspaces

    #region Private Helpers
    private List<CommandViewModel> CreateCommands()
    {
      return new List<CommandViewModel>
      {
          new CommandViewModel("Add Category", new RelayCommand(param => CategorieEntries())),
          new CommandViewModel("MoneyEntry",   new RelayCommand(param => MoneyEntry())),
          new CommandViewModel("Reconciliation", new RelayCommand(param => Reconciliation())),
          new CommandViewModel("Search",  new RelayCommand(param => Query())),
          new CommandViewModel("Charting",  new RelayCommand(param => Chart())),
      };
    }

    int ConvertToNumber(string s)
    {
      try { return Convert.ToInt32(s); }
      catch (FormatException) { return 0; }
    }
    
    private void MoneyEntry()
    {
      MoneyEntryViewModel money = new MoneyEntryViewModel(_currentUser);
      Workspaces.Add(money);
      SetActiveWorkspace(money);
    }

    private void Query()
    {
      QueryViewModel query = new QueryViewModel(_currentUser);
      Workspaces.Add(query);
      SetActiveWorkspace(query);
    }

    private void Reconciliation()
    {
      ReconcilationViewModel reconcile = new ReconcilationViewModel(_currentUser);
      Workspaces.Add(reconcile);
      SetActiveWorkspace(reconcile);
    }

    private void CategorieEntries()
    {
      CategoryViewModel category = new CategoryViewModel(_currentUser);
      Workspaces.Add(category);
      SetActiveWorkspace(category);
    }

    private void Chart()
    {
      ChartViewModel chart = new ChartViewModel(_currentUser);
      Workspaces.Add(chart);
      SetActiveWorkspace(chart);
    }
    
    void SetActiveWorkspace(WorkspaceViewModel workspace)
    {
      Debug.Assert(Workspaces.Contains(workspace));

      ICollectionView collectionView = CollectionViewSource.GetDefaultView(Workspaces);
      if (collectionView != null)
        collectionView.MoveCurrentTo(workspace);
    }

    void OpenBackupLocation()
    {
      s.OpenLocation(_BackupLocation);
    }

    void Exit()
    {
      Application.Current.Shutdown();
    }

    void BackUpDB(bool aStartup)
    {
      s.BackupDB(_BackupLocation, aStartup);
    }

    void RestoreDB()
    {
      s.RestoreDB(_BackupLocation);
    }

    #endregion
  }
}