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
    #region snippet_ContextInController
    public class TypesController : Controller
    {
        private readonly ExpensesEntities _context;

        public TypesController(ExpensesEntities context)
        {
            _context = context;
        }
        #endregion
        // GET: Types
        public async Task<IActionResult> Index()
        {
            return View(await _context.tdType.ToListAsync());
        }
    }
}
