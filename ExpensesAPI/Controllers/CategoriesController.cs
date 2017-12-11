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
    public class CategoriesController : ApiController
    {
        private DataRetreival _dataRetreival;

        public CategoriesController()
        {
            _dataRetreival = DataRetreival.Instance;
        }

        // GET api/categories
        public async Task<IEnumerable<tdCategory>> Get() => await _dataRetreival.GetCurrentCategoriesAsync();
        

        // POST api/categories
        public async Task Post([FromBody]string value)
        {
            if(!String.IsNullOrEmpty(value))
            {
                await _dataRetreival.AddCategoryAsync(value);
            }
        }
    }
}
