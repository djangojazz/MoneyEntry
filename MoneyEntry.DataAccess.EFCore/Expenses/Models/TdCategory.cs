using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    [Table("tdCategory")]
    public sealed class TdCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte CategoryId { get; set; }
        [Column(TypeName = "varchar(128)")]
        public string Description { get; set; }
    }
}
