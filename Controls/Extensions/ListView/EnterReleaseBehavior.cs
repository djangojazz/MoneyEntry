using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using Prism.Interactivity;

namespace Extensions.ListView
{

  public sealed class EnterReleaseBehavior : CommandBehaviorBase<System.Windows.Controls.ListView>
  {  
    public ListViewItem ClickedItem;
    public EnterReleaseBehavior(System.Windows.Controls.ListView Element) : base(Element)
    {

      Element.KeyUp += Element_KeyReleased;
    }

    private void Element_KeyReleased(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        DependencyObject dep = (DependencyObject)e.OriginalSource;
        while (dep != null && dep.GetType() != typeof(ListViewItem))
        {
          dep = VisualTreeHelper.GetParent(dep);
        }
        ClickedItem = (ListViewItem)dep;

        base.ExecuteCommand(null);
      }
    }  
  }      

  public sealed class EnterReleased
  {
    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(EnterReleased), new PropertyMetadata(OnSetCommandCallback));
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(EnterReleased), new PropertyMetadata(OnSetCommandParameterCallback));

    public static readonly DependencyProperty EnterReleaseBehaviorProperty = DependencyProperty.RegisterAttached("EnterReleaseBehavior", typeof(EnterReleaseBehavior), typeof(EnterReleased), null);
                 
    private static EnterReleaseBehavior GetOrCreateBehavior(System.Windows.Controls.ListView element)
    {
      EnterReleaseBehavior behavior = element.GetValue(EnterReleaseBehaviorProperty) as EnterReleaseBehavior;
      if (behavior == null)
      {
        behavior = new EnterReleaseBehavior(element);
        element.SetValue(EnterReleaseBehaviorProperty, behavior);
      }
      return behavior;
    }

    private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      System.Windows.Controls.ListView element = dependencyObject as System.Windows.Controls.ListView;
      if (element != null)
      {
        EnterReleaseBehavior behavior = GetOrCreateBehavior(element);
        behavior.Command = e.NewValue as ICommand;
      }
    }

    private static void OnSetCommandParameterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      System.Windows.Controls.ListView element = dependencyObject as System.Windows.Controls.ListView;
      if (element != null)
      {
        EnterReleaseBehavior behavior = GetOrCreateBehavior(element);
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

    public static EnterReleaseBehavior GetEnterReleaseBehavior(DependencyObject obj)
    {
      return (EnterReleaseBehavior)obj.GetValue(EnterReleaseBehaviorProperty);
    }

    public static void SetEnterReleaseBehavior(DependencyObject obj, EnterReleaseBehavior value)
    {
      obj.SetValue(EnterReleaseBehaviorProperty, value);
    }  
  }    
}           