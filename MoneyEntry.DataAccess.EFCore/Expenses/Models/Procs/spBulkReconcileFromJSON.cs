using System.ComponentModel.DataAnnotations;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    public class spBulkReconcileFromJSON
    {
        [Key]
        public int RowsUpdated { get; set; }
    }
}
