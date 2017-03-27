using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MoneyEntry.Model
{
  [Serializable]
  public class TransactionSummationByDurationInput
  {
    public TransactionSummationByDurationInput()
    { 
    }

    public TransactionSummationByDurationInput(int personId, DateTime start, DateTime end, Frequency grouping, bool summarize, params int[] categories)
    {
      PersonId = personId;
      Start = start.Date;
      End = end.Date;
      Grouping = grouping;
      Summarize = summarize;
      Categories = categories;
    }

    [XmlAttribute]
    public int PersonId { get; set; }
    [XmlAttribute]
    public DateTime Start { get; set; }
    [XmlAttribute]
    public DateTime End { get; set; }
    [XmlAttribute]
    public Frequency Grouping { get; set; }
    [XmlAttribute]
    public bool Summarize { get; set; }
    public int[] Categories { get; set; }
  }
}
