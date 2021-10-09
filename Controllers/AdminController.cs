using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZADALKHAIR.Controllers
{
    [Route("{Controller}")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        [Route("emptyData")]
        public PartialViewResult NoDataFound()
        {
            return PartialView("NoData");
        }
    }
}
