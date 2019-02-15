Create proc spPostTransactionsToArchive
	(
		@TransactionIdToKeep int
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
where CreatedDt <= @LastCreatedDate
and PersonID = @PersonId
and TransactionId <> @TransactionIdToKeep
order by CreatedDt, TransactionID

delete teTransaction
where CreatedDt <= @LastCreatedDate
and PersonID = @PersonId
and TransactionId <> @TransactionIdToKeep

COMMIT TRANSACTION

Set xact_abort off
END