using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace MoneyEntry.Model
{
    public class MoneyEntryModel
    {
        ExpensesEntities ee = new ExpensesEntities();

        public int PersonId { get; set; }
        public string Name { get; set; }
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionDesc { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? RunningTotal { get; set; }
        public bool? Reconciled { get; set; }

        public ReadOnlyCollection<string> Ts
        {
            get
            {
                
                return new ReadOnlyCollection<string>(GetTypes());
            }
        }

        public ReadOnlyCollection<string> Cs
        {
            get
            {
                return new ReadOnlyCollection<string>(GetCats());
            }
        }

        private List<string> GetTypes()
        {
            return new List<string>() { "Credit", "Debit"};
        }

        private List<string> GetCats()
        {
            return ee.tdCategory.Select(n => n.Description).ToList();
        }
    }
}
