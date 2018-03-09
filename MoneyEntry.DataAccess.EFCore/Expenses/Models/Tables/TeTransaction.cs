using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    [Table("teTransaction")]
    public partial class TeTransaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
