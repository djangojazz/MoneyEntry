using System.ComponentModel;
using System.Windows.Input;
using MoneyEntry.Model;
using MessageBox = System.Windows.Forms.MessageBox;
using System;

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
      Validation();
    }

    #region Properties
    public override string DisplayName { get => "Add Category (" + _Person.FullName + ")"; }
    public ICommand AddCommand { get => (_Addcommand == null) ? _Addcommand = new RelayCommand(param => Add(), can => !HasError) : _Addcommand; }

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
        Validation();
        OnPropertyChanged(nameof(Desc));
      }
    } 
    #endregion

    private void Add()
    {
      Repository.AddAndResetCategories(_Desc);
      MessageBox.Show($"Added {_Desc}{Environment.NewLine}Closing window");
      OnRequestClose();
    }
    
    protected override void Validation()
    {
      SetError("Category:", (String.IsNullOrEmpty(Desc)) ? "Need a category name." : String.Empty);
    }
  }
}
