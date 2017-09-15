using System;
using MoneyEntry.DataAccess;


namespace MoneyEntry.Model
{
  //TODO: Unsure if this is needed or not.  Can combine TransactionView and MoneyEntryModel into a single one.
  public class TransactionView
  {
    public int TransactionID { get; set; }
    public string TransactionDesc { get; set; }
    public Person Person { get; set; }
    
    public TypeTran Type { get; set; }
    public Category Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime? CreatedDate { get; set; }
    
    public decimal? RunningTotal { get; set; }
    public bool? reconciled { get; set; }

    public TransactionView(Person person, TypeTran type, Category category, decimal amount, string transactionDesc, DateTime createdDate)
    {
      Person = person;
      Type = type;
      Category = category;
      Amount = amount;
      TransactionDesc = transactionDesc;
      CreatedDate = createdDate;
    }

    public TransactionView(vTrans dbTran)
    {
      TransactionID = dbTran.TransactionID;
      TransactionDesc = dbTran.TransactionDesc;
      Person = new Person(dbTran);
      Amount = dbTran.Amount;
      CreatedDate = dbTran.CreatedDate;
      RunningTotal = dbTran.RunningTotal;
      reconciled = dbTran.reconciled;
    }
  }
}
