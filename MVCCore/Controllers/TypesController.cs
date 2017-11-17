using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EF6;
using EF6.Models;
using MoneyEntry.DataAccess;

namespace ContosoUniversity.Controllers
{
    public class TypesController : Controller
    {
        private readonly ExpensesEntities _context;

        public TypesController(ExpensesEntities context)
        {
            _context = context;
        }

        // GET: Types0
        public async Task<IActionResult> Index()
        {
            var data = await _context.tdType.ToListAsync();
            return View(data);
        }
    }
}
