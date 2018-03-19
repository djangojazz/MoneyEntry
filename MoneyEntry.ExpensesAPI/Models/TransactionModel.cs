using MoneyEntry.DataAccess.EFCore.Expenses.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyEntry.ExpensesAPI.Models
{
    public sealed class TransactionModel
    {
        public int TransactionId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required, MaxLength(128)]
        public string Description { get; set; }
        [Required]
        public int TypeId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public int PersonId { get; set; }
        
        public bool Reconciled { get; set; }
        
        public TransactionModel(decimal amount, string description, int typeId, int categoryId, DateTime createdDate, int personId, int transactionId = 0, bool reconciled = false)
        {
            TransactionId = transactionId;
            Amount = amount;
            Description = description;
            TypeId = typeId;
            CategoryId = categoryId;
            CreatedDate = createdDate;
            PersonId = personId;
            Reconciled = reconciled;
        }

        public TransactionModel(vTrans vTrans)
        {
            TransactionId = vTrans.TransactionID;
            Amount = vTrans.Amount;
            Description = vTrans.TransactionDesc;
            TypeId = vTrans.TypeID;
            CategoryId = vTrans.CategoryID;
            CreatedDate = vTrans.CreatedDate;
            PersonId = vTrans.PersonID;
            Reconciled = vTrans.Reconciled;
        }
    }
}
