using MoneyEntry.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace MoneyEntry.ViewModel
{
  public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
  {
    protected ViewModelBase() { }
    public virtual string DisplayName { get; protected set; }
    
    #region Debugging Aides
    
    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public void VerifyPropertyName(string propertyName)
    {
      if (TypeDescriptor.GetProperties(this)[propertyName] == null)
      {
        string msg = "Invalid property name: " + propertyName;
        if (ThrowOnInvalidPropertyName)
          throw new Exception(msg);
        else
          Debug.Fail(msg);
      }
    }

    protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

    #endregion // Debugging Aides
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected virtual void OnPropertyChanged(string propertyName)
    {
      VerifyPropertyName(propertyName);

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