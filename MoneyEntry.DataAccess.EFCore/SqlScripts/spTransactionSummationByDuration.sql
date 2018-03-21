CREATE PROC spTransactionSummationByDuration @Xml XML 
AS  
                 
BEGIN 
                 
DECLARE  
  @PersonId INT 
, @Start DATE 
, @End DATE  
, @Grouping VARCHAR(64) 
, @Summarize BIT 
, @Floor money 
, @Sql NVARCHAR(Max) =  
'SELECT CategoryId, CategoryDesc, ''{Grouping}'', {Grouping}, ROW_NUMBER() OVER(Partition by CategoryId Order by {Grouping}), SUM(Amount) as Amount  
From #Data  
GROUP BY CategoryId, CategoryDesc, {Grouping} 
' 
                 
SET FMTONLY OFF; 
                 
DROP TABLE IF EXISTS #Data; 
                 
CREATE TABLE #Data 
    ( 
      TransactionId	INT 
    , Amount		MONEY 
    , CategoryId	TINYINT 
    , CategoryDesc	VARCHAR(128) 
    , Day			DATE 
    , Week			DATE 
    , Month			DATE 
    , Quarter		DATE 
    , Year			DATE 
    ) 
                 
Declare @Results Table 
    ( 
      CategoryId	TINYINT 
    , CategoryName	VARCHAR(128) 
    , GroupName		VARCHAR(64) 
    , Grouping		DATE 
    , Position		INT 
    , Amount		MONEY 
    ) 
                 
SELECT 
  @PersonId = y.value('@PersonId', 'int') 
, @Start = y.value('@Start', 'DateTime')	 
, @End = y.value('@End', 'DateTime')	 
, @Grouping = y.value('@Grouping', 'varchar(64)') 
, @Summarize = y.value('@Summarize', 'bit') 
, @Floor = y.value('@Floor', 'money') 
FROM @Xml.nodes('/Input') AS x(y) 
                 
; WITH c AS  
    ( 
    SELECT 
        y.value('.', 'int') AS CategoryID 
    FROM @Xml.nodes('/Input/Categories/int') AS x(y) 
    ) 
INSERT INTO #Data 
SELECT  
  TransactionID 
, Amount 
, t.CategoryId 
, cat.Description AS CategoryDesc 
, CreatedDt  
, DATEADD(WEEK, DATEDIFF(WEEK, 0, CreatedDt), 0)  
, DATEADD(Month, DATEDIFF(MOnth, 0, CreatedDt), 0)  
, DATEADD(QUARTER, DATEDIFF(QUARTER, 0, CreatedDt), 0)  
, DATEADD(YEAR, DATEDIFF(YEAR, 0, CreatedDt), 0)  
From dbo.teTransaction t 
    INNER JOIN c ON t.CategoryID =  c.CategoryID 
        AND t.PersonID = @PersonId 
        AND CreatedDt BETWEEN @Start AND @End 
        AND t.Amount >= @Floor 
    INNER JOIN dbo.tdCategory cat ON cat.CategoryID = t.CategoryID 
ORDER BY t.CreatedDt, t.TransactionID 
                 
SELECT @Sql = REPLACE(@SQL, '{Grouping}', @Grouping) 
IF @Summarize = 1 
BEGIN 
    SELECT @SQL = REPLACE(@SQL, 'CategoryId, CategoryDesc, ', '') 
    SELECT @SQL = REPLACE(@SQL, 'SELECT', 'SELECT 0, ''Summary'', ')  
    SELECT @SQL = REPLACE(@SQL, 'Partition by CategoryId ', '') 
END 
                 
INSERT INTO @Results 
EXEC sp_executesql @SQL 
                 
Select * 
From @Results 
                 
DROP TABLE IF EXISTS #Data 
END