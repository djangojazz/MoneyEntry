using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Controls.Types
{
  public class ItemObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
  {
    public event EventHandler<ItemPropertyChangedEventArgs<T>> ItemPropertyChanged;


    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
      base.OnCollectionChanged(args);
      if (args.NewItems != null)
        foreach (INotifyPropertyChanged item in args.NewItems)
          item.PropertyChanged += item_PropertyChanged;

      if (args.OldItems != null)
        foreach (INotifyPropertyChanged item in args.OldItems)
          item.PropertyChanged -= item_PropertyChanged;
    }

    private void OnItemPropertyChanged(T sender, PropertyChangedEventArgs args)
    {
      if (ItemPropertyChanged != null)
        ItemPropertyChanged(this, new ItemPropertyChangedEventArgs<T>(sender, args.PropertyName));
    }

    private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      OnItemPropertyChanged((T)sender, e);
    }

    public sealed class ItemPropertyChangedEventArgs<Titem> : EventArgs
    {
      private readonly Titem _item;
      private readonly string _propertyName;

      public ItemPropertyChangedEventArgs(Titem item, string propertyName)
      {
        _item = item;
        _propertyName = propertyName;
      }

      public Titem Item
      {
        get { return _item; }
      }

      public string PropertyName
      {
        get { return _propertyName; }
      }
    }
  }
}
