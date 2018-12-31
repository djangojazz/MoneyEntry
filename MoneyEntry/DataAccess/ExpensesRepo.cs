using Controls;
using MoneyEntry.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyEntry.DataAccess
{
  public sealed class ExpensesRepo : IDisposable
  {
    private DataRetreival _dataRetreival;

    private static readonly ExpensesRepo _instance = new ExpensesRepo();
    static ExpensesRepo() { }
    private ExpensesRepo()
    {
      _dataRetreival = DataRetreival.Instance;
      People = GetPeople();
      Types = GetTypes();
      _categories = GetCategories();
    }

    public static ExpensesRepo Instance { get => _instance; }
    public IList<MoneyEntryObservable> MoneyEntryContainer { get; } = new List<MoneyEntryObservable>();
    public IList<TypeTran> Types { get; }
    private List<Category> _categories;
    public List<Category> Categories { get => _categories; }
    public IList<Person> People { get; }
    
    #region Methods

    #region RetreivalMethods
    private IList<TypeTran> GetTypes() => _dataRetreival.GetTypes().Select(x => new TypeTran(x.TypeId, x.Description)).ToList();

    private List<Category> GetCategories() => _dataRetreival.GetCategories().Select(x => new Category(x.CategoryId, x.Description)).ToList();

    public List<Person> GetPeople() => _dataRetreival.GetPeople().Select(x => new Person(x)).ToList();
    
    public List<MoneyEntryObservable> QueryMoneyEntries(DateTime start, DateTime end, int personId, int categoryId, int typeId, string description = null, decimal? moneyAmount = null)
    {
      return _dataRetreival.QueryMoneyEntries(start, end, personId, categoryId, typeId, description, moneyAmount)
        .Select(dbTran => new MoneyEntryObservable(dbTran.PersonID, dbTran.TransactionID, dbTran.Description, dbTran.CreatedDate, dbTran.TypeID, dbTran.CategoryID, dbTran.Amount, dbTran.RunningTotal, dbTran.Reconciled))
        .ToList();
    }
    
    public List<MoneyEntryObservable> GetModelObservables(DateTime start, DateTime end, int personId)
    {
        return _dataRetreival.GetTransactionViews(start, end, personId)
            .OrderBy(d => d.CreatedDate)
            .Select(dbTran => new MoneyEntryObservable(dbTran.PersonID, dbTran.TransactionID, dbTran.Description, dbTran.CreatedDate, dbTran.TypeID, dbTran.CategoryID, dbTran.Amount, dbTran.RunningTotal, dbTran.Reconciled))
            .ToList();
    }

    public DateTime? LastDateEnteredByPerson(int personId, bool? reconciled = null) => _dataRetreival.LastDateEnteredByPerson(personId, reconciled);

    public List<string> TextEntryAcrossRange(DateTime start, DateTime end, int personId) => GetModelObservables(start, end, personId).Select(x => x.Description).Distinct().ToList();
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
          _categories = GetCategories();
        }
      }
      catch (Exception ex)
      {
#if DEBUG
        Console.WriteLine(ex.Message);
#endif
      }
    }

    public int InsertOrUpdateTransaction(MoneyEntryObservable tran)
    {
      using (var context = new ExpensesEntities())
      {
        try
        {
          return context.spInsertOrUpdateTransaction(tran.TransactionId, tran.Amount, tran.Description, tran.TypeId, tran.CategoryId, tran.CreatedDate, tran.PersonId, tran.Reconciled);
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

    public void Refresh(DateTime start, DateTime end, int personId) => MoneyEntryContainer.ClearAndAddRange(GetModelObservables(start, end, personId));

    public void Dispose()
    {
      _dataRetreival = null;
    }
    #endregion

    #endregion
  }
}
