using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
using PeyDej.Models.Bases.Views;
using PeyDej.Models.Report;

namespace PeyDej.Controllers
{
    public class DailyProductionStatisticsController : Controller
    {
        private readonly PeyDejContext _context;

        public DailyProductionStatisticsController(PeyDejContext context)
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
            ViewBag.dailyStatisticsId = dailyStatisticsId;
            return View(await _context.ProductionStatistics.Where(m => m.DailyStatisticsId == dailyStatisticsId)
                .ToListAsync());
        }


        // GET: DailyProductionStatistics/Create
        public IActionResult Create(long dailyStatisticsId)
        {
            ViewBag.departments = departments();
            ViewBag.shifts = shifts();
            ViewBag.dailyStatisticsId = dailyStatisticsId;
            return View(new DailyProductionStatistic()
            {
                DailyStatisticsId = dailyStatisticsId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(long dailyStatisticsId,
            [Bind("Id,InsDate,ShiftId,DepartmentId,ProductionCount,StopsHour,DailyStatisticsId")]
            DailyProductionStatistic dailyProductionStatistic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyProductionStatistic);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "DailyProductionStatistics", new
                {
                    dailyStatisticsId = dailyStatisticsId
                });
            }

            ViewBag.departments = departments();
            ViewBag.shifts = shifts();
            ViewBag.dailyStatisticsId = dailyStatisticsId;
            return View(dailyProductionStatistic);
        }

        // GET: DailyProductionStatistics/Edit/5
        public async Task<IActionResult> Edit(long? id, long dailyStatisticsId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyProductionStatistic = await _context.ProductionStatistics.FindAsync(id);
            if (dailyProductionStatistic == null)
            {
                return NotFound();
            }

            ViewBag.departments = departments();
            ViewBag.shifts = shifts();
            ViewBag.dailyStatisticsId = dailyStatisticsId;
            return View(dailyProductionStatistic);
        }

        // POST: DailyProductionStatistics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, long dailyStatisticsId,
            [Bind("Id,InsDate,ShiftId,DepartmentId,ProductionCount,StopsHour,DailyStatisticsId")]
            DailyProductionStatistic dailyProductionStatistic)
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

                return RedirectToAction("Index", "DailyProductionStatistics", new
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
            var dailyProductionStatistic = await _context.ProductionStatistics.FindAsync(id);
            if (dailyProductionStatistic != null)
            {
                _context.ProductionStatistics.Remove(dailyProductionStatistic);
            }

            await _context.SaveChangesAsync();
            return Json(new { r = true });
        }

        private bool DailyProductionStatisticExists(long id)
        {
            return (_context.ProductionStatistics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}