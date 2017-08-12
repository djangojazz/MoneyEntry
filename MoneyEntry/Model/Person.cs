using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
      PersonId = person.PersonID;
      FirstName = person.FirstName;
      LastName = person.LastName;
      FullName = $"{person.FirstName} {person.LastName}";
    }
  }
}
