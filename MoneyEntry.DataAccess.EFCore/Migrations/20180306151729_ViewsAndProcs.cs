using Microsoft.EntityFrameworkCore.Metadata;
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

            migrationBuilder.Sql(
            "CREATE PROC spCategoryUseOverDuration " +
            "	( " +
            "	  @Start DATE  " +
            "	, @End DATE  " +
            "	, @TypeId INT = 2 " +
            "	, @PersonId INT  " +
            "	, @Minimum MONEY " +
            "	) " +
            "AS  " +
            "BEGIN " +
            "	SELECT " +
            "	  t.CategoryID " +
            "	, t.TypeID " +
            "	, ty.Description AS TypeDesc " +
            "	, c.Description AS CategoryDesc " +
            "	, SUM(t.Amount) AS Amount " +
            "	FROM teTransaction t " +
            "		INNER JOIN tdCategory c ON c.CategoryID = t.CategoryID " +
            "			AND t.CreatedDt BETWEEN @Start AND @End " +
            "			AND t.PersonID = @PersonId " +
            "			AND t.TypeID = ISNULL(@TypeId, t.TypeID) " +
            "		INNER JOIN tdType ty ON ty.TypeID = t.TypeID " +
            "	GROUP BY t.TypeID, ty.Description, t.CategoryID, c.Description " +
            "	HAVING SUM(t.Amount) > @Minimum " +
            "	ORDER BY SUM(t.Amount) DESC " +
            "END ");

            migrationBuilder.Sql(
            "Create PROC spInsertOrUpdateTransaction " +
            "	( " +
            "		@TransactionID	  INT  " +
            "	,	@Amount			  MONEY  " +
            "	,	@TransactionDesc  VARCHAR(128) " +
            "	,	@TypeId		      INT  " +
            "	,	@CategoryId		  INT " +
            "	,	@CreatedDt		  DATETIME  " +
            "	,	@PersonID	      INT	 " +
            "    ,	@Reconciled		  BIT = 0 " +
            "	) " +
            "as " +
            " " +
            "begin " +
            "    SET NOCOUNT ON; " +
            "	declare @MaxInt int = -1-POWER(-2,31); " +
            "	Declare @Archive table (TranId int) " +
            "	Declare @Temp Table (TransactionId int, Amount Money, TransactionDesc Varchar(128), TypeId int, CategoryId int, CreatedDt Date, PersonId int, RunningTotal MONEY, Reconciled bit) " +
            "	Declare @Update Table (TransactionId int, Amount Money, TransactionDesc Varchar(128), TypeId int, CategoryId int, CreatedDt Date, PersonId int, RunningTotal MONEY, Reconciled bit) " +
            "	Declare @Previous Date " +
            "	DECLARE @StartAmount DECIMAL(14,2); " +
            " " +
            "	begin try " +
            "		if Not Exists(Select 1 from teTransaction) " +
            "		BEGIN " +
            "			Insert into teTransaction (Amount, TransactionDesc, TypeId, CategoryId, CreatedDt, ModifiedDt, PersonId, RunningTotal, Reconciled) " +
            "			Values (@Amount, @TransactionDesc, @TypeId, @CategoryId, @CreatedDt, getdate(), @PersonId, Case when @TypeId = 2 then -@Amount else @Amount end, @Reconciled)  " +
            "			Select top 1 TransactionId from tetransaction; " +
            "			return; " +
            "		END " +
            " " +
            "		If @TransactionId > 0 AND Not Exists (Select 1 from teTransaction where TransactionId = @TransactionId) " +
            "		BEGIN " +
            "			Select -1; " +
            "			return; " +
            "		END " +
            " " +
            "		 " +
            "		Select @Previous = MAX(CreatedDt) FROM teTransaction  WHERE PersonId = @PersonId and CreatedDt < @CreatedDt " +
            "		 " +
            "		INSERT INTO @Temp " +
            "		SELECT top 1 " +
            "			TransactionID " +
            "		, Amount " +
            "		, TransactionDesc " +
            "		, TypeID " +
            "		, CategoryID " +
            "		, CreatedDt " +
            "		, PersonID " +
            "		, RunningTotal " +
            "		, Reconciled " +
            "		from dbo.teTransaction " +
            "		WHERE PersonID = @PersonID " +
            "			AND CreatedDt >= ISNULL(@Previous, @CreatedDt) " +
            "		ORDER BY CreatedDt desc, TransactionID desc " +
            "	 " +
            "		Merge @Temp as T " +
            "		Using (Select @TransactionID AS TransactionId, @Amount AS Amount, @TransactionDesc AS TransactionDesc, @TypeId AS TypeId, @CategoryId AS CategoryId, @CreatedDt AS CreatedDt,  " +
            "                @PersonId AS PersonId, 0 AS RunningTotal, @Reconciled AS Reconciled) AS S on T.TransactionId = S.TransactionId " +
            "		When Matched Then  " +
            "			Update Set " +
            "				T.Amount = S.Amount " +
            "			,	T.TransactionDesc = S.TransactionDesc " +
            "			,	T.TypeId = S.TypeId " +
            "			,	T.CategoryId = S.CategoryId " +
            "			,	T.CreatedDt = S.CreatedDt " +
            "			,	T.PersonId = S.PersonId " +
            "			,   T.Reconciled = S.Reconciled " +
            "		When Not Matched Then " +
            "			Insert (TransactionId, Amount, TransactionDesc, TypeId, CategoryId, CreatedDt, PersonId, RunningTotal, Reconciled) " +
            "			Values (S.TransactionId, S.Amount, S.TransactionDesc, S.TypeId, S.CategoryId, S.CreatedDt, S.PersonId, S.RunningTotal, S.Reconciled)  " +
            "		; " +
            " " +
            "		SELECT TOP 1 @StartAmount = RunningTotal FROM @Temp ORDER BY CreatedDt, TransactionId desc " +
            "		 " +
            " " +
            "		; with ord as  " +
            "		( " +
            "		SELECT  " +
            "			* " +
            "		/* if this is a new transaction, 0, trick system to think it should come last in windowed function */ " +
            "		, ROW_NUMBER() OVER(ORDER BY CreatedDt, CASE WHEN TransactionId = 0 THEN @MaxInt ELSE TransactionID END) AS rwn  " +
            "		FROM @Temp " +
            "		) " +
            "		, math AS  " +
            "		( " +
            "		select  " +
            "			* " +
            "		,	SUM(Case when TypeId = 2 then cast('-' + cast(Amount as varchar(16)) as varchar(16)) Else Amount End) Over(Partition by PersonId Order By CreatedDt, CASE WHEN TransactionId = 0 THEN 9999999 ELSE TransactionID END) as Subtract " +
            "		from ord " +
            "		WHERE rwn <> (SELECT Min(rwn) FROM ord) " +
            "		) " +
            "		INSERT INTO @Update " +
            "		SELECT " +
            "			x.TransactionID " +
            "		,	x.Amount " +
            "		,	x.TransactionDesc " +
            "		,	x.TypeId " +
            "		,	x.CategoryId " +
            "		,	x.CreatedDt " +
            "		,	x.PersonId " +
            "		,	@StartAmount + ISNULL(Subtract,0) AS RunningTotal " +
            "		,	x.Reconciled " +
            "		From ord x " +
            "			LEFT JOIN	math z ON x.rwn = z.rwn " +
            "		ORDER BY x.rwn " +
            "		 " +
            "		MERGE dbo.teTransaction AS T " +
            "		USING @Update AS S ON t.TransactionID = S.TransactionId AND T.PersonID = S.PersonId " +
            "		WHEN MATCHED THEN  " +
            "			UPDATE SET " +
            "				T.Amount = S.Amount " +
            "			,	T.TransactionDesc = S.TransactionDesc " +
            "			,	T.TypeId = S.TypeId " +
            "			,	T.CategoryId = S.CategoryId " +
            "			,	T.CreatedDt = S.CreatedDt " +
            "			,	T.ModifiedDt = GETDATE() " +
            "			,	T.RunningTotal = S.RunningTotal " +
            "			, T.Reconciled = S.Reconciled " +
            "		WHEN NOT MATCHED THEN " +
            "			INSERT (Amount, TransactionDesc, TypeID, CategoryID, CreatedDt, ModifiedDt, reconciled, RunningTotal, PersonID) " +
            "			VALUES (S.Amount, S.TransactionDesc, S.TypeId, S.CategoryId, S.CreatedDt, GETDATE(), 0, S.RunningTotal, S.PersonId) " +
            "		OUTPUT inserted.TransactionId " +
            "		INTO @archive; " +
            "		; " +
            " " +
            "		select Case when @TransactionId > 0 then @TransactionId else (select top 1 TranId from @archive) end as TransactionId " +
            " " +
            "		END TRY " +
            "        BEGIN CATCH " +
            "        END Catch " +
            "end ");

            migrationBuilder.Sql(
                "CREATE PROC spTransactionSummationByDuration @Xml XML " +
                "AS  " +
                " " +
                "BEGIN " +
                " " +
                "DECLARE  " +
                "  @PersonId INT " +
                ", @Start DATE " +
                ", @End DATE  " +
                ", @Grouping VARCHAR(64) " +
                ", @Summarize BIT " +
                ",	@Floor money " +
                ", @Sql NVARCHAR(Max) =  " +
                "'SELECT CategoryId, CategoryDesc, ''{Grouping}'', {Grouping}, ROW_NUMBER() OVER(Partition by CategoryId Order by {Grouping}), SUM(Amount) as Amount  " +
                "From #Data  " +
                "GROUP BY CategoryId, CategoryDesc, {Grouping} " +
                "' " +
                " " +
                "SET FMTONLY OFF; " +
                " " +
                "DROP TABLE IF EXISTS #Data; " +
                " " +
                "CREATE TABLE #Data " +
                "	( " +
                "	  TransactionId	INT " +
                "	, Amount		MONEY " +
                "	, CategoryId	TINYINT " +
                "	, CategoryDesc	VARCHAR(128) " +
                "	, Day			DATE " +
                "	, Week			DATE " +
                "	, Month			DATE " +
                "	, Quarter		DATE " +
                "	, Year			DATE " +
                "	) " +
                " " +
                "Declare @Results Table " +
                "	( " +
                "	  CategoryId	TINYINT " +
                "	, CategoryName	VARCHAR(128) " +
                "	, GroupName		VARCHAR(64) " +
                "	, Grouping		DATE " +
                "	, Position		INT " +
                "	, Amount		MONEY " +
                "	) " +
                " " +
                "SELECT " +
                "  @PersonId = y.value('@PersonId', 'int') " +
                ", @Start = y.value('@Start', 'DateTime')	 " +
                ", @End = y.value('@End', 'DateTime')	 " +
                ", @Grouping = y.value('@Grouping', 'varchar(64)') " +
                ", @Summarize = y.value('@Summarize', 'bit') " +
                ", @Floor = y.value('@Floor', 'money') " +
                "FROM @Xml.nodes('/Input') AS x(y) " +
                " " +
                "; WITH c AS  " +
                "	( " +
                "	SELECT " +
                "	  y.value('.', 'int') AS CategoryID " +
                "	FROM @Xml.nodes('/Input/Categories/int') AS x(y) " +
                "	) " +
                "INSERT INTO #Data " +
                "SELECT  " +
                "  TransactionID " +
                ", Amount " +
                ", t.CategoryId " +
                ",	cat.Description AS CategoryDesc " +
                ", CreatedDt  " +
                ", DATEADD(WEEK, DATEDIFF(WEEK, 0, CreatedDt), 0)  " +
                ", DATEADD(Month, DATEDIFF(MOnth, 0, CreatedDt), 0)  " +
                ", DATEADD(QUARTER, DATEDIFF(QUARTER, 0, CreatedDt), 0)  " +
                ", DATEADD(YEAR, DATEDIFF(YEAR, 0, CreatedDt), 0)  " +
                "From dbo.teTransaction t " +
                "	INNER JOIN c ON t.CategoryID =  c.CategoryID " +
                "		AND t.PersonID = @PersonId " +
                "		AND CreatedDt BETWEEN @Start AND @End " +
                "		AND t.Amount >= @Floor " +
                "	INNER JOIN dbo.tdCategory cat ON cat.CategoryID = t.CategoryID " +
                "ORDER BY t.CreatedDt, t.TransactionID " +
                " " +
                "SELECT @Sql = REPLACE(@SQL, '{Grouping}', @Grouping) " +
                "IF @Summarize = 1 " +
                "BEGIN " +
                "    SELECT @SQL = REPLACE(@SQL, 'CategoryId, CategoryDesc, ', '') " +
                "	SELECT @SQL = REPLACE(@SQL, 'SELECT', 'SELECT 0, ''Summary'', ')  " +
                "	SELECT @SQL = REPLACE(@SQL, 'Partition by CategoryId ', '') " +
                "END " +
                " " +
                "INSERT INTO @Results " +
                "EXEC sp_executesql @SQL " +
                " " +
                "Select * " +
                "From @Results " +
                " " +
                "DROP TABLE IF EXISTS #Data " +
                "END "
                );

            migrationBuilder.Sql(
                "CREATE PROC spUpdateTotals " +
                "as  " +
                "begin  " +
                "	set nocount on; " +
                " " +
                "	if OBJECT_ID('tempdb..#math') is not null drop table #math; " +
                "	if OBJECT_ID('tempdb..#updates') is not null drop table #updates; " +
                "		 " +
                "	select  " +
                "		a.TransactionID	 " +
                "	,	a.PersonID " +
                "	,	a.TypeID " +
                "	,	a.Amount " +
                "	,	a.CreatedDt " +
                "	,	a.RunningTotal " +
                "	,	cast(LEFT(CONVERT(varchar, CreatedDt, 112),14) as varchar(16)) + '.' +  " +
                "		cast(  ROW_NUMBER() over(Partition by CreatedDt order by TransactionID)  as varchar(16) ) as fl " +
                "	,	ROW_NUMBER() over(Partition by PersonId order by CreatedDt, TransactionID) as rwn " +
                "	into #math " +
                "	from dbo.teTransaction a (nolock) " +
                "	Order by CreatedDt, TransactionID " +
                " " +
                "	; with x as  " +
                "		( " +
                "		select  " +
                "			TransactionID " +
                "		,	Amount " +
                "		,	Sum(Case when TypeId = 2 then cast('-' + cast(Amount as varchar(16)) as varchar(16)) Else Amount End) Over(Partition by PersonId Order By rwn) as RunningTotal " +
                "		from #math " +
                "		) " +
                "	select  " +
                "		x.TransactionID " +
                "	,	x.PersonID " +
                "	,	x.TypeID " +
                "	,	x.Amount " +
                "	,	x.CreatedDt " +
                "	,	y.RunningTotal " +
                "	into #updates " +
                "	from #math x " +
                "		left join x y on x.TransactionID = y.TransactionID " +
                "	Order by x.CreatedDt, x.TransactionId " +
                " " +
                "	update dbo.teTransaction " +
                "	set " +
                "		RunningTotal = u.RunningTotal " +
                "	from #updates u " +
                "	where TeTransaction.TransactionID = u.TransactionID " +
                "		 " +
                "	Select @@ROWCOUNT as RowsUpdated " +
                "end "
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //Procs
            migrationBuilder.Sql("drop proc spUpdateTotals");
            migrationBuilder.Sql("drop proc spTransactionSummationByDuration");
            migrationBuilder.Sql("drop proc spInsertOrUpdateTransaction");
            migrationBuilder.Sql("drop proc spCategoryUseOverDuration");

            //Views
            migrationBuilder.Sql("drop view vTrans");
        }
    }
}
