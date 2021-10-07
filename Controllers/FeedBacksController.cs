using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZADALKHAIR.Data;
using ZADALKHAIR.Models;

namespace ZADALKHAIR.Controllers
{
    public class FeedBacksController : Controller
    {
        private readonly ZADALKHAIRContext _context;

        public FeedBacksController(ZADALKHAIRContext context)
        {
            _context = context;
        }

        // GET: FeedBacks
        public async Task<IActionResult> Index()
        {
            return View(await _context.FeedBack.ToListAsync());
        }

        // GET: FeedBacks/Details/5
        [HttpGet]
        [Route("Admin/Feedback/Details/injuction/{id}")]
        public async Task<PartialViewResult> Details(int? id)
        {
            if (id == null)
            {
                return PageNotFound();
            }

            var feedBack = await _context.FeedBack
                .FirstOrDefaultAsync(m => m.FeedBackID == id);
            if (feedBack == null)
            {
                return PageNotFound();
            }

            return PartialView("_FeedbackDetails", feedBack);
        }

        // GET: FeedBacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FeedBacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeedBackID,Name,Email,PhoneNumber,CounrtyCode,Massege,FeedBackSatuts,CreatedAt,SatutsUpdate,Rate,Image")] FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedBack);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feedBack);
        }

        // GET: FeedBacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedBack = await _context.FeedBack.FindAsync(id);
            if (feedBack == null)
            {
                return NotFound();
            }
            return View(feedBack);
        }

        // POST: FeedBacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FeedBackID,Name,Email,PhoneNumber,CounrtyCode,Massege,FeedBackSatuts,CreatedAt,SatutsUpdate,Rate,Image")] FeedBack feedBack)
        {
            if (id != feedBack.FeedBackID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedBack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedBackExists(feedBack.FeedBackID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(feedBack);
        }

        // GET: FeedBacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedBack = await _context.FeedBack
                .FirstOrDefaultAsync(m => m.FeedBackID == id);
            if (feedBack == null)
            {
                return NotFound();
            }

            return View(feedBack);
        }

        // POST: FeedBacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedBack = await _context.FeedBack.FindAsync(id);
            _context.FeedBack.Remove(feedBack);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedBackExists(int id)
        {
            return _context.FeedBack.Any(e => e.FeedBackID == id);
        }

        [HttpGet]
        public PartialViewResult PageNotFound()
        {
            return PartialView("_PageNotFound");
        }
    }
}
