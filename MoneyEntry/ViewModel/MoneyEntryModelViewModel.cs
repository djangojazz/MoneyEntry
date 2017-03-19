using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows;
using MoneyEntry.Model;

namespace MoneyEntry.ViewModel
{
    public class MoneyEntryModelViewModel :  ViewModelBase
    {
        ExpensesEntities ee = new ExpensesEntities();

        private readonly MoneyEntryModel _MoneyEntry;
        //private ObservableCollection<Type> _types;


        public MoneyEntryModelViewModel(MoneyEntryModel aMoneyEntryModel)
        {
            _MoneyEntry = aMoneyEntryModel;
        }

        public int TransactionId
        {
            get { return _MoneyEntry.TransactionId;  }
        }

        public int PersonId 
        {
            get { return _MoneyEntry.PersonId; }
            set
            {
                _MoneyEntry.PersonId = value;

                OnPropertyChanged("PersonId");
            }
        }

        public string Name
        {
            get { return _MoneyEntry.Name; }
            set
            {
                _MoneyEntry.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public decimal Amount
        {
            get { return _MoneyEntry.Amount; }
            set
            {
                if ( value == _MoneyEntry.Amount)
                    return;
                else
                {
                    using (var transaction = new TransactionScope())
                    {
                        ee.spMoneyUpdate(
                            _MoneyEntry.TransactionId,
                            value,
                            _MoneyEntry.TransactionDesc,
                            _MoneyEntry.TypeName,
                            _MoneyEntry.CategoryName,
                            _MoneyEntry.CreatedDate,
                            _MoneyEntry.Reconciled,
                            _MoneyEntry.Name);

                        ee.spUpdateTotals();

                        transaction.Complete();
                    }

                }

                _MoneyEntry.Amount = value;

                OnPropertyChanged("Amount");
            }
        }

        public string TransactionDesc
        {
            get { return _MoneyEntry.TransactionDesc; }
            set
            {
                if (string.Compare(value, _MoneyEntry.TransactionDesc, StringComparison.CurrentCultureIgnoreCase) == 0)
                    return;
                else
                {
                    using (var transaction = new TransactionScope())
                    {
                        ee.spMoneyUpdate(
                            _MoneyEntry.TransactionId,
                            _MoneyEntry.Amount,
                            value,
                            _MoneyEntry.TypeName,
                            _MoneyEntry.CategoryName,
                            _MoneyEntry.CreatedDate,
                            _MoneyEntry.Reconciled,
                            _MoneyEntry.Name);

                        transaction.Complete();
                    }
                }

                _MoneyEntry.TransactionDesc = value;

                OnPropertyChanged("TransactionDesc");
            }
        }

        public string Type
        {
            get { return _MoneyEntry.TypeName; }
            set
            {
                if (string.Compare(value, _MoneyEntry.TypeName, StringComparison.CurrentCultureIgnoreCase) == 0)
                    return;
                else
                {
                    if (_MoneyEntry.Ts.Contains(value))
                    {
                        using (var transaction = new TransactionScope())
                        {
                            ee.spMoneyUpdate(
                                _MoneyEntry.TransactionId,
                                _MoneyEntry.Amount,
                                _MoneyEntry.TransactionDesc,
                                value,
                                _MoneyEntry.CategoryName,
                                _MoneyEntry.CreatedDate,
                                _MoneyEntry.Reconciled,
                                _MoneyEntry.Name);

                            ee.spUpdateTotals();

                            transaction.Complete();
                        }
                    }
                    else
                    {
                        string s = "";
                        _MoneyEntry.Ts.ToList().ForEach(n => s += n + "\n");

                        MessageBox.Show("You may only choose: \n\n" + s + "\n\n Resetting value to original reference");

                        OnPropertyChanged("Type");

                        return;
                    }
                }

                _MoneyEntry.TypeName = value;

                OnPropertyChanged("Type");
            }
        }

        public int CategoryId
        {
            get { return _MoneyEntry.CategoryId; }
            set
            {
                _MoneyEntry.CategoryId = value;
                OnPropertyChanged("CategoryId");
            }
        }


        public string Category
        {
            get { return _MoneyEntry.CategoryName; }
            set
            {
                if (string.Compare(value, _MoneyEntry.CategoryName, StringComparison.CurrentCultureIgnoreCase) == 0)
                    return;
                else
                {
                    if (_MoneyEntry.Cs.Contains(value))
                    {
                        using (var transaction = new TransactionScope())
                        {
                            ee.spMoneyUpdate(
                                _MoneyEntry.TransactionId,
                                _MoneyEntry.Amount,
                                _MoneyEntry.TransactionDesc,
                                _MoneyEntry.TypeName,
                                value,
                                _MoneyEntry.CreatedDate,
                                _MoneyEntry.Reconciled,
                                _MoneyEntry.Name);

                            transaction.Complete();
                        }
                    }
                    else
                    {
                        string s = "";
                        _MoneyEntry.Cs.ToList().ForEach(n => s += n + "\n");

                        MessageBox.Show("You may only choose: \n\n" + s + "\n\n Resetting value to original reference");

                        OnPropertyChanged("Type");

                        return;
                    }
                }

                _MoneyEntry.CategoryName = value;

                OnPropertyChanged("Category");
            }
        }

        public DateTime? CreatedDate
        {
            get { return _MoneyEntry.CreatedDate; }
            set
            {
                if ( value == _MoneyEntry.CreatedDate)
                    return;
                else
                {
                    using (var transaction = new TransactionScope())
                    {
                        ee.spMoneyUpdate(
                            _MoneyEntry.TransactionId,
                            _MoneyEntry.Amount,
                            _MoneyEntry.TransactionDesc,
                            _MoneyEntry.TypeName,
                            _MoneyEntry.CategoryName,
                            value,
                            _MoneyEntry.Reconciled,
                            _MoneyEntry.Name);

                        transaction.Complete();
                    }
                }

                _MoneyEntry.CreatedDate = value;

                OnPropertyChanged("CreatedDate");
            }
        }

        public decimal? RunningTotal
        {
            get { return _MoneyEntry.RunningTotal; }
            set
            {
                _MoneyEntry.RunningTotal = value;

                OnPropertyChanged("RunningTotal");
            }
        }

        public bool? Reconciled
        {
            get { return _MoneyEntry.Reconciled; }
            set
            {
                if (value == _MoneyEntry.Reconciled)
                    return;
                else
                {
                    using (var transaction = new TransactionScope())
                    {
                        ee.spMoneyUpdate(
                            _MoneyEntry.TransactionId,
                            _MoneyEntry.Amount,
                            _MoneyEntry.TransactionDesc,
                            _MoneyEntry.TypeName,
                            _MoneyEntry.CategoryName,
                            _MoneyEntry.CreatedDate,
                            value,
                            _MoneyEntry.Name);

                        transaction.Complete();
                    }
                }

                _MoneyEntry.Reconciled = value;

                OnPropertyChanged("CreatedDate");
            }
        }

    }
}
