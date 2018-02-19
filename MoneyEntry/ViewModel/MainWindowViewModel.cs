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
using Microsoft.Win32;
using System.Data;

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
        OpenFileDialog _openFileDialog = new OpenFileDialog();

        private string _BackupLocation;
        private string _InitialBackupLocation;

        public MainWindowViewModel()
        {
            base.DisplayName = Strings.MainWindowViewModel_DisplayName;
            People = new ObservableCollection<Person>(Repository.GetPeople());

            _currentUser = _people.FirstOrDefault(x => x.FirstName == "Shared");

            _BackupLocation = ConfigurationManager.AppSettings["DatabaseBackupsLocation"]; // Start the initial backup

            if (!Directory.Exists(_BackupLocation)) { Directory.CreateDirectory(_BackupLocation); }

            DateTime dt = DateTime.Now;
            _InitialBackupLocation = _BackupLocation + "\\ExpensesStart_" + dt.Year + "-" + dt.Month + "-" + dt.Day + ".bak";

            //Not needed at this point.
            //if (!File.Exists(_InitialBackupLocation)) { BackUpDB(true); }  // initial backup on startup
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
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }

        public ReadOnlyCollection<CommandViewModel> Commands { get => _commands ?? (_commands = new ReadOnlyCollection<CommandViewModel>(CreateCommands())); }
        public ICommand ExitCommand { get => _Exit ?? (_Exit = new RelayCommand(param => Exit())); }
        public ICommand OpenLocationCommand { get => _OpenLoc ?? (_OpenLoc = new RelayCommand(param => OpenBackupLocation())); }
        public ICommand BackupDBCommand { get => _BackUp ?? (_BackUp = new RelayCommand(param => this.BackUpDB(false))); }
        public ICommand RestoreDBCommand { get => _Restore ?? (_Restore = new RelayCommand(param => this.RestoreDB())); }


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
            if (Directory.Exists(_BackupLocation))
            {
                Process.Start(_BackupLocation);
            }
            else
            {
                MessageBox.Show("Directory does not exist");
            }
        }

        void Exit()
        {
            Application.Current.Shutdown();

        }

        void BackUpDB(bool aStartup)
        {
            try
            {
                using (var sqlTalker = new SQLTalker())
                {
                    var result = sqlTalker.BackupDB(_BackupLocation, aStartup);
                    if (!aStartup) { MessageBox.Show(result.Value, result.Key ? "Success" : "Failure"); }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Process failed...", "Backup");
            }
        }

        void RestoreDB()
        {
            try
            {
                switch (
                MessageBox.Show(
                    "Kill existing connection to Expenses and restore?\n\nWARNING IF YOU REVERT, THE APPLICATION\nNEEDS TO CLOSE BEFORE YOU MAY RESTART.",
                    "Restore", MessageBoxButton.YesNo, MessageBoxImage.Exclamation))
                {
                    case MessageBoxResult.Yes:
                        {
                            _openFileDialog.InitialDirectory = "C:\\SQLServer\\Backups\\";
                            _openFileDialog.Filter = "Backup Files (.bak)|*.bak|All Files (*.*)|*.*";

                            bool? fileResult = _openFileDialog.ShowDialog();
                            if (fileResult == true)
                            {
                                var restoreloc = _openFileDialog.FileName;

                                switch (MessageBox.Show("You wish to restore backup: " + restoreloc + "? ", "Restore", MessageBoxButton.OKCancel, MessageBoxImage.Question))
                                {
                                    case MessageBoxResult.OK:
                                        using (var sqlTalker = new SQLTalker())
                                        {
                                            sqlTalker.KillConnectionsToDatabase(); 
                                            var result = sqlTalker.RestoreDB(restoreloc);
                                            MessageBox.Show($"{result.Value}{Environment.NewLine}{Environment.NewLine}Closing Application", result.Key ? "Success" : "Failure");
                                        }
                                        Application.Current.Shutdown();
                                        break;
                                    case MessageBoxResult.Cancel:
                                        MessageBox.Show("Operation cancelled.", "Restore");
                                        break;
                                }
                            }
                            else { MessageBox.Show("Could not locate file to restore", "NOT FOUND"); }
                        }
                        break;
                    case MessageBoxResult.No:
                        MessageBox.Show("Operation cancelled.", "Restore");
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Process failed...", "Restore");
            }
        }

        #endregion
    }
}