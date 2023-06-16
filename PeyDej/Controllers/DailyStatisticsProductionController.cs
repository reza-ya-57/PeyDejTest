using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models.Report;

using System.Data;

namespace PeyDej.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DailyStatisticsProductionController : Controller
    {
        private readonly PeyDejContext _context;

        public DailyStatisticsProductionController(PeyDejContext context)
        {
            _context = context;
        }

        private IEnumerable<object> departments()
        {
            return new SelectList(_context.VwCategories.Where(m => m.CategoryId == 9).ToList(), "SubCategoryId",
                "SubCategoryCaption");
        }

        private IEnumerable<object> shifts()
        {
            return new SelectList(_context.VwCategories.Where(m => m.CategoryId == 6).ToList(), "SubCategoryId",
                "SubCategoryCaption");
        }

        // GET: DailyProductionStatistics
        public async Task<IActionResult> Index(long dailyStatisticsId)
        {
            //ViewBag.dailyStatisticsId = dailyStatisticsId;
            var result = await _context.DailyStatisticsProduction.ToListAsync();
            return View(result);
        }


        // GET: DailyProductionStatistics/Create
        public IActionResult Create(long dailyStatisticsId)
        {
            ViewBag.departments = departments();
            ViewBag.shifts = shifts();
            ViewBag.dailyStatisticsId = dailyStatisticsId;
            return View(new DailyStatisticsProduction()
            {
                Id = dailyStatisticsId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,WetShelfCount,DryShelfCount,StopHour,StopMinute")] 
                                                 DailyStatisticsProduction dailyStatisticsProduction)
        {
            if (ModelState.IsValid)
            {
                var checkDate = _context.DailyStatisticsProduction.ToList().Exists(m => m.Date == dailyStatisticsProduction.Date);
                //var result = _context.DailyStatisticsProduction.Select(m => m.Date == dailyStatisticsProduction.Date).ToList();
                if (checkDate == true)
                {
                    ViewBag.ErrorMessage = "این تاریخ قبلا وارد شده است";
                    return View();
                }
                _context.Add(dailyStatisticsProduction);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "DailyStatisticsProduction");
            }
            return View(dailyStatisticsProduction);
            //ViewBag.departments = departments();
            //ViewBag.shifts = shifts();
            //ViewBag.dailyStatisticsId = dailyStatisticsId;
            //return View(dailyProductionStatistic);
        }

        // GET: DailyProductionStatistics/Edit/5
        public async Task<IActionResult> Edit(long? id, long dailyStatisticsId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyProductionStatistic = await _context.DailyStatisticsProduction.FindAsync(id);
            if (dailyProductionStatistic == null)
            {
                return NotFound();
            }

            //ViewBag.departments = departments();
            //ViewBag.shifts = shifts();
            //ViewBag.dailyStatisticsId = dailyStatisticsId;
            return View(dailyProductionStatistic);
        }

        // POST: DailyProductionStatistics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, long dailyStatisticsId,
            [Bind("Id,Date,WetShelfCount,DryShelfCount,StopHour,StopMinute")]
            DailyStatisticsProduction dailyProductionStatistic)
        {
            if (id != dailyProductionStatistic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyProductionStatistic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyProductionStatisticExists(dailyProductionStatistic.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index", "DailyStatisticsProduction", new
                {
                    dailyStatisticsId = dailyStatisticsId
                });
            }

            ViewBag.dailyStatisticsId = dailyStatisticsId;
            ViewBag.departments = departments();
            ViewBag.shifts = shifts();
            return View(dailyProductionStatistic);
        }


        // POST: DailyProductionStatistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var dailyProductionStatistic = await _context.DailyStatisticsProduction.FindAsync(id);
            if (dailyProductionStatistic != null)
            {
                _context.DailyStatisticsProduction.Remove(dailyProductionStatistic);
            }

            await _context.SaveChangesAsync();
            return Json(new { r = true });
        }

        private bool DailyProductionStatisticExists(long id)
        {
            return (_context.DailyStatisticsProduction?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}