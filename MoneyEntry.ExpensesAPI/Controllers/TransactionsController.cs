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
    [Route("api/Transactions")]
    public class TransactionsController : Controller
    {
        ExpensesRepository _repo = ExpensesRepository.Instance;

        //Optional params as needed
        [HttpGet("{personId?}/{start?}/{end?}")]
        public async Task<IActionResult> Get(int personId = 1, DateTime? start = null, DateTime? end = null) =>
            Ok((await _repo.GetTransactionViewsAsync(start ?? DateTime.Now.Date.AddMonths(-3), end ?? DateTime.Now, personId)).ToList());
        
        // POST: api/Transactions
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TransactionModel t)
        {
            if(ModelState.IsValid)
            {
                return Ok(await _repo.InsertOrUpdaTeTransactionAsync(t.TransactionId, t.Amount, t.Description, t.TypeId, t.CategoryId, t.CreatedDate, t.PersonId, t.Reconciled));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }   
    }
}
