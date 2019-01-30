using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyEntry.DataAccess.EFCore.Migrations
{
    public partial class ViewsAndProcs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //locales
            migrationBuilder.Sql(@".\SqlScripts\spCategoryUseOverDuration.sql".ReadFile());
            migrationBuilder.Sql(@".\SqlScripts\spInsertOrUpdateTransaction.sql".ReadFile());
            migrationBuilder.Sql(@".\SqlScripts\spTransactionSummationByDuration.sql".ReadFile());
            migrationBuilder.Sql(@".\SqlScripts\spUpdateTotals.sql".ReadFile());
            migrationBuilder.Sql(@".\SqlScripts\spBulkReconcileFromJSON.sql".ReadFile());
            migrationBuilder.Sql(@".\SqlScripts\vTrans.sql".ReadFile());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Procs
            migrationBuilder.Sql("drop proc spUpdateTotals");
            migrationBuilder.Sql("drop proc spTransactionSummationByDuration");
            migrationBuilder.Sql("drop proc spInsertOrUpdateTransaction");
            migrationBuilder.Sql("drop proc spCategoryUseOverDuration");
            migrationBuilder.Sql("drop proc spBulkReconcileFromJSON");

            //Views
            migrationBuilder.Sql("drop view vTrans");
        }
    }
}
