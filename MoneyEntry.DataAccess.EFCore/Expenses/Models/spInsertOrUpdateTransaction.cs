using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    public class spInsertOrUpdateTransaction
    {
        [Key]
        public int TransactionId { get; set; }
    }
}
