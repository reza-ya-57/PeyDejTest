using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models.Bases.Views;
using PeyDej.Models.Dtos;
using PeyDej.Models.Report;

namespace PeyDej.Controllers
{
    public class LoadingReportsController : Controller
    {
        private readonly PeyDejContext _context;

        public LoadingReportsController(PeyDejContext context)
        {
            _context = context;
        }

        // GET: LoadingReports
        public async Task<IActionResult> Index()
        {
            return View(await _context.LoadingReports.ToListAsync());
        }


        // GET: LoadingReports/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoadingReport loadingReport)
        {
            if (ModelState.IsValid)
            {
                _context.LoadingReports.Add(loadingReport);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(loadingReport);
        }

        // GET: LoadingReports/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loadingReport = await _context.LoadingReports.FindAsync(id);
            //var lodings = _context.LoadingReports.Where(w => w.Date == loadingReport.Date).ToList();

            if (loadingReport == null)
            {
                return NotFound();
            }
            return View(loadingReport);
        }

        // POST: LoadingReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, LoadingReport loadingReport)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    _context.LoadingReports.Update(loadingReport);
                    _context.SaveChanges();
               
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewBag.BladKind = BladKind();
            return View(loadingReport);
        }


        // POST: LoadingReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var loadingReport = await _context.LoadingReports.FindAsync(id);

            _context.LoadingReports.Remove(loadingReport);
            _context.SaveChanges();
            //return RedirectToAction(nameof(Index));
            return Json(new { r = true });
        }





        private bool LoadingReportExists(long id)
        {
            return (_context.LoadingReports?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
