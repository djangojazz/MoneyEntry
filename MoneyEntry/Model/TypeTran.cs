using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoneyEntry.Model
{
  public class TypeTran
  {
    public TypeTran(byte typeId, string typeName)
    {
      TypeId = typeId;
      TypeName = typeName;
    }

    public TypeTran(vTrans tran)
    {
      TypeId = tran.TypeID;
      TypeName = tran.Type;
    }

    public byte TypeId { get; set; }
    public string TypeName { get; set; }
  }
}