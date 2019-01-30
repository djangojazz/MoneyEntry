using MoneyEntry.DataAccess.EFCore.Expenses.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyEntry.DataAccess.EFCore
{
    public interface IExpensesRepository
    {
        IList<TdType> GetTypes();
        Task<IList<TdType>> GetTypesAsync();
        List<TdCategory> GetCategories();
        Task<List<TdCategory>> GetCategoriesAsync();
        List<TePerson> GetPeople();
        Task<List<TePerson>> GetPeopleAsync();
        List<vTrans> QueryMoneyEntries(DateTime start, DateTime end, int personId, int categoryId, int typeId, string description = null, decimal? moneyAmount = null);
        Task<List<vTrans>> QueryMoneyEntriesAsync(DateTime start, DateTime end, int personId, int categoryId, int typeId, string description = null, decimal? moneyAmount = null);
        List<vTrans> GetTransactionViews(DateTime start, DateTime end, int personId);
        Task<List<vTrans>> GetTransactionViewsAsync(DateTime start, DateTime end, int personId);
        DateTime? LastDateEnteredByPerson(int personId, bool? reconciled = null);
        Task<DateTime?> LastDateEnteredByPersonAsync(int personId, bool? reconciled = null);
        List<string> TextEntryAcrossRange(DateTime start, DateTime end, int personId);
        Task<List<string>> TextEntryAcrossRangeAsync(DateTime start, DateTime end, int personId);
        void AddCategory(string description);
        Task AddCategoryAsync(string description);
        int InsertOrUpdaTeTransaction(int transactionId, decimal amount, string description, int typeId, int categoryId, DateTime createdDate, int personId, bool reconciled);
        Task<int> InsertOrUpdaTeTransactionAsync(int transactionId, decimal amount, string description, int typeId, int categoryId, DateTime createdDate, int personId, bool reconciled);
        int UpdateTotals();
    }
}
