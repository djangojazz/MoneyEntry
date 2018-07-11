using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyEntry.ExpensesAPI
{
    public class BaseController: Controller
    {
        internal async Task<IActionResult> DetermineModelThenReturn(Func<object, Task<IActionResult>> method)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            return await method(null);
        }
    }
}
