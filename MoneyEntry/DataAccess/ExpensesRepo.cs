using Controls;
using MoneyEntry.Model;
using MoneyEntry.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace MoneyEntry.DataAccess
{
  public sealed class ExpensesRepo
  {
    private static readonly ExpensesRepo _instance = new ExpensesRepo();
    static ExpensesRepo() { }
    private ExpensesRepo() { }
    public static ExpensesRepo Instance { get => _instance; }

    private List<MoneyEntryModelViewModel> _moneyEntryContainer;

    public List<MoneyEntryModelViewModel> MoneyEntryContainer
    {
      get => (_moneyEntryContainer == null) ? _moneyEntryContainer = new List<MoneyEntryModelViewModel>() : _moneyEntryContainer;
      set => _moneyEntryContainer = value;
    }

    #region Properties
    public IEnumerable<TypeTran> _types;
    public IEnumerable<TypeTran> Types { get => (_types == null) ? _types = GetTypes() : _types; }
    
    private List<Category> _categories;
    public List<Category> Categories { get =>  (_categories == null) ? _categories = GetCurrentCategories() : _categories; }

    private List<Person> _people;
    public List<Person> People  { get => (_people == null) ? _people = GetPeople() : _people; }
    #endregion

    #region Methods
    private IEnumerable<TEntity> GetEntities<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : class
    {
      using (var context = new ExpensesEntities())
      {
        IQueryable<TEntity> data = context.Set<TEntity>();
        if (predicate != null)
        {
          data = data.Where(predicate);
        }

        return data.ToList();
      }
    }

    #region RetreivalMethods
    private IEnumerable<TypeTran> GetTypes() => GetEntities<tdType>(x => x.TypeID != 3).Select(x => new TypeTran(x.TypeID, x.Description));

    private List<Category> GetCurrentCategories() => GetEntities<tdCategory>().Select(x => new Category(x.CategoryID, x.Description)).ToList();

    public List<Person> GetPeople() => GetEntities<tePerson>().Select(x => new Person(x)).ToList();
    

    public List<MoneyEntryModelViewModel> QueryMoneyEntries(DateTime start, DateTime end, int personId, int categoryId, int typeId, string description = null, decimal? moneyAmount = null)
    {
      var list = GetEntities<vTrans>(x => x.CreatedDate >= start && x.CreatedDate <= end && x.PersonID == personId && x.TypeID == typeId && x.CategoryID == categoryId);
      var final = (description == null) ? list.Where(x => x.Amount.Equals(moneyAmount)) : list.Where(x => x.TransactionDesc.Contains(description));
      return final.OrderBy(d => d.CreatedDate).Select(dbTran => new MoneyEntryModelViewModel(new TransactionView(dbTran))).ToList();
    }

    private List<TransactionView> GetTransactionViews(DateTime start, DateTime end, int personId) => 
      GetEntities<vTrans>(x => x.CreatedDate >= start && x.CreatedDate <= end && x.PersonID == personId)
      .OrderBy(d => d.CreatedDate).Select(dbTran => new TransactionView(dbTran)).ToList();
    
    public DateTime? LastDateEnteredByPerson(int personId, bool? reconciled = null) => (DateTime)GetEntities<vTrans>(x => x.PersonID == personId && x.reconciled == reconciled)
      .OrderByDescending(x => x.CreatedDate).Select(x => x.CreatedDate).First();
    #endregion

    #region AlterMethods
    public void AddAndResetCategories(string description)
    {
      try
      {
        using (var context = new ExpensesEntities())
        {
          context.tdCategory.Add(new tdCategory { Description = description });
          context.SaveChanges();
          _categories = GetCurrentCategories();
        }
      }
      catch (Exception ex)
      {
#if DEBUG
        Console.WriteLine(ex.Message);
#endif
      }
    }

    public int InsertOrUpdateTransaction(TransactionView tran)
    {
      using (var context = new ExpensesEntities())
      {
        try
        {
          return context.spInsertOrUpdateTransaction(tran.TransactionID, tran.Amount, tran.TransactionDesc, tran.Type.TypeId, tran.Category.CategoryId, tran.CreatedDate, tran.Person.PersonId, tran.reconciled);
        }
        catch (Exception ex)
        {
#if DEBUG
          Console.WriteLine(ex.Message);
#endif
          return -1;
        }
      }
    }

    public void Refresh(DateTime start, DateTime end, int personId) => MoneyEntryContainer.ClearAndAddRange(GetTransactionViews(start, end, personId).ConvertAll(x => new MoneyEntryModelViewModel(x)));
    #endregion

    #endregion
  }
}
