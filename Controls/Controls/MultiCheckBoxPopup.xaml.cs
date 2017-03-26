using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Controls
{

  public partial class MultiCheckBoxPopup : UserControl
  {
    public MultiCheckBoxPopup()
    {
      InitializeComponent();
      PART_Main.DataContext = this;
    }

    #region "IsMultiCheckBoxOpen" 
    public static readonly DependencyProperty IsMultiCheckBoxOpenProperty = DependencyProperty.Register("IsMultiCheckBoxOpen", typeof(bool), typeof(MultiCheckBoxPopup), new UIPropertyMetadata(false));
    public bool IsMultiCheckBoxOpen
    {
      get { return (bool)GetValue(IsMultiCheckBoxOpenProperty); }
      set { SetValue(IsMultiCheckBoxOpenProperty, value); }
    }
    #endregion

    #region "scrollingheight"

    public static readonly DependencyProperty ScrollingHeightproperty = DependencyProperty.Register("ScrollingHeight", typeof(int), typeof(MultiCheckBoxPopup), new UIPropertyMetadata(400));
    public int ScrollingHeight
    {
      get { return (int)GetValue(ScrollingHeightproperty); }
      set { SetValue(ScrollingHeightproperty, value); }
    }
    #endregion                                                        s

    #region "Header"   
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(MultiCheckBoxPopup), new UIPropertyMetadata(string.Empty));
    public string Header
    {
      get { return (string)GetValue(HeaderProperty); }

      set { SetValue(HeaderProperty, value); }
    }
    #endregion

    #region "ItemsCollection"

    public static readonly DependencyProperty ItemsCollectionProperty = DependencyProperty.Register("ItemsCollection", typeof(IList), typeof(MultiCheckBoxPopup), new UIPropertyMetadata(null));
    public IList ItemsCollection
    {
      get { return (IList)GetValue(ItemsCollectionProperty); }
      set { SetValue(ItemsCollectionProperty, value); }
    }
    #endregion
  }
}