using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyEntry.DataAccess.EFCore;

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
        
        // GET: api/Transactions/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Transactions
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
