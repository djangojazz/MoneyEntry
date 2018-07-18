using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyEntry.DataAccess.EFCore;
using MoneyEntry.ExpensesAPI.Models;

namespace MoneyEntry.ExpensesAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TransactionsController : BaseController
        //Controller
    {
        ExpensesRepository _repo = ExpensesRepository.Instance;

        [HttpGet, Route("{personId:int}")]
        public async Task<IActionResult> GetLastDate(int personId) => Ok(await _repo.LastDateEnteredByPersonAsync(personId));

        [HttpGet]
        public async Task<IActionResult> GetDate() => Ok(await Task.Factory.StartNew(() => new DateTime(2018, 2, 1)));

        //Optional params as needed
        [HttpGet, Route("{personId?}/{start?}/{end?}")]
        public async Task<IActionResult> GetTransactions(int personId = 1, DateTime? start = null, DateTime? end = null)
        {
            var trans = await _repo.GetTransactionViewsAsync(start ?? DateTime.Now.Date.AddMonths(-3), end ?? DateTime.Now, personId);

            if (!trans.Any())
               return NotFound("There were no results found for this time period");

            return Ok(trans);
        }
        
        // POST: api/Transactions
        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody]TransactionModel t) =>
         await DetermineModelThenReturn(async () => Ok(await _repo.InsertOrUpdaTeTransactionAsync(t.TransactionId, t.Amount, t.Description, t.TypeId, t.CategoryId, t.CreatedDate, t.PersonId, t.Reconciled)));   
    }
}
