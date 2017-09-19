using System;
using MoneyEntry.Model;
using System.Windows;

namespace MoneyEntry.ViewModel
{
  public class MoneyEntryModelViewModel : WorkspaceViewModel
  {
    private int _transactionId;
    private byte _typeId;
    private readonly TransactionView _viewTransaction;

    public MoneyEntryModelViewModel(TransactionView transaction)
    {
      _viewTransaction = transaction;
      TransactionId = _viewTransaction.TransactionID;
      TypeId = transaction.Type.TypeId;
    }
    
    public MoneyEntryModelViewModel(int transactionId, string transactionDesc, byte typeId, int categoryId, decimal amount, DateTime? createdDate, decimal? runningTotal, bool? reconciled)
    {
      TransactionId = transactionId;
      TransactionDesc = transactionDesc;
      TypeId = typeId;
      CategoryId = categoryId;
      //Amount = amount;
      //CreatedDate = createdDate;
      //RunningTotal = runningTotal;
      //Reconciled = reconciled;
    }
    
    public int TransactionId
    {
      get => _transactionId;
      set
      {
        _transactionId = value;
        OnPropertyChanged(nameof(TransactionId));
      }
    }
    
    public string TransactionDesc
    {
      get => _viewTransaction.TransactionDesc;
      set
      {
        if (string.Compare(value, _viewTransaction.TransactionDesc, StringComparison.CurrentCultureIgnoreCase) == 0)
          return;
        else
        {
          _viewTransaction.TransactionDesc = value;
          Repository.InsertOrUpdateTransaction(_viewTransaction);
          OnPropertyChanged("TransactionDesc");
        }
      }
    }
    
    //public Person Person
    //{
    //  get => _viewTransaction.Person;
    //  set
    //  {
    //    _viewTransaction.Person = value;
    //    OnPropertyChanged("Person");
    //  }
    //}
    
    public byte TypeId
    {
      get => _typeId;
      set
      {
        if (_typeId != 0) { MessageBox.Show($"Change to {value}"); }
        _typeId = value;
        OnPropertyChanged("TypeId");
      }
    }

    public TypeTran Type
    {
      get => _viewTransaction.Type;
      set
      {
        _viewTransaction.Type = value;
        Repository.InsertOrUpdateTransaction(_viewTransaction);
        OnPropertyChanged("Type");
      }
    }

    private int _categoryId;

    public int CategoryId
    {
      get => _categoryId;
      set
      {
        _categoryId = value;
        OnPropertyChanged(nameof(CategoryId));
      }
    }
    
    public Category Category
    {
      get => _viewTransaction.Category;
      set
      {
        _viewTransaction.Category = value;
        Repository.InsertOrUpdateTransaction(_viewTransaction);
        OnPropertyChanged("Category");
      }
    }

    public decimal Amount
    {
      get => _viewTransaction.Amount;
      set
      {
        if (value == _viewTransaction.Amount) { return; }
        else
        {
          _viewTransaction.Amount = value;
          Repository.InsertOrUpdateTransaction(_viewTransaction);
          OnPropertyChanged("Amount");
        }
      }
    }

    public DateTime? CreatedDate
    {
      get { return _viewTransaction.CreatedDate; }
      set
      {
        if (value == _viewTransaction.CreatedDate) { return; }
        else
        {
          _viewTransaction.CreatedDate = value;
          Repository.InsertOrUpdateTransaction(_viewTransaction);
          OnPropertyChanged("CreatedDate");
        }
      }
    }

    public decimal? RunningTotal
    {
      get { return _viewTransaction.RunningTotal; }
      set
      {
        _viewTransaction.RunningTotal = value;
        OnPropertyChanged("RunningTotal");
      }
    }

    public bool? Reconciled
    {
      get { return _viewTransaction.reconciled; }
      set
      {
        if (value == _viewTransaction.reconciled) { return; }
        else
        {
          _viewTransaction.reconciled = value;
          Repository.InsertOrUpdateTransaction(_viewTransaction);
          OnPropertyChanged("Reconciled");
        }
      }
    }
  }
}
