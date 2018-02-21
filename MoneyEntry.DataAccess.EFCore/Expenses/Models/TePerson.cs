using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    [Table("tePerson")]
    public partial class TePerson
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string FirstName { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string LastName { get; set; }
    }
}
