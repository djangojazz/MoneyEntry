using MoneyEntry.DataAccess.EFCore.Expenses.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyEntry.ExpensesAPI.Models
{
    public class TransactionModel
    {
        public int TransactionId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required, MaxLength(128)]
        public string Description { get; set; }
        [Required, Range(1,2)]
        public int TypeId { get; set; }
        [Required, Range(1,99)]
        public int CategoryId { get; set; }
        [Required, DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        [Required, Range(1,10)]
        public int PersonId { get; set; }

        public bool Reconciled { get; set; }
    }
}
