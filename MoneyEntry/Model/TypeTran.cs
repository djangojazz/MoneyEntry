using MoneyEntry.DataAccess;

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

    public override string ToString() => TypeName;
  }
}