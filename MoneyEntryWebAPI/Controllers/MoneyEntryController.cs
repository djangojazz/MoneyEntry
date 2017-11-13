using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyEntry.DataAccess;

namespace MoneyEntryWebAPI.Controllers
{
    [Route("api/[controller]")]
    public sealed class MoneyEntryController : Controller
    {
        private DataRetreival _dataRetreival;

        public MoneyEntryController()
        {
            _dataRetreival = DataRetreival.Instance;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return _dataRetreival.GetTypes().Select(x => x.Description).ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
