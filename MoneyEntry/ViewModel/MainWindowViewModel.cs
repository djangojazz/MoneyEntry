using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MoneyEntry.DataAccess;
using MoneyEntry.Model;
using MoneyEntry.Properties;

namespace MoneyEntry.ViewModel
{
    /// <summary>
    /// The ViewModel for the application's main window.
    /// </summary>
    public class MainWindowViewModel : WorkspaceViewModel
    {
        #region Fields
            
        ReadOnlyCollection<CommandViewModel> _commands;
        RelayCommand _Exit;
        RelayCommand _OpenLoc;
        RelayCommand _BackUp;
        RelayCommand _Restore;

        Person _currentPerson;
        ReadOnlyCollection<Person> _people;
        ObservableCollection<WorkspaceViewModel> _workspaces;
        
        private string _BackupLocation { get; set; }
        private string _InitialBackupLocation { get; set; }
        
        public string CurrentUser { get; set; }


        ExpensesEntities ee = new ExpensesEntities();
        SQLTalker s = new SQLTalker();

        #endregion // Fields

        #region Constructor

        public MainWindowViewModel(string customerDataFile)
        {
            base.DisplayName = Strings.MainWindowViewModel_DisplayName;


            CurrentUser = ee.tePerson.Where(n => n.FirstName == "Shared")
                        .Select(x => x.PersonID).FirstOrDefault().ToString();

            DateTime dt = DateTime.Now;

            // Start the initial backup
            _BackupLocation = "C:\\SQLServer\\Backups";
            _InitialBackupLocation = _BackupLocation +  "\\ExpensesStart_" + dt.Year + "-" + dt.Month + "-" + dt.Day + ".bak";

            if (!File.Exists(_InitialBackupLocation))
            {
                BackUpDB(true);  // initial backup on startup
            }

        }

        #endregion // Constructor

        #region People

        public ReadOnlyCollection<Person> People
        {
            get
            {
                if (_people == null)
                {
                    List<Person> persns = this.GetPeople();
                    _people = new ReadOnlyCollection<Person>(persns);
                }
                return _people;
            }
        }


        List<Person> GetPeople()
        {
            return ee.tePerson.Select(x => new Person
                                         {
                                             PersonId = x.PersonID,
                                             FirstName = x.FirstName
                                         }).ToList();
        }

        #endregion

        #region Commands

        /// <summary>
        /// Returns a read-only list of commands 
        /// that the UI can display and execute.
        /// </summary>
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    _commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _commands;
            }
        }

        List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel("Add Category", new RelayCommand(param => this.Categories())),
                new CommandViewModel("MoneyEntry",   new RelayCommand(param => this.MoneyEntry())),
                new CommandViewModel("Reconciliation", new RelayCommand(param => this.Reconciliation())),
                new CommandViewModel("Search",  new RelayCommand(param => this.Query())),
                new CommandViewModel("Charting",  new RelayCommand(param => this.Chart())),
            };
        }

        public ICommand ExitCommand
        {
            get
            {
                if (_Exit == null)
                {
                    _Exit = new RelayCommand(param => this.Exit());
                }
                return _Exit;
            }
        }

        public ICommand OpenLocationCommand
        {
            get
            {
                if (_OpenLoc == null)
                {
                    _OpenLoc = new RelayCommand(param => this.OpenBackupLocation());
                }
                return _OpenLoc;
            }
        }

        public ICommand BackupDBCommand
        {
            get
            {
                if (_BackUp == null)
                {
                    _BackUp = new RelayCommand(param => this.BackUpDB(false));
                }
                return _BackUp;
            }
        }

        public ICommand RestoreDBCommand
        {
            get
            {
                if (_Restore == null)
                {
                    _Restore = new RelayCommand(param => this.RestoreDB());
                }
                return _Restore;
            }
        }

        #endregion // Commands

        #region Workspaces

        /// <summary>
        /// Returns the collection of available workspaces to display.
        /// A 'workspace' is a ViewModel that can request to be closed.
        /// </summary>
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += this.OnWorkspacesChanged;
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
            this.Workspaces.Remove(workspace);
        }

        #endregion // Workspaces

        #region Private Helpers

        int ConvertToNumber(string s)
        {
            try
            {
                return Convert.ToInt32(s);
            }
            catch (FormatException)
            {
                return 0;
            }
        }

        void SetCurrentUser()
        {
            int currentID = ConvertToNumber(CurrentUser);

            _currentPerson = ee.tePerson
                               .Where(i => i.PersonID == currentID)
                               .Select(p => new Person
                                                {
                                                    PersonId = p.PersonID,
                                                    FirstName = p.FirstName
                                                }).FirstOrDefault();
        }

        private void MoneyEntry()
        {
            SetCurrentUser();
            MoneyEntryViewModel money = new MoneyEntryViewModel(_currentPerson);
            this.Workspaces.Add(money);
            this.SetActiveWorkspace(money);
        }

        private void Query()
        {
            SetCurrentUser();
            QueryViewModel query = new QueryViewModel(_currentPerson);
            this.Workspaces.Add(query);
            this.SetActiveWorkspace(query);
        }

        private void Reconciliation()
        {
            SetCurrentUser();
            ReconcilationViewModel reconcile = new ReconcilationViewModel(_currentPerson);
            this.Workspaces.Add(reconcile);
            this.SetActiveWorkspace(reconcile);
        }

        private void Categories()
        {
            SetCurrentUser();
            CategoryViewModel category = new CategoryViewModel(_currentPerson);
            this.Workspaces.Add(category);
            this.SetActiveWorkspace(category);
        }

        private void Chart()
        {
          SetCurrentUser();
          ChartViewModel chart = new ChartViewModel(_currentPerson);
          this.Workspaces.Add(chart);
          this.SetActiveWorkspace(chart);
        }


    void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
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

        #endregion // Private Helpers
    }
}