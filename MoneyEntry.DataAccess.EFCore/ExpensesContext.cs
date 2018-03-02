﻿using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MoneyEntry.DataAccess.EFCore.Expenses.Models;

namespace MoneyEntry.DataAccess.EFCore.Expenses
{
    public partial class ExpensesContext : DbContext
    {
        private readonly string _connectionString;

        public ExpensesContext(string connectionString) => _connectionString = connectionString;

        public virtual DbSet<TdCategory> TdCategory { get; set; }
        public virtual DbSet<TdType> TdType { get; set; }
        public virtual DbSet<TePerson> TePerson { get; set; }
        public virtual DbSet<TeTransaction> TeTransaction { get; set; }
        public virtual DbSet<vTrans> vTrans { get; set; }

        // Unable to generate entity type for table 'dbo.export'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.teRunningTotals'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
                    //(@"Server=.;Database=ExpensesTestView;integrated security=True");
                //optionsBuilder.UseSqlServer(@"Server=tcp:brettdb.database.windows.net,1433;Database=Expenses;User ID=bmorin@brettdb;Password=nope");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //A Unique key cannot work on data annotations attributes so they are done in the model builder instead.
            modelBuilder.Entity<TdCategory>(entity => entity.HasIndex(e => e.Description).HasName("U_UniqueDescriptionCategory").IsUnique());
            modelBuilder.Entity<TdType>(entity => entity.HasIndex(e => e.Description).HasName("U_UniqueDescriptionType").IsUnique());
            modelBuilder.Entity<TePerson>(entity => entity.HasIndex(e => new { e.FirstName, e.LastName }).HasName("U_UniqueFirstNameLastNamePerson").IsUnique());

            //An index is most likely the same deal as above
            modelBuilder.Entity<TeTransaction>(entity => entity.HasIndex(e => new { e.CreatedDt, e.TransactionId, e.PersonId }).HasName("IX_teTransaction_PersonId"));
        }
    }
}
