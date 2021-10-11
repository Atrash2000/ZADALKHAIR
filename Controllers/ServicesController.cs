using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZADALKHAIR.Data;
using ZADALKHAIR.Models;

namespace ZADALKHAIR.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ZADALKHAIRContext _context;
        private readonly IWebHostEnvironment webhostenvironment;
        public ServicesController(ZADALKHAIRContext context, IWebHostEnvironment webhostenvironment)
        {
            _context = context;
            this.webhostenvironment = webhostenvironment;
        }
        // GET: Services
        public async Task<IActionResult> Index()
        {
            return View(await _context.Service.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Service
                .FirstOrDefaultAsync(m => m.ServiceID == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }
   
        // GET: Services/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceID,ServiceTitle,ServiceDiscription,features,ServiceStartingPrice,ServicePic")] Service service)
        {
            if (ModelState.IsValid)
            {
                service.CreatedAt = DateTime.Now;
                string profilePic = await UploadFile(service.ServicePic);
                service.ServiceImage = profilePic;
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

       

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Service.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        public async Task<string> UploadFile(IFormFile profilePic)
        {
            string fileName = null;

            if (profilePic != null)
            {
                string uploadDir = Path.Combine(webhostenvironment.WebRootPath, "images/servicePic");
                /*fileName = Guid.NewGuid().ToString() + "-" + profilePic.FileName;*/
                fileName = profilePic.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    profilePic.CopyTo(fileStream);
                }
            }
            return fileName;
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceID,ServiceTitle,ServiceDiscription,features,ServiceStartingPrice,ServicePic,ServiceImage")] Service service)
        {
            if (id != service.ServiceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //service.CreatedAt = DateTime.Now;
                    string profilePic = await UploadFile(service.ServicePic);
                    service.ServiceImage = profilePic;
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceID))
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
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Service
                .FirstOrDefaultAsync(m => m.ServiceID == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Service.FindAsync(id);
            _context.Service.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
            return _context.Service.Any(e => e.ServiceID == id);
        }
    }
}
