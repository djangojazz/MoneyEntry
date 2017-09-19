using System;
using MoneyEntry.Model;
using System.Windows;

namespace MoneyEntry.ViewModel
{
  public class MoneyEntryObservable : ViewModelBase
  {
    private int _transactionId;
    private string _transactionDesc;
    private DateTime? _createdDate;
    private int _typeId;
    
    
    public MoneyEntryObservable(int transactionId, string transactionDesc, DateTime? createdDate, int typeId)
      //, int categoryId, decimal amount, DateTime? createdDate, decimal? runningTotal, bool? reconciled)
    {
      TransactionId = transactionId;
      TransactionDesc = transactionDesc;
      CreatedDate = createdDate;
      TypeId = typeId;
      //CategoryId = categoryId;
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
      get => _transactionDesc;
      set
      {
        _transactionDesc = value;
        OnPropertyChanged("TransactionDesc");
      }
    }

    public DateTime? CreatedDate
    {
      get => _createdDate;
      set
      {
        _createdDate = value;
        OnPropertyChanged("CreatedDate");
       }
    }

    public int TypeId
    {
      get => _typeId;
      set
      {
        if (_typeId != 0) { MessageBox.Show($"Change to {value}"); }
        _typeId = value;
        OnPropertyChanged("TypeId");
      }
    }
    
  }
}
