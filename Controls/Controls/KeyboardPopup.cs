using System;      
using System.Windows.Input;

namespace Controls
{
  public class KeyboardPopup : System.Windows.Controls.Primitives.Popup
  {
    protected override void OnOpened(EventArgs e)
    {
      base.OnOpened(e);
      this.Child.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
    }
  }
}