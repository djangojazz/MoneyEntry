using Controls.Enums;
using System;
using System.Xml.Serialization;

namespace MoneyEntry.Model
{
  [Serializable, XmlRoot("Input")]
  public class TransactionSummationByDurationInput
  {
    public TransactionSummationByDurationInput()
    { 
    }

    public TransactionSummationByDurationInput(int personId, DateTime start, DateTime end, GroupingFrequency grouping, decimal floor, bool summarize, params int[] categories)
    {
      PersonId = personId;
      Start = start.Date;
      End = end.Date;
      Grouping = grouping;
      Floor = floor;
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
    public GroupingFrequency Grouping { get; set; }
    [XmlAttribute]
    public bool Summarize { get; set; }
    [XmlAttribute]
    public decimal Floor { get; set; }
    public int[] Categories { get; set; }
  }
}
