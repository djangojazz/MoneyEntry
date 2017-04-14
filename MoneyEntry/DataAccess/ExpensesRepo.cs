using Controls;
using MoneyEntry.Model;
using MoneyEntry.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyEntry.DataAccess
{
  public static class ExpensesRepo
  {
    private static ObservableCollection<MoneyEntryModelViewModel> _moneyEntryContainer;

    public static ObservableCollection<MoneyEntryModelViewModel> MoneyEntryContainer
    {
      get
      {
        if (_moneyEntryContainer == null) _moneyEntryContainer = new ObservableCollection<MoneyEntryModelViewModel>();
        return _moneyEntryContainer;
      }
      set
      {
        _moneyEntryContainer = value;
      }
    }

    private static List<MoneyEntryModel> GetMoneyEnts(DateTime start, DateTime end, int personId)
    {
      using (var context = new ExpensesEntities())
      {
        return context.vTrans.Where(d => d.CreatedDate >= start && d.CreatedDate <= end && d.PersonID == personId)
            .Select(m => new MoneyEntryModel()
            {
              PersonId = m.PersonID,
              Name = m.Name,
              TransactionId = m.TransactionID,
              Amount = Decimal.Round(m.Amount, 2),
              TransactionDesc = m.TransactionDesc,
              TypeName = m.Type,
              CategoryName = m.Category,
              CreatedDate = m.CreatedDate,
              RunningTotal = Decimal.Round(m.RunningTotal.Value, 2),
              Reconciled = m.reconciled
            }).ToList().OrderBy(d => d.CreatedDate).ToList();
      }


    }

    public static void Refresh(DateTime start, DateTime end, int personId)
    {
      List<MoneyEntryModel> mems = GetMoneyEnts(start, end, personId);
      MoneyEntryContainer.ClearAndAddRange(mems.ConvertAll(x => new MoneyEntryModelViewModel(x)));
    }
  }
}
