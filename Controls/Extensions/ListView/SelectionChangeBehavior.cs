using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using Prism.Interactivity;

namespace Extensions.ListView
{   
  public sealed class SelectionChangeBehavior : CommandBehaviorBase<System.Windows.Controls.ListView>
  {
    public ListViewItem ClickedItem;
    public SelectionChangeBehavior(System.Windows.Controls.ListView Element) : base(Element)
    {

      Element.SelectionChanged += Element_Clicked;
    }

    public void Element_Clicked(object Sender, SelectionChangedEventArgs e)
    {
      DependencyObject dep = (DependencyObject)e.OriginalSource;
      while (dep != null && dep.GetType() != typeof(ListViewItem))
      {
        dep = VisualTreeHelper.GetParent(dep);
      }
      ClickedItem = dep as ListViewItem;

      if ((ClickedItem != null))
        base.ExecuteCommand(null);
    }
  }

  public sealed class SelectionChange
  {
    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(SelectionChange), new PropertyMetadata(OnSetCommandCallback));
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(SelectionChange), new PropertyMetadata(OnSetCommandParameterCallback));

    public static readonly DependencyProperty ClickBehaviorProperty = DependencyProperty.RegisterAttached("SelectedIndexChangedBehavior", typeof(SelectionChangeBehavior), typeof(SelectionChange), null);

    private static SelectionChangeBehavior GetOrCreateBehavior(System.Windows.Controls.ListView element)
    {
      SelectionChangeBehavior behavior = element.GetValue(ClickBehaviorProperty) as SelectionChangeBehavior;
      if (behavior == null)
      {
        behavior = new SelectionChangeBehavior(element);
        element.SetValue(ClickBehaviorProperty, behavior);
      }
      return behavior;
    }

    private static void OnSetCommandCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      System.Windows.Controls.ListView element = dependencyObject as System.Windows.Controls.ListView;
      if (element != null)
      {
        SelectionChangeBehavior behavior = GetOrCreateBehavior(element);
        behavior.Command = e.NewValue as ICommand;
      }
    }

    private static void OnSetCommandParameterCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
      System.Windows.Controls.ListView element = dependencyObject as System.Windows.Controls.ListView;
      if (element != null)
      {
        SelectionChangeBehavior behavior = GetOrCreateBehavior(element);
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

    public static SelectionChangeBehavior GetClickBehavior(DependencyObject obj)
    {
      return (SelectionChangeBehavior)obj.GetValue(ClickBehaviorProperty);
    }

    public static void SetClickBehavior(DependencyObject obj, SelectionChangeBehavior value)
    {
      obj.SetValue(ClickBehaviorProperty, value);
    }
  }
}