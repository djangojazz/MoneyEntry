using MoneyEntry.DataAccess;


namespace MoneyEntry.Model
{
  public class Person
  {
    public int PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }

    public Person(vTrans tran)
    {
      PersonId = tran.PersonID;
      FullName = tran.Name;
    }

    public Person(tePerson person)
    {
      PersonId = person.PersonId;
      FirstName = person.FirstName;
      LastName = person.LastName;
      FullName = $"{person.FirstName} {person.LastName}";
    }
  }
}
