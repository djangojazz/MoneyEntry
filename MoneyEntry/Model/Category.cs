using MoneyEntry.ViewModel;
using MoneyEntry.DataAccess;


namespace MoneyEntry.Model
{
  public class Category : ViewModelBase
  {                    
    public Category() {}
    public Category(byte categoryId, string categoryName)
    {
      CategoryId = categoryId;
      CategoryName = categoryName;
    }
    
    public byte CategoryId { get; set; }
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
                       
    public override string ToString() => CategoryName;
  }
}
