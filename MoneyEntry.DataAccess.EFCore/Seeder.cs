using MoneyEntry.DataAccess.EFCore.Expenses;
using MoneyEntry.DataAccess.EFCore.Expenses.Models;
using System;
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
                context.TePerson.Add(new TePerson { FirstName = "Test", LastName = "Test" });
                context.SaveChanges();
            }

            //Only invoke seeding of data if tables do not contain data already.
            if (!context.TdType.Any())
            {
                new List<TdType>
                {
                    new TdType{ Description = "Credit" },
                    new TdType{ Description = "Debit" },
                    new TdType{ Description = "Adjustment" }
                }
                .ForEach(x => context.TdType.Add(x));
                context.SaveChanges();
            }

            if (!context.TdCategory.Any())
            {
                new List<TdCategory>
                {
                    new TdCategory { Description = "EatingOut" },
                    new TdCategory { Description = "Food" },
                    new TdCategory { Description = "Car" },
                    new TdCategory { Description = "Electronics" },
                    new TdCategory { Description = "Transfer" },
                    new TdCategory { Description = "Education" },
                    new TdCategory { Description = "Bills" },
                    new TdCategory { Description = "Rent/Mortgage" },
                    new TdCategory { Description = "Cellphone" },
                    new TdCategory { Description = "Paycheck" },
                    new TdCategory { Description = "Alcohol" },
                    new TdCategory { Description = "Gas" },
                    new TdCategory { Description = "Fee" },
                    new TdCategory { Description = "Health" },
                    new TdCategory { Description = "Clothing" },
                    new TdCategory { Description = "Fun" },
                    new TdCategory { Description = "Miscellaneous" },
                    new TdCategory { Description = "Gifts" },
                    new TdCategory { Description = "Wedding" },
                    new TdCategory { Description = "Debt" },
                    new TdCategory { Description = "Pets" },
                    new TdCategory { Description = "Parking" },
                    new TdCategory { Description = "Paddling" },
                    new TdCategory { Description = "Household Goods" },
                    new TdCategory { Description = "Cleaning" },
                    new TdCategory { Description = "Insurance" },
                    new TdCategory { Description = "Charity/Donation" },
                    new TdCategory { Description = "Travel" },
                    new TdCategory { Description = "Kids" },
                    new TdCategory { Description = "Investments" }
                }
                .ForEach(x => context.TdCategory.Add(x));
                context.SaveChanges();
            }
        }
    }
}
