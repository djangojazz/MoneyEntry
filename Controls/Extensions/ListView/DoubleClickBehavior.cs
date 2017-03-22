using System.Windows.Input;
using System.Windows;
using Prism.Interactivity;   
using System.Windows.Controls;

namespace Extensions.ListView
{

  public sealed class DoubleClickBehavior : CommandBehaviorBase<System.Windows.Controls.ListView>
  {             
    public ListViewItem ClickedItem;
    public DoubleClickBehavior(System.Windows.Controls.ListView Element) : base(Element)
    {

      Element.MouseDoubleClick += Element_DoubleClicked;
    }

    public void Element_DoubleClicked(object Sender, MouseButtonEventArgs e)
    {
      DependencyObject dep = (DependencyObject)e.OriginalSource;
      while (dep != null && dep.GetType() != typeof(ListViewItem))
      {
        dep = System.Windows.Media.VisualTreeHelper.GetParent(dep);
      }
      ClickedItem = dep as ListViewItem;
                                                                         
      if ((ClickedItem != null))
        base.ExecuteCommand(null);
    }                                 
  }


  public sealed class DoubleClick
  {
    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(DoubleClick), new PropertyMetadata(OnSetCommandCallback));
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(DoubleClick), new PropertyMetadata(OnSetCommandParameterCallback));

    public static readonly DependencyProperty DoubleClickBehaviorProperty = DependencyProperty.RegisterAttached("SelectedIndexChangedBehavior", typeof(DoubleClickBehavior), typeof(DoubleClick), null);
              
    private static DoubleClickBehavior GetOrCreateBehavior(System.Windows.Controls.ListView element)
    {
      DoubleClickBehavior behavior = element.GetValue(DoubleClickBehaviorProperty) as DoubleClickBehavior;
      if (behavior == null)
      {
        behavior = new DoubleClickBehavior(element);
        element.SetValue(DoubleClickBehaviorProperty, behavior);
      }
      return behavior;
    }

    private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      System.Windows.Controls.ListView element = dependencyObject as System.Windows.Controls.ListView;
      if (element != null)
      {
        DoubleClickBehavior behavior = GetOrCreateBehavior(element);
        behavior.Command = e.NewValue as ICommand;
      }
    }

    private static void OnSetCommandParameterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      System.Windows.Controls.ListView element = dependencyObject as System.Windows.Controls.ListView;
      if (element != null)
      {
        DoubleClickBehavior behavior = GetOrCreateBehavior(element);
        behavior.CommandParameter = e.NewValue;
      }
    }

    public static ICommand GetCommand(DependencyObject obj)
    {
      return (ICommand)obj.GetValue(CommandProperty);
    }

    public static void SetCommand(DependencyObject obj, ICommand value)
    {
      obj.SetValue(CommandProperty, value);
    }

    public static object GetCommandParameter(System.Windows.Controls.ListView Element)
    {
      return Element.GetValue(CommandParameterProperty);
    }

    public static void SetCommandParameter(System.Windows.Controls.ListView Element, object parameter)
    {
      Element.SetValue(CommandParameterProperty, parameter);
    }

    public static DoubleClickBehavior GetDoubleClickBehavior(DependencyObject obj)
    {
      return (DoubleClickBehavior)obj.GetValue(DoubleClickBehaviorProperty);
    }

    public static void SetDoubleClickBehavior(DependencyObject obj, DoubleClickBehavior value)
    {
      obj.SetValue(DoubleClickBehaviorProperty, value);
    }  
  }  
}