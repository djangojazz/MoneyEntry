using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using Prism.Interactivity;

namespace Extensions.ListView
{

  public sealed class EscapeReleaseBehavior : CommandBehaviorBase<System.Windows.Controls.ListView>
  {             
    public ListViewItem ClickedItem;
    public EscapeReleaseBehavior(System.Windows.Controls.ListView Element) : base(Element)
    {

      Element.KeyUp += Element_KeyReleased;
    }

    private void Element_KeyReleased(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Escape)
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
         
  public sealed class EscapeReleased
  {        
    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(EscapeReleased), new PropertyMetadata(OnSetCommandCallback));
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(EscapeReleased), new PropertyMetadata(OnSetCommandParameterCallback));

    public static readonly DependencyProperty EscapeReleaseBehaviorProperty = DependencyProperty.RegisterAttached("EscapeReleaseBehavior", typeof(EscapeReleaseBehavior), typeof(EscapeReleased), null);
 
    private static EscapeReleaseBehavior GetOrCreateBehavior(System.Windows.Controls.ListView element)
    {
      EscapeReleaseBehavior behavior = element.GetValue(EscapeReleaseBehaviorProperty) as EscapeReleaseBehavior;
      if (behavior == null)
      {
        behavior = new EscapeReleaseBehavior(element);
        element.SetValue(EscapeReleaseBehaviorProperty, behavior);
      }
      return behavior;
    }

    private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      System.Windows.Controls.ListView element = dependencyObject as System.Windows.Controls.ListView;
      if (element != null)
      {
        EscapeReleaseBehavior behavior = GetOrCreateBehavior(element);
        behavior.Command = e.NewValue as ICommand;
      }
    }

    private static void OnSetCommandParameterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      System.Windows.Controls.ListView element = dependencyObject as System.Windows.Controls.ListView;
      if (element != null)
      {
        EscapeReleaseBehavior behavior = GetOrCreateBehavior(element);
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

    public static EscapeReleaseBehavior GetEscapeReleaseBehavior(DependencyObject obj)
    {
      return (EscapeReleaseBehavior)obj.GetValue(EscapeReleaseBehaviorProperty);
    }

    public static void SetEscapeReleaseBehavior(DependencyObject obj, EscapeReleaseBehavior value)
    {
      obj.SetValue(EscapeReleaseBehaviorProperty, value);
    }

  }

}