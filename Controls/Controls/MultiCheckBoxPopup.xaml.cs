using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Controls
{
  /// <summary>
  /// Interaction logic for MultiCheckBoxPopup.xaml
  /// </summary>
  public partial class MultiCheckBoxPopup : UserControl
  {
    public MultiCheckBoxPopup()
    {
      InitializeComponent();
    }


    #region "IsMultiCheckBoxOpen"

    public static readonly DependencyProperty IsMultiCheckBoxOpenProperty = DependencyProperty.Register("IsMultiCheckBoxOpen", typeof(bool), typeof(MultiCheckBoxPopup), new PropertyMetadata(false));
    public bool IsMultiCheckBoxOpen
    {
      get { return (bool)GetValue(IsMultiCheckBoxOpenProperty); }
      set { SetValue(IsMultiCheckBoxOpenProperty, value); }
    }
    #endregion

    //#region "ScrollingHeight"

    //public static readonly DependencyProperty ScrollingHeightProperty = DependencyProperty.Register("ScrollingHeight", typeof(int), typeof(MultiCheckBoxPopup), new PropertyMetadata(400));
    //public string ScrollingHeight
    //{
    //  get { return Convert.ToInt32(GetValue(ScrollingHeightProperty).ToString()); }

    //  set { SetValue(ScrollingHeightProperty, value); }
    //}

    //#endregion                                                        s

    #region "Header"

    

    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(MultiCheckBoxPopup), new PropertyMetadata(string.Empty));
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