using ExpensesAPI.Models;
using MoneyEntry.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExpensesAPI.Controllers
{
    public class ExpensesController : ApiController
    {
        private DataRetreival _dataRetreival;

        public ExpensesController()
        {
            _dataRetreival = DataRetreival.Instance;
        }

        [Route("api/expenses/{start}/{end}/{personId}")]
        public async Task<IEnumerable<vTrans>> Get(DateTime start, DateTime end, int personId) =>
            await _dataRetreival.GetTransactionViewsAsync(start, end, personId);
        
        
        // POST api/categories
        public async Task<IHttpActionResult> Post([FromBody]WebTransaction tran)
        {
            if(!ModelState.IsValid) { return  BadRequest(); }

            try
            {
                var tranId = await _dataRetreival.InsertOrUpdateTransactionAsync(tran.TransactionId, tran.Amount, tran.Description, tran.TypeId, tran.CategoryId, tran.CreatedDate, tran.PersonId, tran.Reconciled);
                return Ok(tranId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}