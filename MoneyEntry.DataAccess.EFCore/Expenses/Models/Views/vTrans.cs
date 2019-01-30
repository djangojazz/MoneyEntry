using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    public sealed class vTrans
    {
        public int PersonID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public byte TypeID { get; set; }
        public string Type { get; set; }
        public byte CategoryID { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }
        [Key]
        public int TransactionID { get; set; }
        public decimal RunningTotal { get; set; }
        public bool Reconciled { get; set; }
    }
}
