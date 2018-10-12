using Microsoft.AspNetCore.Mvc;
using MoneyEntry.ExpensesAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyEntry.ExpensesAPI
{
    public abstract class BaseController: Controller
    {
        internal IJWTService JwtService;

        public BaseController(IJWTService jwt)
        {
            JwtService = jwt;
        }

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
