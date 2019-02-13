using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    public class TeTransactionArchive
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TransactionId { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        [Column(TypeName = "varchar(128)")]
        public string TransactionDesc { get; set; }
        [ForeignKey("TypeId")]
        public TdType Type { get; set; }
        [ForeignKey("CategoryId")]
        public TdCategory Category { get; set; }
        public DateTime? CreatedDt { get; set; }
        public DateTime? ModifiedDt { get; set; }
        public bool? Reconciled { get; set; }
        [Column(TypeName = "money")]
        public decimal? RunningTotal { get; set; }
        public int PersonId { get; set; }
        [ForeignKey("PersonId")]
        public TePerson Person { get; set; }
    }
}
