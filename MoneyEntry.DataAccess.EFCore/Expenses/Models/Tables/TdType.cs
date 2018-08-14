using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    [Table("tdType")]
    public partial class TdType
    {
        public TdType() { }
        public TdType(string description) => Description = description;
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte TypeId { get; set; }

        [Column(TypeName = "varchar(128)")]
        public string Description { get; set; }
    }
}
