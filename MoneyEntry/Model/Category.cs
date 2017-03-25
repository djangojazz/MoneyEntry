using MoneyEntry.ViewModel;

namespace MoneyEntry.Model
{
  public class Category : ViewModelBase
  {                    
    public Category() {}
    public Category(int categoryId, string categoryName, bool isUsed)
    {
      CategoryId = categoryId;
      CategoryName = categoryName;
      IsUsed = isUsed;
    }

    public int CategoryId { get; set; }
    public string CategoryName { get; set; }

    private bool _isUsed;

    public bool IsUsed
    {
      get { return _isUsed; }
      set
      {
        _isUsed = value;
        OnPropertyChanged("IsUsed");
      }
    }
                       
    public override string ToString()
    {
      return CategoryName;
    }
  }
}
