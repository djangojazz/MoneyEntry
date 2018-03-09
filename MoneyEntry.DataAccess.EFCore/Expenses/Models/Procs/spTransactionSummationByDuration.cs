using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    public sealed class spTransactionSummationByDuration
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string GroupName { get; set; }
        public DateTime Grouping { get; set; }
        [Key]
        public int Position { get; set; }
        public float Amount { get; set; }
    }
}
