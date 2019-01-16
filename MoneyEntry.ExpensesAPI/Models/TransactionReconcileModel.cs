using System.ComponentModel.DataAnnotations;

namespace MoneyEntry.ExpensesAPI.Models
{
    public class TransactionReconcileModel
    {
        [Required]
        public int transactionId { get; set; }

        [Required]
        public bool reconciled { get; set; }
    }
}