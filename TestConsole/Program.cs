using MoneyEntry.DataAccess.EFCore.Expenses;
using System;
using System.Linq;
using MoneyEntry.DataAccess.EFCore;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new ExpensesContext())
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
