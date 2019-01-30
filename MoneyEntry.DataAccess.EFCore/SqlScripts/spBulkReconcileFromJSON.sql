create proc spBulkReconcileFromJSON ( @json varchar(max))
as
BEGIN
with jdata as 
	(
	SELECT *  
	FROM OPENJSON(@json)  
	WITH (TransactionId int 'strict $.transactionId', Reconciled bit '$.reconciled')  
	)
update t
set t.Reconciled = j.Reconciled
from teTransaction t
	join jdata j on t.TransactionID = j.TransactionId

Select @@ROWCOUNT as RowsUpdated
END
GO