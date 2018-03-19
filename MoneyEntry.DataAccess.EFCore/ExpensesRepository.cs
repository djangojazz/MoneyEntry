﻿using Microsoft.EntityFrameworkCore;
using MoneyEntry.DataAccess.EFCore.Expenses;
using MoneyEntry.DataAccess.EFCore.Expenses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntry.DataAccess.EFCore
{
    public sealed class ExpensesRepository
    {
        #region SingletonPattern
        private static string _connection;

        private static ExpensesRepository _instance = null;
        static ExpensesRepository() { }
        public ExpensesRepository() { }
        public static ExpensesRepository Instance { get => _instance; }

        public static void SetConnectionFirstTime(string connection)
        {
            if (_instance == null)
            {
                _connection = connection;
                _instance = new ExpensesRepository();
            }
        } 
        #endregion

        #region Methods

        #region RetreivalMethods
        public IList<TdType> GetTypes() => GetEntities<TdType>(x => x.TypeId != 3);
        public async Task<IList<TdType>> GetTypesAsync() => await GetEntitiesAsync<TdType>(x => x.TypeId != 3);

        public List<TdCategory> GetCategories() => GetEntities<TdCategory>();
        public async Task<List<TdCategory>> GetCategoriesAsync() => await GetEntitiesAsync<TdCategory>();

        public List<TePerson> GetPeople() => GetEntities<TePerson>().ToList();
        public async Task<List<TePerson>> GetPeopleAsync() => await GetEntitiesAsync<TePerson>();

        public List<vTrans> QueryMoneyEntries(DateTime start, DateTime end, int personId, int categoryId, int typeId, string description = null, decimal? moneyAmount = null)
        {
            var list = GetEntities<vTrans>(x => x.CreatedDate >= start && x.CreatedDate <= end && x.PersonID == personId && x.TypeID == typeId && x.CategoryID == categoryId);
            var final = (description == null) ? list.Where(x => x.Amount.Equals(moneyAmount)) : list.Where(x => x.TransactionDesc.Contains(description));
            return final.OrderBy(d => d.CreatedDate).ToList();
        }

        public async Task<List<vTrans>> QueryMoneyEntriesAsync(DateTime start, DateTime end, int personId, int categoryId, int typeId, string description = null, decimal? moneyAmount = null)
        {
            var list = await GetEntitiesAsync<vTrans>(x => x.CreatedDate >= start && x.CreatedDate <= end && x.PersonID == personId && x.TypeID == typeId && x.CategoryID == categoryId);
            var final = (description == null) ? list.Where(x => x.Amount.Equals(moneyAmount)) : list.Where(x => x.TransactionDesc.Contains(description));
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

        public List<string> TextEntryAcrossRange(DateTime start, DateTime end, int personId) => GetTransactionViews(start, end, personId).Select(x => x.TransactionDesc).Distinct().ToList();

        public async Task<List<string>> TextEntryAcrossRangeAsync(DateTime start, DateTime end, int personId)
        {
            var data = await GetTransactionViewsAsync(start, end, personId);
            return data.Select(x => x.TransactionDesc).Distinct().ToList();
        }
        #endregion

        #region AlterMethods
        public void AddCategory(string description)
        {
            try
            {
                using (var context = new ExpensesContext(_connection))
                {
                    context.TdCategory.Add(new TdCategory { Description = description });
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
                using (var context = new ExpensesContext(_connection))
                {
                    context.TdCategory.Add(new TdCategory { Description = description });
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

        public int InsertOrUpdaTeTransaction(int transactionId, decimal amount, string description, int typeId, int categoryId, DateTime createdDate, int personId, bool reconciled)
        {
            using (var context = new ExpensesContext(_connection))
            {
                try
                {
                    return context.spInsertOrUpdateTransaction.FromSql("spInsertOrUpdateTransaction @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7", 
                        parameters: new[] { transactionId, amount, (object)description, typeId, categoryId, createdDate, personId, reconciled }).FirstOrDefault()?.TransactionId ?? -1;
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

        public async Task<int> InsertOrUpdaTeTransactionAsync(int transactionId, decimal amount, string description, int typeId, int categoryId, DateTime createdDate, int personId, bool reconciled)
        {
            using (var context = new ExpensesContext(_connection))
            {
                try
                {
                    return await Task.Factory.StartNew(() => context.spInsertOrUpdateTransaction.FromSql("spInsertOrUpdateTransaction @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7",
                        parameters: new[] { transactionId, amount, (object)description, typeId, categoryId, createdDate, personId, reconciled }).FirstOrDefault()?.TransactionId ?? -1);
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

        public int UpdateTotals()
        {
            using (var context = new ExpensesContext(_connection))
            {
                try
                {
                    return context.spUpdateTotals.FromSql("spUpdateTotals").First().RowsUpdated;   
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

        public async Task<int> UpdateTotalsAsync()
        {
            using (var context = new ExpensesContext(_connection))
            {
                try
                {
                    return await Task.Factory.StartNew(() => context.spUpdateTotals.FromSql("spUpdateTotals").First().RowsUpdated);
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
            using (var context = new ExpensesContext(_connection))
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
            using (var context = new ExpensesContext(_connection))
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
