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
using MessageBox = System.Windows.Forms.MessageBox;


namespace MoneyEntry.ViewModel
{
    class CategoryViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        Person _Person;
        ObservableCollection<tdCategory> _categories;

        RelayCommand _Addcommand;

        private string _Desc;
        private string _CurrentCategory;

        ExpensesEntities ee = new ExpensesEntities();

        public CategoryViewModel(Person person)
        {
            _Person = person;
        }

        public string Desc
        {
            get { return _Desc; }
            set
            {
                _Desc = value;

                OnPropertyChanged("Desc");
            }
        }


        public ObservableCollection<tdCategory> Categories
        {
            get
            {
                if (_categories == null)
                {
                    List<MoneyEntry.tdCategory> cats = this.GetCats();
                    
                    _categories = new ObservableCollection<tdCategory>(cats);
                }

                return _categories;
            }
            set
            {
                _categories = value;

                OnPropertyChanged("Categories");
            }
        }

        List<tdCategory> GetCats()
        {
            return ee.tdCategories.ToList();
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

        public override string DisplayName
        {
            get { return  "Add Category (" + _Person.FirstName + ")"; }
        }

        public ICommand AddCommand
        {
            get
            {
                if (_Addcommand == null)
                {
                    _Addcommand = new RelayCommand(param => this.Add());
                }
                return _Addcommand;
            }
        }

        private void Add()
        {
            tdCategory newcat = new tdCategory {Description = _Desc};

            ee.tdCategories.AddObject(newcat);
            ee.SaveChanges();
            ee.AcceptAllChanges();

            GetCats();
            
            MessageBox.Show("Added " + _Desc);
        }

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
