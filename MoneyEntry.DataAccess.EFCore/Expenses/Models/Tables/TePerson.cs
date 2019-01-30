using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    [Table("tePerson")]
    public partial class TePerson
    {
        public TePerson() {}
        public TePerson(string firstName, string lastName, string userName, byte[] salt, byte[] password)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Salt = salt;
            Password = password;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId { get; set; }
        [Required, Column(TypeName = "varchar(255)")]
        public string FirstName { get; set; }
        [Required, Column(TypeName = "varchar(255)")]
        public string LastName { get; set; }
        [Required, Column(TypeName = "varchar(128)")]
        public string UserName { get; set; }
        [Required, MaxLength(128)]
        public byte[] Salt { get; set; }
        [Required, MaxLength(512)]
        public byte[] Password { get; set; }
    }
}
