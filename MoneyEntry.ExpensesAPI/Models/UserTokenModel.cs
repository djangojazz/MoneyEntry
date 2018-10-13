using System.ComponentModel.DataAnnotations;

namespace MoneyEntry.ExpensesAPI.Models
{
    public class UserTokenModel
    {
        [Required]
        public string UserName { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Password { get; set; }
        public int UserId { get; set; }
    }
}
