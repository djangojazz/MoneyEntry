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

        public async Task<IEnumerable<vTrans>> Get() => await _dataRetreival.GetTransactionViewsAsync();


        // POST api/categories
        public async Task Post([FromBody]string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                await _dataRetreival.AddCategoryAsync(value);
            }
        }
    }
}
