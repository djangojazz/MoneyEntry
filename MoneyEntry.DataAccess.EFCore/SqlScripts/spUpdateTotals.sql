Create PROC spUpdateTotals
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
    ,	ROW_NUMBER() over(Partition by PersonId order by CreatedDt, TransactionID) as rwn 
    into #math 
    from dbo.teTransaction a (nolock) 
                 
    ; with x as  
        ( 
        select  
            rwn
		 ,	personId
		 ,	(select min(RunningTotal) from #math i where i.PersonId = o.PersonId and i.rwn = 1) as FirstRunningTotal
         ,	Case when rwn = 1 then RunningTotal else
				 Sum(Case when TypeId = 2  then cast('-' + cast(Amount as varchar(16)) as varchar(16)) Else Amount End) Over(Partition by PersonId Order By rwn) 
				end as Op 
        from #math o
		where rwn <> 1
        ) 
    select  
        x.TransactionID 
    ,	x.PersonID 
    ,	x.TypeID 
    ,	x.Amount 
    ,	x.CreatedDt 
    ,	ISNULL(y.FirstRunningTotal + y.Op, x.RunningTotal) as RunningTotal
    into #updates 
    from #math x 
        left join x y on x.PersonId = y.PersonId and x.rwn = y.rwn
    Order by x.CreatedDt, x.TransactionId 
                 
    update dbo.teTransaction 
    set 
        RunningTotal = u.RunningTotal 
    from #updates u 
    where TeTransaction.TransactionID = u.TransactionID 
                		 
    Select @@ROWCOUNT as RowsUpdated 
end
