﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoneyEntry.DataAccess.EFCore.Expenses.Models
{
    public sealed class vTrans
    {
        public int PersonID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public string TransactionDesc { get; set; }
        public byte TypeID { get; set; }
        public string Type { get; set; }
        public byte CategoryID { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }
        [Key]
        public int TransactionID { get; set; }
        public double RunningTotal { get; set; }
        public bool reconciled { get; set; }
    }
}
