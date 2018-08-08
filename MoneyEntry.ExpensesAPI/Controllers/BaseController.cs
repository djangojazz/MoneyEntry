using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyEntry.ExpensesAPI
{
    public class BaseController: Controller
    {
        internal async Task<IActionResult> DetermineModelThenReturn(Func<Task<IActionResult>> method)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return await method();
            }
            catch (Exception ex)
            {
                string s = "Fatal Exception";
#if DEBUG
                s = ex.Message;
#endif
                return BadRequest(s);
            }
        }
    }
}
