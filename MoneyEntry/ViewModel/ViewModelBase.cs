using System;
using System.ComponentModel;
using System.Diagnostics;

namespace MoneyEntry.ViewModel
{
  public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
  {
    protected ViewModelBase() { }
    public virtual string DisplayName { get; protected set; }
    
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
    
    #region IDisposable Members

    public void Dispose() => OnDispose();
    protected virtual void OnDispose() {}

#if DEBUG
    ~ViewModelBase()
    {
      string msg = string.Format("{0} ({1}) ({2}) Finalized", this.GetType().Name, this.DisplayName, this.GetHashCode());
      Debug.WriteLine(msg);
    }
#endif

    #endregion // IDisposable Members

  }
}