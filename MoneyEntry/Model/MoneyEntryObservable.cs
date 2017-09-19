using System;
using MoneyEntry.Model;
using System.Windows;
using MoneyEntry.ViewModel;
using System.ComponentModel;

namespace MoneyEntry.Model
{
  public class MoneyEntryObservable : INotifyPropertyChanged
  {
    private int _transactionId;
    private string _transactionDesc;
    private DateTime? _createdDate;
    private byte _typeId;
    private int _categoryId;
    private decimal _amount;
    private decimal? _runningTotal;
    private bool? _reconciled;

    public MoneyEntryObservable(int transactionId, string transactionDesc, DateTime? createdDate, byte typeId, int categoryId, decimal amount, decimal? runningTotal, bool? reconciled)
    {
      TransactionId = transactionId;
      TransactionDesc = transactionDesc;
      CreatedDate = createdDate;
      TypeId = typeId;
      CategoryId = categoryId;
      Amount = amount;
      CreatedDate = createdDate;
      RunningTotal = runningTotal;
      Reconciled = reconciled;
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
        OnPropertyChanged(nameof(TransactionDesc));
      }
    }

    public DateTime? CreatedDate
    {
      get => _createdDate;
      set
      {
        _createdDate = value;
        OnPropertyChanged(nameof(CreatedDate));
      }
    }

    public byte TypeId
    {
      get => _typeId;
      set
      {
        if (_typeId != 0) { MessageBox.Show($"Change to {value}"); }
        _typeId = value;
        OnPropertyChanged(nameof(TypeId));
      }
    }

    public int CategoryId
    {
      get => _categoryId;
      set
      {
        _categoryId = value;
        OnPropertyChanged(nameof(CategoryId));
      }
    }

    public decimal Amount
    {
      get => _amount;
      set
      {
        if (value == _amount) { return; }
        else
        {
          _amount = value;
          //Repository.InsertOrUpdateTransaction(_viewTransaction);
          OnPropertyChanged(nameof(Amount));
        }
      }
    }

    public decimal? RunningTotal
    {
      get => _runningTotal;
      set
      {
        _runningTotal = value;
        OnPropertyChanged(nameof(RunningTotal));
      }
    }

    public bool? Reconciled
    {
      get => _reconciled;
      set
      {
        if (value == _reconciled) { return; }
        else
        {
          _reconciled = value;
          OnPropertyChanged(nameof(Reconciled));
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
      //VerifyPropertyName(propertyName);

      PropertyChangedEventHandler handler = this.PropertyChanged;
      if (handler != null)
      {
        var e = new PropertyChangedEventArgs(propertyName);
        handler(this, e);
      }
    }
  }
}
