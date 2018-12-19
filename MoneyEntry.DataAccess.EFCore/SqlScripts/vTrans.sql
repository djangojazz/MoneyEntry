Create view vTrans
as
select
	p.PersonID
,	p.FirstName + ' ' + p.LastName as Name
,	t.Amount
,	t.TransactionDesc as Description
,	ty.TypeID
,	ty.Description as [Type]
,	c.CategoryID
,	c.Description as [Category]
,	cast(t.CreatedDt as DATE) as CreatedDate
,	t.TransactionID
,	t.RunningTotal
,	t.Reconciled
from teTransaction t (nolock)
	join tdType ty (nolock)	 on t.TypeID = ty.TypeID
	join tdCategory c (nolock) on t.CategoryID = c.CategoryID
	join tePerson p (nolock) on t.PersonID = p.PersonID