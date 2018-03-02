using MoneyEntry.DataAccess.EFCore;
using MoneyEntry.DataAccess.EFCore.Expenses;
using System;
using Microsoft.Extensions.Configuration;
using System.IO;

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

            
            using (var context = new ExpensesContext(_configuration.GetConnectionString("Expenses")))
            {
                try
                {
                    Seeder.Initialize(context);
                    Console.WriteLine("Success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fail");
                }
            }

            Console.ReadLine();
        }
    }
}
