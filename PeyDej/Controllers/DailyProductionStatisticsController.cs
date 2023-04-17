using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
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

        // GET: DailyProductionStatistics
        public async Task<IActionResult> Index(long Id)
        {
            return _context.ProductionStatistics != null
                ? View(await _context.ProductionStatistics.Where(m => m.DailyStatisticsId == Id).ToListAsync())
                : Problem("Entity set 'PeyDejContext.ProductionStatistics'  is null.");
        }

        // GET: DailyProductionStatistics/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.ProductionStatistics == null)
            {
                return NotFound();
            }

            var dailyProductionStatistic = await _context.ProductionStatistics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyProductionStatistic == null)
            {
                return NotFound();
            }

            return View(dailyProductionStatistic);
        }

        // GET: DailyProductionStatistics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DailyProductionStatistics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,InsDate,ShiftId,DepartmentId,ProductionCount,StopsHour,DailyStatisticsId")]
            DailyProductionStatistic dailyProductionStatistic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyProductionStatistic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(dailyProductionStatistic);
        }

        // GET: DailyProductionStatistics/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.ProductionStatistics == null)
            {
                return NotFound();
            }

            var dailyProductionStatistic = await _context.ProductionStatistics.FindAsync(id);
            if (dailyProductionStatistic == null)
            {
                return NotFound();
            }

            return View(dailyProductionStatistic);
        }

        // POST: DailyProductionStatistics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id,
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

                return RedirectToAction(nameof(Index));
            }

            return View(dailyProductionStatistic);
        }

        // GET: DailyProductionStatistics/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.ProductionStatistics == null)
            {
                return NotFound();
            }

            var dailyProductionStatistic = await _context.ProductionStatistics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyProductionStatistic == null)
            {
                return NotFound();
            }

            return View(dailyProductionStatistic);
        }

        // POST: DailyProductionStatistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.ProductionStatistics == null)
            {
                return Problem("Entity set 'PeyDejContext.ProductionStatistics'  is null.");
            }

            var dailyProductionStatistic = await _context.ProductionStatistics.FindAsync(id);
            if (dailyProductionStatistic != null)
            {
                _context.ProductionStatistics.Remove(dailyProductionStatistic);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyProductionStatisticExists(long id)
        {
            return (_context.ProductionStatistics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}