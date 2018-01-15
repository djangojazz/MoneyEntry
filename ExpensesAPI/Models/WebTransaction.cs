using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpensesAPI.Models
{
    public class WebTransaction
    {
        public WebTransaction() { }

        [Required, Range(0, int.MaxValue)]
        public int TransactionId { get; set; }
        [Required, Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Description { get; set; }
        [Required, Range(1, 2)]
        public int TypeId { get; set; }
        [Required, Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
        [Required, DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        [Required, Range(1, int.MaxValue)]
        public int PersonId { get; set; }
        [Required]
        public bool Reconciled { get; set; }
    }
}