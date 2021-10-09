using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZADALKHAIR.Data;

namespace ZADALKHAIR.Controllers
{
    [Route("{Controller}")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ZADALKHAIRContext _context;

        public AdminController(ZADALKHAIRContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            TempData["EmployeesCount"] = _context.User.Count();

            return View();
        }
    }
}
