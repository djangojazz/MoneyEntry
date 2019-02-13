create proc spPostTransactionsToArchive
	(
		@TransactionIdStarting int
	,	@LastCreatedDate date
	,	@PersonId int
	)
as 
BEGIN

Set xact_abort on

BEGIN TRANSACTION

Insert into teTransactionArchive
Select  *
from teTransaction
where TransactionID < @TransactionIdStarting
and CreatedDt <= @LastCreatedDate
and PersonID = @PersonId
order by CreatedDt, TransactionID

delete teTransaction
where TransactionID < @TransactionIdStarting
and CreatedDt <= @LastCreatedDate
and PersonID = @PersonId

COMMIT TRANSACTION

Set xact_abort off
END