using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MoneyEntry.DataAccess
{
  public sealed class DataRetreival
  {
    private static readonly DataRetreival _instance = new DataRetreival();
    static DataRetreival() { }
    private DataRetreival() {}
    public static DataRetreival Instance { get => _instance; }
    
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
    public IList<tdType> GetTypes() => GetEntities<tdType>(x => x.TypeID != 3).ToList();

    public List<tdCategory> GetCurrentCategories() => GetEntities<tdCategory>().ToList();

    public List<tePerson> GetPeople() => GetEntities<tePerson>().ToList();
    
    public List<vTrans> QueryMoneyEntries(DateTime start, DateTime end, int personId, int categoryId, int typeId, string description = null, decimal? moneyAmount = null)
    {
      var list = GetEntities<vTrans>(x => x.CreatedDate >= start && x.CreatedDate <= end && x.PersonID == personId && x.TypeID == typeId && x.CategoryID == categoryId);
      var final = (description == null) ? list.Where(x => x.Amount.Equals(moneyAmount)) : list.Where(x => x.TransactionDesc.Contains(description));
      return final.OrderBy(d => d.CreatedDate).ToList();
    }

    private List<vTrans> GetTransactionViews(DateTime start, DateTime end, int personId) => 
      GetEntities<vTrans>(x => x.CreatedDate >= start && x.CreatedDate <= end && x.PersonID == personId).OrderBy(d => d.CreatedDate).ToList();

    public DateTime? LastDateEnteredByPerson(int personId, bool? reconciled = null) => (DateTime)GetEntities<vTrans>(x => x.PersonID == personId && x.reconciled == (reconciled ?? false))
      .OrderByDescending(x => x.CreatedDate).Select(x => x.CreatedDate).First();

    public List<string> TextEntryAcrossRange(DateTime start, DateTime end, int personId) => GetTransactionViews(start, end, personId).Select(x => x.TransactionDesc).Distinct().ToList();
    #endregion

    #region AlterMethods
    public void AddCategory(string description)
    {
      try
      {
        using (var context = new ExpensesEntities())
        {
          context.tdCategory.Add(new tdCategory { Description = description });
          context.SaveChanges();
        }
      }
      catch (Exception ex)
      {
#if DEBUG
        Console.WriteLine(ex.Message);
#endif
      }
    }

    public int InsertOrUpdateTransaction(int transactionId, decimal amount, string description, int typeId, int categoryId, DateTime createdDate, int personId, bool reconciled)
    {
      using (var context = new ExpensesEntities())
      {
        try
        {
          return context.spInsertOrUpdateTransaction(transactionId, amount, description, typeId, categoryId, createdDate, personId, reconciled);
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
    #endregion

    #endregion
  }
}
