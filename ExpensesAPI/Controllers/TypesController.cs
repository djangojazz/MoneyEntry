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
    }
}
