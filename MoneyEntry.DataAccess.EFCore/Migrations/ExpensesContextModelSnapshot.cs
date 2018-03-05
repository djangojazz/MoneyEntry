﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using MoneyEntry.DataAccess.EFCore.Expenses;
using System;

namespace MoneyEntry.DataAccess.EFCore.Migrations
{
    [DbContext(typeof(ExpensesContext))]
    partial class ExpensesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
            
            modelBuilder.Entity("MoneyEntry.DataAccess.EFCore.Expenses.Models.TdCategory", b =>
                {
                    b.Property<byte>("CategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasColumnType("varchar(128)");

                    b.HasKey("CategoryId");

                    b.HasIndex("Description")
                        .IsUnique()
                        .HasName("U_UniqueDescriptionCategory")
                        .HasFilter("[Description] IS NOT NULL");

                    b.ToTable("tdCategory");
                });

            modelBuilder.Entity("MoneyEntry.DataAccess.EFCore.Expenses.Models.TdType", b =>
                {
                    b.Property<byte>("TypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasColumnType("varchar(128)");

                    b.HasKey("TypeId");

                    b.HasIndex("Description")
                        .IsUnique()
                        .HasName("U_UniqueDescriptionType")
                        .HasFilter("[Description] IS NOT NULL");

                    b.ToTable("tdType");
                });

            modelBuilder.Entity("MoneyEntry.DataAccess.EFCore.Expenses.Models.TePerson", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(255)");

                    b.HasKey("PersonId");

                    b.HasIndex("FirstName", "LastName")
                        .IsUnique()
                        .HasName("U_UniqueFirstNameLastNamePerson")
                        .HasFilter("[FirstName] IS NOT NULL AND [LastName] IS NOT NULL");

                    b.ToTable("tePerson");
                });

            modelBuilder.Entity("MoneyEntry.DataAccess.EFCore.Expenses.Models.TeTransaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<byte?>("CategoryId");

                    b.Property<DateTime?>("CreatedDt");

                    b.Property<DateTime?>("ModifiedDt");

                    b.Property<int>("PersonId");

                    b.Property<bool?>("Reconciled");

                    b.Property<decimal?>("RunningTotal")
                        .HasColumnType("money");

                    b.Property<string>("TransactionDesc")
                        .HasColumnType("varchar(128)");

                    b.Property<byte?>("TypeId");

                    b.HasKey("TransactionId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PersonId");

                    b.HasIndex("TypeId");

                    b.HasIndex("CreatedDt", "TransactionId", "PersonId")
                        .HasName("IX_teTransaction_PersonId");

                    b.ToTable("teTransaction");
                });
            

            modelBuilder.Entity("MoneyEntry.DataAccess.EFCore.Expenses.Models.TeTransaction", b =>
                {
                    b.HasOne("MoneyEntry.DataAccess.EFCore.Expenses.Models.TdCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("MoneyEntry.DataAccess.EFCore.Expenses.Models.TePerson", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MoneyEntry.DataAccess.EFCore.Expenses.Models.TdType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId");
                });
#pragma warning restore 612, 618
        }
    }
}