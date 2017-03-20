namespace Controls.Types
{

  using System;
  using System.Collections;
  using System.Collections.ObjectModel;
  using System.Collections.Specialized;
  using System.ComponentModel;

  public class ObservableCollectionContentNotifying<T> : ObservableCollection<T>, IDisposable
  {                
    public event OnCollectionItemChangedEventHandler OnCollectionItemChanged;
    public delegate void OnCollectionItemChangedEventHandler(object sender, ObservableCollectionContentChangedArgs e);
                   
    private bool _disposedValue;
    private bool _SuspendNotification;
                
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
      if (!SuspendNotification)
      {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
          RegisterPropertyChangedItems(e.NewItems);
        }
        else if (e.Action == NotifyCollectionChangedAction.Remove)
        {
          UnregisterPropertyChangedItems(e.OldItems);
        }
        else if (e.Action == NotifyCollectionChangedAction.Replace)
        {
          UnregisterPropertyChangedItems(e.OldItems);
          RegisterPropertyChangedItems(e.NewItems);
        }        
        base.OnCollectionChanged(e);
      }
    }

    public bool SuspendNotification
    {
      get { return _SuspendNotification; }
      set
      {
        dynamic Notify = _SuspendNotification & !value;
        _SuspendNotification = value;
        if (Notify)
          OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      }
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      if (!_SuspendNotification)
        base.OnPropertyChanged(e);
    }

    protected override void ClearItems()
    {
      UnregisterPropertyChangedItems(this);
      base.ClearItems();
    }

    private void RegisterPropertyChangedItems(IList items)
    {
      foreach (INotifyPropertyChanged obj in items)
      {
        obj.PropertyChanged += RaiseNotification;
      }
    }

    private void UnregisterPropertyChangedItems(IList items)
    {
      foreach (INotifyPropertyChanged obj in items)
      {
        obj.PropertyChanged -= RaiseNotification;
      }
    }

    private void RaiseNotification(object sender, PropertyChangedEventArgs e)
    {
      if (OnCollectionItemChanged != null)
      {
        OnCollectionItemChanged(this, new ObservableCollectionContentChangedArgs
        {
          ElementChanged = sender,
          PropertyChanged = e
        });
      }
    }

    public void Dispose()
    {
      Dispose(true);
    }
    protected virtual void Dispose(bool disposing)
    {
      if (!_disposedValue)
      {
        if (disposing)
        {
          UnregisterPropertyChangedItems(this);
        }
      }
      _disposedValue = true;
    }     
  }       

  public class ObservableCollectionContentChangedArgs : EventArgs
  {       
    public object ElementChanged { get; set; }
    public PropertyChangedEventArgs PropertyChanged { get; set; } 
  }                                                               

}
