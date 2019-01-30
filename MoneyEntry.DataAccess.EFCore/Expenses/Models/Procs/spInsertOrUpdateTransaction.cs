using System.ComponentModel.DataAnnotations;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    public class spInsertOrUpdateTransaction
    {
        [Key]
        public int TransactionId { get; set; }
    }
}
