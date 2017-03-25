using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Controls
{
  /// <summary>
  /// Interaction logic for SearchCombo.xaml
  /// </summary>
  public partial class SearchCombo : UserControl
  {
    //DEPENDENCY PROPERTIES
    public static readonly DependencyProperty FilterTextProperty = DependencyProperty.Register("FilterText", typeof(string), typeof(SearchCombo), new UIPropertyMetadata(string.Empty, ReloadFilteredList));
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(SearchCombo), new UIPropertyMetadata(false, ExpandChanged));
    public static readonly DependencyProperty MaxResultsProperty = DependencyProperty.Register("MaxResults", typeof(int), typeof(SearchCombo), new UIPropertyMetadata(500, ReloadFilteredList));
    public static readonly DependencyProperty CompleteCollectionProperty = DependencyProperty.Register("CompleteCollection", typeof(IList), typeof(SearchCombo), new UIPropertyMetadata(new ObservableCollection<object>(), ReloadFilteredList));
    public static readonly DependencyProperty FilteredCollectionProperty = DependencyProperty.Register("FilteredCollection", typeof(ObservableCollection<object>), typeof(SearchCombo), new UIPropertyMetadata(new ObservableCollection<object>(), ReloadFilteredList));
    public static readonly DependencyProperty SelectCommandProperty = DependencyProperty.Register("SelectCommand", typeof(System.Windows.Input.ICommand), typeof(SearchCombo), new UIPropertyMetadata(null));
    public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register("Selected", typeof(object), typeof(SearchCombo), new UIPropertyMetadata(string.Empty));
    public static readonly DependencyProperty SelectedFilterProperty = DependencyProperty.Register("SelectedFilter", typeof(string), typeof(SearchCombo), new UIPropertyMetadata(string.Empty));

    public static readonly DependencyProperty PlacementProperty = DependencyProperty.Register("Placement", typeof(PlacementMode), typeof(SearchCombo), new UIPropertyMetadata(PlacementMode.Bottom));

    //CONSTRUCTOR
    public SearchCombo()
    {
      InitializeComponent();

      this.FilteredCollection = new ObservableCollection<object>();
      Part_Layout.DataContext = this;
    }


    //PROPERTIES
    public string FilterText
    {
      get { return Convert.ToString(GetValue(FilterTextProperty)); }
      set { SetValue(FilterTextProperty, value); }
    }

    public bool IsExpanded
    {
      get { return Convert.ToBoolean(GetValue(IsExpandedProperty)); }
      set { SetValue(IsExpandedProperty, value); }
    }

    public int MaxResults
    {
      get { return Convert.ToInt32(GetValue(MaxResultsProperty)); }
      set { SetValue(MaxResultsProperty, value); }
    }

    public IList CompleteCollection
    {
      get { return (IList)GetValue(CompleteCollectionProperty); }
      set { SetValue(CompleteCollectionProperty, value); }
    }

    public ObservableCollection<object> FilteredCollection
    {
      get { return (ObservableCollection<object>)GetValue(FilteredCollectionProperty); }
      private set { SetValue(FilteredCollectionProperty, value); }
    }

    public PlacementMode Placement
    {
      get { return (PlacementMode)GetValue(PlacementProperty); }
      private set { SetValue(PlacementProperty, value); }
    }

    public System.Windows.Input.ICommand SelectCommand
    {
      get { return (System.Windows.Input.ICommand)GetValue(SelectCommandProperty); }
      set { SetValue(SelectCommandProperty, value); }
    }

    public object Selected
    {
      get { return GetValue(SelectedProperty); }
      set { SetValue(SelectedProperty, value); }
    }

    public string SelectedFilter
    {
      get { return Convert.ToString(GetValue(SelectedProperty)); }
      set { SetValue(SelectedProperty, value); }
    }

    public DelegateCommand<object> CommandSelect { get; set; }

    public DelegateCommand<object> CommandEsc { get; set; }


    //METHODS
    private static void ReloadFilteredList(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((SearchCombo)d).LoadFilterSet();
    }

    private void SelectElement(object o)
    {
      if ((o == null))
        return;
      var EnumerableObjects = from object p in CompleteCollection select p;
      Selected = EnumerableObjects.Where(x => x.ToString() == o.ToString()).FirstOrDefault();
      FocusButton();
      if ((SelectCommand != null))
        SelectCommand.Execute(Selected);
    }

    private void CloseControl(object o)
    {
      if (IsExpanded)
        FocusButton();
    }

    public static void ExpandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      dynamic Control = (SearchCombo)d;
      if (!Control.IsExpanded)
      {
        Control.FocusButton();
      }
      else
      {
        Control.LoadFilterSet();
      }
    }

    public void FocusButton()
    {
      IsExpanded = false;
      Part_Button.Focus();
    }

    public void LoadFilterSet()
    {
      if (!IsExpanded)
        return;
      var EnumerableObjects = from object p in CompleteCollection select p;
      FilteredCollection.ClearAndAddRange(EnumerableObjects.Where(t => t.ToString().ContainsInvariant(FilterText)).Take(MaxResults));
    }
  }
}