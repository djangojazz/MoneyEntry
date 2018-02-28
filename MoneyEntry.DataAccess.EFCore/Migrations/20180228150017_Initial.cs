﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyEntry.DataAccess.EFCore.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tdCategory",
                columns: table => new
                {
                    CategoryId = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "varchar(128)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tdCategory", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "tdType",
                columns: table => new
                {
                    TypeId = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "varchar(128)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tdType", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "tePerson",
                columns: table => new
                {
                    PersonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(255)", nullable: true),
                    LastName = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tePerson", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "teTransaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    CategoryId = table.Column<byte>(nullable: true),
                    CreatedDt = table.Column<DateTime>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    PersonId = table.Column<int>(nullable: false),
                    Reconciled = table.Column<bool>(nullable: true),
                    RunningTotal = table.Column<decimal>(type: "money", nullable: true),
                    TransactionDesc = table.Column<string>(type: "varchar(128)", nullable: true),
                    TypeId = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teTransaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_teTransaction_tdCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "tdCategory",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_teTransaction_tePerson_PersonId",
                        column: x => x.PersonId,
                        principalTable: "tePerson",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teTransaction_tdType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "tdType",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "U_UniqueDescriptionCategory",
                table: "tdCategory",
                column: "Description",
                unique: true,
                filter: "[Description] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "U_UniqueDescriptionType",
                table: "tdType",
                column: "Description",
                unique: true,
                filter: "[Description] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "U_UniqueFirstNameLastNamePerson",
                table: "tePerson",
                columns: new[] { "FirstName", "LastName" },
                unique: true,
                filter: "[FirstName] IS NOT NULL AND [LastName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_teTransaction_CategoryId",
                table: "teTransaction",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_teTransaction_PersonId0",
                table: "teTransaction",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_teTransaction_TypeId",
                table: "teTransaction",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_teTransaction_PersonId",
                table: "teTransaction",
                columns: new[] { "CreatedDt", "TransactionId", "PersonId" });

            //Views

            //Procs
            migrationBuilder.Sql(
            "Create view vTrans " + 
            "as " +
            "select " +
            "	p.PersonID " +
            ",	p.FirstName + ' ' + p.LastName as Name	" +
            ",	t.Amount " +
            ",	t.TransactionDesc " +
            ",	ty.TypeID " +
            ",	ty.Description as [Type] " +
            ",	c.CategoryID " +
            ",	c.Description as [Category] " +
            ",	cast(t.CreatedDt as DATE) as CreatedDate " +
            ",	t.TransactionID " +
            ",	t.RunningTotal " +
            ",	t.reconciled " +
            "from teTransaction t (nolock)	" +
            "	join tdType ty (nolock)	 on t.TypeID = ty.TypeID " +
            "	join tdCategory c (nolock)	 on t.CategoryID = c.CategoryID " +
            "	join tePerson p (nolock)	on t.PersonID = p.PersonID"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Procs


            //Views
            migrationBuilder.Sql("drop view vTrans");

            //Tables
            migrationBuilder.DropTable(name: "teTransaction");
            migrationBuilder.DropTable(name: "tdCategory");
            migrationBuilder.DropTable(name: "tePerson");
            migrationBuilder.DropTable(name: "tdType");
        }
    }
}
