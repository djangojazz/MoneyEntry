using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MoneyEntry.DataAccess.EFCore;
using MoneyEntry.ExpensesAPI.Models;
using MoneyEntry.ExpensesAPI.Services;
using Newtonsoft.Json;

namespace MoneyEntry.ExpensesAPI.Controllers
{
    [Route("expensesApi/[controller]/[action]"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionsController : BaseController
    //Controller
    {
        ExpensesRepository _repo = ExpensesRepository.Instance;

        public TransactionsController(IJWTService jwt, JwtSecurityTokenHandler handler, IConfiguration configuration) : base(jwt, handler, configuration) { }

        [HttpGet]
        public async Task<IActionResult> TestUp()
        {
            var header = (string)Request.Headers["Authorization"];
            var token = header.Split(' ').Skip(1).First();
            var key = Config["Security:Tokens:key"];
            return Ok(await JwtService.GetUserDetailsFromJWTToken(token, JWTHandler, key));
        }

        //internal async Task<UserTokenModel> GetUserModelFromJWT() => await JwtService.GetUserDetailsFromJWTToken(
        //        ((string)Request.Headers["Authorization"]).Split(' ').Skip(1).First(),
        //        JWTHandler,
        //        Config["Security:Tokens:Key"]);

        [HttpGet]
        public async Task<IActionResult> EchoJwt() => Ok(await GetUserModelFromJWT());


        [HttpGet]
        public async Task<IActionResult> GetLastDate()
        {
            var person = await GetUserModelFromJWT();
            return Ok(await _repo.LastDateEnteredByPersonAsync(person.UserId));
        }

        [HttpGet]
        public async Task<IActionResult> GetDate() => Ok(await Task.Factory.StartNew(() => new DateTime(2018, 2, 1)));

        //Optional params as needed
        [HttpGet, Route("{start?}/{end?}")]
        public async Task<IActionResult> GetTransactions(DateTime? start = null, DateTime? end = null) =>
            await CheckPersonToProceed(async personId =>
            {
                var trans = await _repo.GetTransactionViewsAsync(start ?? DateTime.Now.Date.AddMonths(-3), end ?? DateTime.Now, personId);

                if (!trans.Any())
                    return NotFound("There were no results found for this time period");

                return Ok(trans);
            });

        // POST: api/Transactions
        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody]TransactionModel t) =>
         await DetermineModelToProceed(async () => await CheckPersonToProceed(async personId => Ok(await _repo.InsertOrUpdaTeTransactionAsync(t.TransactionId, t.Amount, t.Description, t.TypeId, t.CategoryId, t.CreatedDate, personId, t.Reconciled))));

        [HttpPost]
        public async Task<IActionResult> ReconcileTransactions([FromBody]TransactionReconcileModel[] trans) =>
            await CheckPersonToProceed(async personId => Ok(await _repo.ReconcileTransactionsAsync(JsonConvert.SerializeObject(trans))) );
    }
}
