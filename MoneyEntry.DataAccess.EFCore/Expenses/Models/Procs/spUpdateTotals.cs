using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    public sealed class spUpdateTotals
    {
        [Key]
        public int RowsUpdated { get; set; }
    }
}
