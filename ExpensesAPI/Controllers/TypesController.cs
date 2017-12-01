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
    public class TypesController : ApiController
    {
        private DataRetreival _dataRetreival;

        public TypesController()
        {
            _dataRetreival = DataRetreival.Instance;
        }

        // GET api/types
        public async Task<IEnumerable<tdType>> Get()
        {
            return await _dataRetreival.GetTypesAsync();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
