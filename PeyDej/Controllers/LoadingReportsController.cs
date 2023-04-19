using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
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
              return _context.LoadingReports != null ? 
                          View(await _context.LoadingReports.ToListAsync()) :
                          Problem("Entity set 'PeyDejContext.LoadingReports'  is null.");
        }

        // GET: LoadingReports/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.LoadingReports == null)
            {
                return NotFound();
            }

            var loadingReport = await _context.LoadingReports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loadingReport == null)
            {
                return NotFound();
            }

            return View(loadingReport);
        }

        // GET: LoadingReports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoadingReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InsDate,Date,DayCaption,BladKindId,LoadingIntervalId,Description,OtherBladCaption")] LoadingReport loadingReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loadingReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loadingReport);
        }

        // GET: LoadingReports/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.LoadingReports == null)
            {
                return NotFound();
            }

            var loadingReport = await _context.LoadingReports.FindAsync(id);
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
        public async Task<IActionResult> Edit(long id, [Bind("Id,InsDate,Date,DayCaption,BladKindId,LoadingIntervalId,Description,OtherBladCaption")] LoadingReport loadingReport)
        {
            if (id != loadingReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(loadingReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoadingReportExists(loadingReport.Id))
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
            return View(loadingReport);
        }

        // GET: LoadingReports/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.LoadingReports == null)
            {
                return NotFound();
            }

            var loadingReport = await _context.LoadingReports
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loadingReport == null)
            {
                return NotFound();
            }

            return View(loadingReport);
        }

        // POST: LoadingReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.LoadingReports == null)
            {
                return Problem("Entity set 'PeyDejContext.LoadingReports'  is null.");
            }
            var loadingReport = await _context.LoadingReports.FindAsync(id);
            if (loadingReport != null)
            {
                _context.LoadingReports.Remove(loadingReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoadingReportExists(long id)
        {
          return (_context.LoadingReports?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
