using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    public sealed class spCategoryUseOverDuration
    {
        [Key]
        public int CategoryID { get; set; }
        public int TypeID { get; set; }
        public string TypeDesc { get; set; }
        public string CategoryDesc { get; set; }
        public float Amount { get; set; }
    }
}
