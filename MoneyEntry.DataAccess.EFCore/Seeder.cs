using MoneyEntry.DataAccess.EFCore.Expenses;
using MoneyEntry.DataAccess.EFCore.Expenses.Models;
using System.Collections.Generic;
using System.Linq;

namespace MoneyEntry.DataAccess.EFCore
{
    public static class Seeder
    {
        public static void Initialize(ExpensesContext context)
        {
            context.Database.EnsureCreated();

            if(!context.TePerson.Any())
            {
                context.TePerson.Add(new TePerson("Test", "Test", "TestUser", Crypto.GetSalt(), Crypto.GetSalt(256)));
                context.SaveChanges();
            }

            //Only invoke seeding of data if tables do not contain data already.
            if (!context.TdType.Any())
            {
                new List<TdType>
                {
                    new TdType("Credit"),
                    new TdType("Debit"),
                    new TdType("Adjustment")
                }
                .ForEach(x => context.TdType.Add(x));
                context.SaveChanges();
            }

            if (!context.TdCategory.Any())
            {
                new List<TdCategory>
                {
                    new TdCategory("EatingOut"),
                    new TdCategory("Food"),
                    new TdCategory("Car"),
                    new TdCategory("Electronics"),
                    new TdCategory("Transfer"),
                    new TdCategory("Education"),
                    new TdCategory("Bills"),
                    new TdCategory("Rent/Mortgage"),
                    new TdCategory("Cellphone"),
                    new TdCategory("Paycheck"),
                    new TdCategory("Alcohol"),
                    new TdCategory("Gas"),
                    new TdCategory("Fee"),
                    new TdCategory("Health"),
                    new TdCategory("Clothing"),
                    new TdCategory("Fun"),
                    new TdCategory("Miscellaneous"),
                    new TdCategory("Gifts"),
                    new TdCategory("Wedding"),
                    new TdCategory("Debt"),
                    new TdCategory("Pets"),
                    new TdCategory("Parking"),
                    new TdCategory("Paddling"),
                    new TdCategory("Household Goods"),
                    new TdCategory("Cleaning"),
                    new TdCategory("Insurance"),
                    new TdCategory("Charity/Donation"),
                    new TdCategory("Travel"),
                    new TdCategory("Kids"),
                    new TdCategory("Investments")
                }
                .ForEach(x => context.TdCategory.Add(x));
                context.SaveChanges();
            }
        }
    }
}
