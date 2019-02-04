using MoneyEntry.DataAccess.EFCore;
using MoneyEntry.DataAccess.EFCore.Expenses;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestConsole
{
    class Program
    {
        private static IConfiguration _configuration { get; set; }

        static void Main(string[] args)
        {
            var baseLocale = Extensions.GetBaseDirectory();

            var builder = new ConfigurationBuilder()
                    .SetBasePath(baseLocale)
                    .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
            var connectionString = _configuration.GetConnectionString("Expenses");

            ////First time seeder if needed
            //using (var context = new ExpensesContext())
            //{
            //    try
            //    {
            //        Seeder.Initialize(context);
            //        Console.WriteLine("Success");
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Fail");
            //    }
            //}

            ExpensesRepository.SetConnectionFirstTime(connectionString);
            var repo = ExpensesRepository.Instance;

            var yesterday = DateTime.Now.Date.AddDays(-1);

            //repo.InsertOrUpdaTeTransaction(0, 1000, "Start", 1, 20, yesterday, 1, false);
            //repo.InsertOrUpdaTeTransaction(0, 200, "Transaction Test", 2, 13, yesterday, 1, false);

            //[
            //{"transactionId": "6853", "reconciled": "true"},
            //{"transactionId": "6854", "reconciled": "true"}
            //]

            //var items = new (int transactionId, bool reconciled)[] ;
            //var json = JsonConvert.SerializeObject(new object[] 
            //    {
            //        new { transactionId= 1, reconciled =  false},
            //        new { transactionId= 2, reconciled =  false}
            //    });
            //var result = repo.ReconcileTransactions(json);
            //Console.WriteLine(result);

            Console.ReadLine();
        }
    }
}
