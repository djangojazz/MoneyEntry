using MoneyEntry.DataAccess.EFCore;
using MoneyEntry.DataAccess.EFCore.Expenses;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        private static IConfiguration _configuration { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
            var connectionString = _configuration.GetConnectionString("Expenses");

            ExpensesRepository.SetConnectionFirstTime(connectionString);
            var repo = ExpensesRepository.Instance;

            var yesterday = DateTime.Now.Date.AddDays(-1);

            var result = Task.Factory.StartNew(async () =>
            {
                //await repo.InsertOrUpdaTeTransactionAsync(0, 100, "Test debit", 2, 14, yesterday.Date.AddDays(1), 1, false);
                var rows = await repo.GetTransactionViewsAsync(yesterday, yesterday.Date.AddDays(1), 1);
                
            }
            );


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

            Console.ReadLine();
        }
    }
}
