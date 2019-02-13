using System.ComponentModel.DataAnnotations;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    public sealed class spPostTransactionsToArchive
    {
        [Key]
        public int RowsUpdated { get; set; }
    }
}
