using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZADALKHAIR.Data;
using ZADALKHAIR.Models;

namespace ZADALKHAIR.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ZADALKHAIRContext _context;
        private readonly FeedBacksController feedBacksController;

        public HomeController(ILogger<HomeController> logger, ZADALKHAIRContext context)
        {
            _logger = logger;
            _context = context;
            feedBacksController = new FeedBacksController(context);
        }

        public async Task<IActionResult> Index()
        {
            ViewData["feedback"] = await _context.FeedBack.Where(f => f.FeedBackSatuts == true).ToListAsync();
            /*ViewData["feedbackCount"] = _context.FeedBack.Where(f => f.FeedBackSatuts == true).Count();*/
            ViewBag.feedbackCount = _context.FeedBack.Where(f => f.FeedBackSatuts == true).Count();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Route("Service List")]
        [HttpGet]
        public async Task<IActionResult> ServiceView()
        {
            return View(await _context.Service.ToListAsync());
        }
        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
