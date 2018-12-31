using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoneyEntry.DataAccess
{
    public sealed class DataRetreival
    {
        private static readonly DataRetreival _instance = new DataRetreival();
        static DataRetreival() { }
        private DataRetreival() { }
        public static DataRetreival Instance { get => _instance; }

        #region Methods

        #region RetreivalMethods
        public IList<tdType> GetTypes() => GetEntities<tdType>(x => x.TypeId != 3);
        public async Task<IList<tdType>> GetTypesAsync() => await GetEntitiesAsync<tdType>(x => x.TypeId != 3);

        public List<tdCategory> GetCategories() => GetEntities<tdCategory>();
        public async Task<List<tdCategory>> GetCategoriesAsync() => await GetEntitiesAsync<tdCategory>();

        public List<tePerson> GetPeople() => GetEntities<tePerson>().ToList();
        public async Task<List<tePerson>> GetPeopleAsync() => await GetEntitiesAsync<tePerson>();

        public List<vTrans> QueryMoneyEntries(DateTime start, DateTime end, int personId, int categoryId, int typeId, string description = null, decimal? moneyAmount = null)
        {
            var list = GetEntities<vTrans>(x => x.CreatedDate >= start && x.CreatedDate <= end && x.PersonID == personId && x.TypeID == typeId && x.CategoryID == categoryId);
            var final = (description == null) ? list.Where(x => x.Amount.Equals(moneyAmount)) : list.Where(x => x.Description.Contains(description));
            return final.OrderBy(d => d.CreatedDate).ToList();
        }

        public async Task<List<vTrans>> QueryMoneyEntriesAsync(DateTime start, DateTime end, int personId, int categoryId, int typeId, string description = null, decimal? moneyAmount = null)
        {
            var list = await GetEntitiesAsync<vTrans>(x => x.CreatedDate >= start && x.CreatedDate <= end && x.PersonID == personId && x.TypeID == typeId && x.CategoryID == categoryId);
            var final = (description == null) ? list.Where(x => x.Amount.Equals(moneyAmount)) : list.Where(x => x.Description.Contains(description));
            return final.OrderBy(d => d.CreatedDate).ToList();
        }

        public List<vTrans> GetTransactionViews(DateTime start, DateTime end, int personId) =>
          GetEntities<vTrans>(x => x.CreatedDate >= start && x.CreatedDate <= end && x.PersonID == personId).OrderBy(d => d.CreatedDate).ToList();

        public async Task<List<vTrans>> GetTransactionViewsAsync(DateTime start, DateTime end, int personId) =>
            (await GetEntitiesAsync<vTrans>(x => x.CreatedDate >= start && x.CreatedDate <= end && x.PersonID == personId)).OrderBy(d => d.CreatedDate).ToList();


        public DateTime? LastDateEnteredByPerson(int personId, bool? reconciled = null) => 
            (DateTime)GetEntities<vTrans>(x => x.PersonID == personId && x.Reconciled == (reconciled ?? false))
                .OrderByDescending(x => x.CreatedDate).Select(x => x.CreatedDate).FirstOrDefault();

        public async Task<DateTime?> LastDateEnteredByPersonAsync(int personId, bool? reconciled = null)
        {
            var data = await GetEntitiesAsync<vTrans>(x => x.PersonID == personId && x.Reconciled == (reconciled ?? false));
            return (DateTime)data.OrderByDescending(x => x.CreatedDate).Select(x => x.CreatedDate).FirstOrDefault();
        }

        public List<string> TextEntryAcrossRange(DateTime start, DateTime end, int personId) => GetTransactionViews(start, end, personId).Select(x => x.Description).Distinct().ToList();

        public async Task<List<string>> TextEntryAcrossRangeAsync(DateTime start, DateTime end, int personId)
        {
            var data = await GetTransactionViewsAsync(start, end, personId);
            return data.Select(x => x.Description).Distinct().ToList();
        }
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

        public async Task AddCategoryAsync(string description)
        {
            try
            {
                using (var context = new ExpensesEntities())
                {
                    context.tdCategory.Add(new tdCategory { Description = description });
                    await context.SaveChangesAsync();
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
        
        public async Task<int> InsertOrUpdateTransactionAsync(int transactionId, decimal amount, string description, int typeId, int categoryId, DateTime createdDate, int personId, bool reconciled)
        {
            using (var context = new ExpensesEntities())
            {
                try
                {
                    var data = await Task.Factory.StartNew(() => context.spInsertOrUpdateTransaction(transactionId, amount, description, typeId, categoryId, createdDate, personId, reconciled));
                    return data;
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


        private List<TEntity> GetEntities<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : class
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

        private async Task<List<TEntity>> GetEntitiesAsync<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : class
        {
            using (var context = new ExpensesEntities())
            {
                IQueryable<TEntity> data = context.Set<TEntity>();
                if (predicate != null)
                {
                    data = data.Where(predicate);
                }

                return await data.ToListAsync();
            }
        }
        #endregion
    }
}
