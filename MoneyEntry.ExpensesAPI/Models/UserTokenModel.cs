using System.ComponentModel.DataAnnotations;

namespace MoneyEntry.ExpensesAPI.Models
{
    public class UserTokenModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
