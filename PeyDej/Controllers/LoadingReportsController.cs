using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
using PeyDej.Models.Bases.Views;
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
        
        private IEnumerable<object> BladKind()
        {
            var data = _context.VwCategories.Where(m => m.CategoryId == 7).ToList();
            data.Add(new Categories()
            {
                SubCategoryCaption = "متفرقه",
                SubCategoryId = 0,
                CategoryId = 0,
                CategoryCaption = "",
            });
            return new SelectList(data, "SubCategoryId",
                "SubCategoryCaption");
        }
        
        private IEnumerable<object> LoadingInterval()
        {
            return new SelectList(_context.VwCategories.Where(m => m.CategoryId == 8).ToList(), "SubCategoryId",
                "SubCategoryCaption");
        }


        // GET: LoadingReports/Create
        public IActionResult Create()
        {
            ViewBag.BladKind = BladKind();
            ViewBag.LoadingInterval = LoadingInterval();
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
            ViewBag.BladKind = BladKind();
            ViewBag.LoadingInterval = LoadingInterval();
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
            if (loadingReport == null)
            {
                return NotFound();
            }
            ViewBag.BladKind = BladKind();
            ViewBag.LoadingInterval = LoadingInterval();
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
            ViewBag.BladKind = BladKind();
            ViewBag.LoadingInterval = LoadingInterval();
            return View(loadingReport);
        }


        // POST: LoadingReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
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
