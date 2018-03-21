﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyEntry.DataAccess.EFCore;
using MoneyEntry.DataAccess.EFCore.Expenses.Models;
using MoneyEntry.ExpensesAPI.Models;

namespace MoneyEntry.ExpensesAPI.Controllers
{
    [Route("api/Categories")]
    public class CategoriesController : Controller
    {
        ExpensesRepository _repo = ExpensesRepository.Instance;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok((await _repo.GetCategoriesAsync()).ToList());
        
        
        // POST: api/Categories
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]string value)
        {
            await _repo.AddCategoryAsync(value);
            var results = (await _repo.GetCategoriesAsync()).ToList();

            if (results.Exists(x => x.Description == value))
            { 
                return Ok(results);
            }
            else
            {
                return BadRequest($"Could not create category {value}");
            }
        }
    }
}