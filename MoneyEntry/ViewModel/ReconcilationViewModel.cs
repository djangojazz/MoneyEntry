using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Transactions;
using MoneyEntry.Model;
using MoneyEntry.DataAccess;
using MoneyEntry.Properties;


namespace MoneyEntry.ViewModel
{
    class ReconcilationViewModel : WorkspaceViewModel, IDataErrorInfo
    {
          #region Fields

        Person _Person;
        ReadOnlyCollection<Categories> _categories;
        ReadOnlyCollection<Types> _types;
        ObservableCollection<MoneyEntryModelViewModel> _moneyentries;
        
        RelayCommand _SaveCommand;
        RelayCommand _RefreshCommand;

        bool _isSelected;
        string _Desc;
        decimal _MoneyAmount;
        string _CurrentCategory;
        DateTime _MoneyEntry;
        DateTime _RefreshStart;
        DateTime _RefreshEnd;

        ExpensesEntities ee = new ExpensesEntities();

        #endregion // Fields

        #region Constructor

        public ReconcilationViewModel(Person person)
        {
            if (person == null)
                throw new ArgumentNullException("person");

            _Person = person;
            CurrentType = "Debit";
            var lastreconciledate = ee.vTrans
                                    .Where(n => n.PersonID == _Person.PersonId && n.reconciled == true)
                                    .OrderByDescending(d => d.CreatedDate)
                                    .Select(d => d.CreatedDate)
                                    .FirstOrDefault();

            var lastdate = ee.vTrans
                              .Where(n => n.PersonID == _Person.PersonId)
                              .OrderByDescending(d => d.CreatedDate)
                              .Select(d => d.CreatedDate)
                              .First();

            if(lastreconciledate != null)
            {
                RefreshStart = (DateTime)lastreconciledate;
            }
            else
            {
                RefreshStart = (DateTime)lastdate;
            }

            RefreshEnd = DateTime.Now;
        }

        #endregion // Constructor

        #region MoneyEntry Properties

        public string Desc
        {
            get { return _Desc; }
            set
            {
                _Desc = value;

                OnPropertyChanged("Desc");
            }
        }

        public decimal MoneyAmount
        {
            get { return _MoneyAmount; }
            set
            {
                _MoneyAmount = value;

                OnPropertyChanged("MoneyAmount");
            }
        }

        public string CurrentCategory
        {
            get { return _CurrentCategory; }
            set
            {
                _CurrentCategory = value;

                OnPropertyChanged("CurrentCategory");
            }
        }

        public DateTime RefreshStart
        {
            get { return _RefreshStart; }
            set
            {
                _RefreshStart = value;

                OnPropertyChanged("RefreshStart");
            }
        }

        public DateTime RefreshEnd
        {
            get { return _RefreshEnd; }
            set
            {
                _RefreshEnd = value;

                OnPropertyChanged("RefreshEnd");
            }
        }

        #endregion

        #region MoneyEntry Collection


        public ObservableCollection<MoneyEntryModelViewModel> MoneyEnts
        {
            get
            {
                if (_moneyentries == null)
                {
                    List<MoneyEntryModel> mems = this.GetMoneyEnts();
                    List<MoneyEntryModelViewModel> mes = new List<MoneyEntryModelViewModel>();

                     mems.ForEach(m => mes.Add(new MoneyEntryModelViewModel(m)));

                     _moneyentries = new ObservableCollection<MoneyEntryModelViewModel>(mes);
                }

                return _moneyentries;
            }
            set
            {
                _moneyentries = value;

                OnPropertyChanged("MoneyEnts");
            }
        }

        List<MoneyEntryModel> GetMoneyEnts()
        {
            return ee.vTrans.Where(d => d.CreatedDate >= RefreshStart && d.CreatedDate <= RefreshEnd && d.PersonID == _Person.PersonId)
                .Select(m => new MoneyEntryModel()
                {
                    PersonId = m.PersonID,
                    Name = m.Name,
                    TransactionId = m.TransactionID,
                    Amount = Decimal.Round(m.Amount, 2),
                    TransactionDesc = m.TransactionDesc,
                    TypeName = m.Type,
                    CategoryName = m.Category,
                    CreatedDate = m.CreatedDate,
                    RunningTotal = Decimal.Round(m.RunningTotal.Value, 2),
                    Reconciled = m.reconciled
                }).ToList().OrderBy(d => d.CreatedDate).ToList();
        } 
        #endregion

        #region Category related
        public ReadOnlyCollection<Categories> Categories
        {
            get
            {
                if (_categories == null)
                {
                    List<Categories> cats = this.GetCategories();
                    _categories = new ReadOnlyCollection<Categories>(cats);
                }
                return _categories;
            }
        }

        List<Categories> GetCategories()
        {
            //ExpensesEntities ee = new ExpensesEntities();
            return ee.tdCategory.Select(x => new Categories()
                                         {
                                             CategoryId = x.CategoryID,
                                             CategoryName = x.Description
                                         }).ToList();
        }

        #endregion

        #region Types Related
        public ReadOnlyCollection<Types> Types
        {
            get
            {
                if (_types == null)
                {
                    List<Types> types = this.GetTypes();
                    _types = new ReadOnlyCollection<Types>(types);
                }
                return _types;
            }
        }

        List<Types> GetTypes()
        {
            return ee.tdType.Select(x => new Types()
                                              {
                                                  TypeId = x.TypeID,
                                                  TypeName = x.Description
                                              }).ToList();
        }

        public string CurrentType { get; set; } 
        #endregion

        #region Presentation Properties

        /// <summary>
        /// Gets/sets whether this customer is selected in the UI.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected)
                    return;

                _isSelected = value;

                base.OnPropertyChanged("IsSelected");
            }
        }

        public override string DisplayName
        {
            get { return Strings.ReconciliationViewModel_DisplayName + "(" + _Person.FirstName + ")"; }
        }

        public ICommand RefreshCommand
        {
            get
            {
                if (_RefreshCommand == null)
                {
                    _RefreshCommand = new RelayCommand(param => this.Refresh() );
                }
                return _RefreshCommand;
            }
        }

        #endregion // Presentation Properties

        #region Public Methods

        #endregion // Public Methods

        #region Private Helpers

        private void Refresh()
        {
            List<MoneyEntryModel> mems = this.GetMoneyEnts();
            List<MoneyEntryModelViewModel> mes = new List<MoneyEntryModelViewModel>();

            mems.ForEach(m => mes.Add(new MoneyEntryModelViewModel(m)));

            MoneyEnts = new ObservableCollection<MoneyEntryModelViewModel>(mes);
        }

        
        #endregion // Private Helpers

        #region IDataErrorInfo Members

        string IDataErrorInfo.Error
        {
            get { return (_Person as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;

                return error;
            }
        }

        #endregion // IDataErrorInfo Members
    }
}
