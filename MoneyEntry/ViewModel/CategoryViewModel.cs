using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using MoneyEntry.Model;
using MessageBox = System.Windows.Forms.MessageBox;

namespace MoneyEntry.ViewModel
{
  class CategoryViewModel : WorkspaceViewModel, IDataErrorInfo
  {
    private Person _Person;
    private RelayCommand _Addcommand;
    private string _Desc;
    private string _CurrentCategory;

    public CategoryViewModel(Person person)
    {
      _Person = person;
    }

    #region Properties
    public IEnumerable<Category> Categories { get => Repository.Categories; }
    public override string DisplayName { get => "Add Category (" + _Person.FullName + ")"; }
    public ICommand AddCommand { get => (_Addcommand == null) ? _Addcommand = new RelayCommand(param => Add()) : _Addcommand; }

    public string CurrentCategory
    {
      get => _CurrentCategory;
      set
      {
        _CurrentCategory = value;
        OnPropertyChanged(nameof(CurrentCategory));
      }
    }

    public string Desc
    {
      get => _Desc;
      set
      {
        _Desc = value;
        OnPropertyChanged(nameof(Desc));
      }
    } 
    #endregion

    private void Add()
    {
      Repository.AddAndResetCategories(_Desc);
      MessageBox.Show("Added " + _Desc);
    }

    #region IDataErrorInfo Members

    //TODO Hookup IDataError for description
    string IDataErrorInfo.Error { get => (_Person as IDataErrorInfo).Error; }

    string IDataErrorInfo.this[string propertyName]
    {
      get
      {
        string error = null;
        return error;
      }
    }

    #endregion // IDataErrorInfo Members
  }
}
