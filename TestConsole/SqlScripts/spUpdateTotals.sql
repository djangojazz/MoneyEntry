CREATE PROC spUpdateTotals 
as  
begin  
    set nocount on; 
                 
    if OBJECT_ID('tempdb..#math') is not null drop table #math; 
    if OBJECT_ID('tempdb..#updates') is not null drop table #updates; 
                		 
    select  
        a.TransactionID	 
    ,	a.PersonID 
    ,	a.TypeID 
    ,	a.Amount 
    ,	a.CreatedDt 
    ,	a.RunningTotal 
    ,	cast(LEFT(CONVERT(varchar, CreatedDt, 112),14) as varchar(16)) + '.' +  
        cast(  ROW_NUMBER() over(Partition by CreatedDt order by TransactionID)  as varchar(16) ) as fl 
    ,	ROW_NUMBER() over(Partition by PersonId order by CreatedDt, TransactionID) as rwn 
    into #math 
    from dbo.teTransaction a (nolock) 
    Order by CreatedDt, TransactionID 
                 
    ; with x as  
        ( 
        select  
            TransactionID 
        ,	Amount 
        ,	Sum(Case when TypeId = 2 then cast('-' + cast(Amount as varchar(16)) as varchar(16)) Else Amount End) Over(Partition by PersonId Order By rwn) as RunningTotal 
        from #math 
        ) 
    select  
        x.TransactionID 
    ,	x.PersonID 
    ,	x.TypeID 
    ,	x.Amount 
    ,	x.CreatedDt 
    ,	y.RunningTotal 
    into #updates 
    from #math x 
        left join x y on x.TransactionID = y.TransactionID 
    Order by x.CreatedDt, x.TransactionId 
                 
    update dbo.teTransaction 
    set 
        RunningTotal = u.RunningTotal 
    from #updates u 
    where TeTransaction.TransactionID = u.TransactionID 
                		 
    Select @@ROWCOUNT as RowsUpdated 
end