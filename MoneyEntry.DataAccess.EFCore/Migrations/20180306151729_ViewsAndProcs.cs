﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MoneyEntry.DataAccess.EFCore.Migrations
{
    public partial class ViewsAndProcs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
            "	join tdCategory c (nolock) on t.CategoryID = c.CategoryID " +
            "	join tePerson p (nolock) on t.PersonID = p.PersonID"
            );

            //migrationBuilder.Sql(
            //"Create proc spInsertOrUpdateTransaction " +
            //"	( " +
            //"		@TransactionID	  INT  " +
            //"	,	@Amount			  MONEY  " +
            //"	,	@TransactionDesc  VARCHAR(128) " +
            //"	,	@TypeId		      INT  " +
            //"	,	@CategoryId		  INT " +
            //"	,	@CreatedDt		  DATETIME  " +
            //"	,	@PersonID	      INT	 " +
            //"    ,	@Reconciled		  BIT = 0 " +
            //"	) " +
            //"as " +
            //"begin " +
            //" SET NOCOUNT ON; " +
            //" declare @MaxInt int = -1-POWER(-2,31); " +
            //" Declare @Archive table (TranId int) " +
            //" Declare @Temp Table (TransactionId int, Amount Money, TransactionDesc Varchar(128), TypeId int, CategoryId int, CreatedDt Date, PersonId int, RunningTotal MONEY, Reconciled bit) " +
            //" Declare @Update Table (TransactionId int, Amount Money, TransactionDesc Varchar(128), TypeId int, CategoryId int, CreatedDt Date, PersonId int, RunningTotal MONEY, Reconciled bit) " +
            //" Declare @Previous Date " +
            //" DECLARE @StartAmount DECIMAL(14,2); " +
            //" " +
            //"	begin try " +
            //"		If @TransactionId > 0 AND Not Exists (Select 1 from tetransaction where TransactionId = @TransactionId) " +
            //"		BEGIN " +
            //"			Select -1; " +
            //"			return; " +
            //"		END " +
            //" " +
            //"		Select @Previous = MAX(CreatedDt) FROM teTransaction  WHERE PersonId = @PersonId and CreatedDt < @CreatedDt " +
            //"	 " +
            //"		INSERT INTO @Temp " +
            //"		SELECT  " +
            //"		  TransactionID " +
            //"		, Amount " +
            //"		, TransactionDesc " +
            //"		, TypeID " +
            //"		, CategoryID " +
            //"		, CreatedDt " +
            //"		, PersonID " +
            //"		, RunningTotal " +
            //"        , Reconciled " +
            //"		from dbo.teTransaction " +
            //"		WHERE PersonID = @PersonID " +
            //"			AND CreatedDt >= @Previous " +
            //"		ORDER BY CreatedDt, TransactionID " +
            //" " +
            //"		Merge @Temp as T " +
            //"		Using (Select @TransactionID AS TransactionId, @Amount AS Amount, @TransactionDesc AS TransactionDesc, @TypeId AS TypeId, @CategoryId AS CategoryId, @CreatedDt AS CreatedDt,  " +
            //"                @PersonId AS PersonId, 0 AS RunningTotal, @Reconciled AS Reconciled) AS S on T.TransactionId = S.TransactionId " +
            //"		When Matched Then  " +
            //"			Update Set " +
            //"				T.Amount = S.Amount " +
            //"			,	T.TransactionDesc = S.TransactionDesc " +
            //"			,	T.TypeId = S.TypeId " +
            //"			,	T.CategoryId = S.CategoryId " +
            //"			,	T.CreatedDt = S.CreatedDt " +
            //"			,	T.PersonId = S.PersonId " +
            //"			,   T.Reconciled = S.Reconciled " +
            //"		When Not Matched Then " +
            //"			Insert (TransactionId, Amount, TransactionDesc, TypeId, CategoryId, CreatedDt, PersonId, RunningTotal, Reconciled) " +
            //"			Values (S.TransactionId, S.Amount, S.TransactionDesc, S.TypeId, S.CategoryId, S.CreatedDt, S.PersonId, S.RunningTotal, S.Reconciled)  " +
            //"		; " +
            //"		SELECT TOP 1 @StartAmount = RunningTotal FROM @Temp ORDER BY CreatedDt, TransactionId;" +
            //" " +
            //"		with ord as  " +
            //"		( " +
            //"		SELECT * " +
            //"		--if this is a new transaction, 0, trick system to think it should come last in windowed function " +
            //"		, ROW_NUMBER() OVER(ORDER BY CreatedDt, CASE WHEN TransactionId = 0 THEN @MaxInt ELSE TransactionID END) AS rwn  " +
            //"		FROM @Temp " +
            //"		) " +
            //"		, math AS  " +
            //"		( " +
            //"		select * " +
            //"		,	SUM(Case when TypeId = 2 then cast('-' + cast(Amount as varchar(16)) as varchar(16)) Else Amount End) Over(Partition by PersonId Order By CreatedDt, CASE WHEN TransactionId = 0 THEN 9999999 ELSE TransactionID END) as Subtract " +
            //"		from ord " +
            //"		WHERE rwn <> (SELECT Min(rwn) FROM ord) " +
            //"		) " +
            //"		INSERT INTO @Update " +
            //"		SELECT " +
            //"			x.TransactionID " +
            //"		,	x.Amount " +
            //"		,	x.TransactionDesc " +
            //"		,	x.TypeId " +
            //"		,	x.CategoryId " +
            //"		,	x.CreatedDt " +
            //"		,	x.PersonId " +
            //"		,	@StartAmount + ISNULL(Subtract,0) AS RunningTotal " +
            //"       , x.Reconciled " +
            //"		From ord x " +
            //"			LEFT JOIN	math z ON x.rwn = z.rwn " +
            //"		ORDER BY x.rwn " +
            //" " +
            //"		MERGE dbo.teTransaction AS T " +
            //"		USING @Update AS S ON t.TransactionID = S.TransactionId AND T.PersonID = S.PersonId " +
            //"		WHEN MATCHED THEN  " +
            //"			UPDATE SET " +
            //"				T.Amount = S.Amount " +
            //"			,	T.TransactionDesc = S.TransactionDesc " +
            //"			,	T.TypeId = S.TypeId " +
            //"			,	T.CategoryId = S.CategoryId " +
            //"			,	T.CreatedDt = S.CreatedDt " +
            //"			,	T.ModifiedDt = GETDATE() " +
            //"			,	T.RunningTotal = S.RunningTotal " +
            //"			, T.Reconciled = S.Reconciled " +
            //"		WHEN NOT MATCHED THEN " +
            //"			INSERT (Amount, TransactionDesc, TypeID, CategoryID, CreatedDt, ModifiedDt, reconciled, RunningTotal, PersonID) " +
            //"			VALUES (S.Amount, S.TransactionDesc, S.TypeId, S.CategoryId, S.CreatedDt, GETDATE(), 0, S.RunningTotal, S.PersonId) " +
            //"		OUTPUT inserted.TransactionId " +
            //"		INTO @archive; " +
            //"		; " +
            //" " +
            //"		select Case when @TransactionId > 0 then @TransactionId else (select top 1 TranId from @archive) end " +
            //" " +
            //"		END TRY " +
            //"        BEGIN CATCH " +
            //"        END Catch " +
            //"end ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Procs
            //migrationBuilder.Sql("drop proc spInsertOrUpdateTransaction");

            //Views
            migrationBuilder.Sql("drop view vTrans");
        }
    }
}
