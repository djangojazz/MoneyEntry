using System;
using MoneyEntry.Model;
using System.Windows;
using MoneyEntry.ViewModel;
using System.ComponentModel;

namespace MoneyEntry.Model
{
  public class MoneyEntryObservable : INotifyPropertyChanged
  {
    private int _personId;
    private int _transactionId;
    private string _description;
    private DateTime? _createdDate;
    private byte _typeId;
    private byte _categoryId;
    private decimal _amount;
    private decimal? _runningTotal;
    private bool? _reconciled;

   
    public MoneyEntryObservable(int personId, string description, DateTime createdDate, byte typeId, byte categoryId, decimal amount)
    {
      PersonId = personId;
      Description = description;
      CreatedDate = createdDate;
      TypeId = typeId;
      CategoryId = categoryId;
      Amount = amount;
      CreatedDate = createdDate;
    }

    public MoneyEntryObservable(int personId, int transactionId, string description, DateTime? createdDate, byte typeId, byte categoryId, decimal amount, decimal? runningTotal, bool? reconciled)
    {
      PersonId = personId;
      TransactionId = transactionId;
      Description = description;
      CreatedDate = createdDate;
      TypeId = typeId;
      CategoryId = categoryId;
      Amount = amount;
      CreatedDate = createdDate;
      RunningTotal = runningTotal;
      Reconciled = reconciled;
    }

    
    public int PersonId
    {
      get => _personId;
      set
      {
        _personId = value;
        OnPropertyChanged(nameof(PersonId));
      }
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

    public string Description
    {
      get => _description;
      set
      {
        if (value == _description) { return; }
        _description = value;
        OnPropertyChanged(nameof(Description));
      }
    }

    public DateTime? CreatedDate
    {
      get => _createdDate;
      set
      {
        if (value == _createdDate) { return; }
        _createdDate = value;
        OnPropertyChanged(nameof(CreatedDate));
      }
    }

    public byte TypeId
    {
      get => _typeId;
      set
      {
        if (value == _typeId) { return; }
        _typeId = value;
        OnPropertyChanged(nameof(TypeId));
      }
    }

    public byte CategoryId
    {
      get => _categoryId;
      set
      {
        if (value == _categoryId) { return; }
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
      PropertyChangedEventHandler handler = this.PropertyChanged;
      if (handler != null)
      {
        var e = new PropertyChangedEventArgs(propertyName);
        handler(this, e);
      }
    }
  }
}
