using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
using PeyDej.Models.Report;
using PeyDej.Tools;

namespace PeyDej.Controllers
{
    public class DailyStatisticController : Controller
    {
        private readonly PeyDejContext _context;

        public DailyStatisticController(PeyDejContext context)
        {
            _context = context;
        }

        // GET: DailyStatistic
        public async Task<IActionResult> Index()
        {
            return _context.DailyStatistics != null
                ? View(await _context.DailyStatistics.ToListAsync())
                : Problem("Entity set 'PeyDejContext.DailyStatistics'  is null.");
        }

        // GET: DailyStatistic/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.DailyStatistics == null)
            {
                return NotFound();
            }

            var dailyStatistic = await _context.DailyStatistics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyStatistic == null)
            {
                return NotFound();
            }

            return View(dailyStatistic);
        }

        // GET: DailyStatistic/Create
        public IActionResult Create()
        {
            return View(new DailyStatistic()
            {
                Date = PeyDejTools.GetCurPersianDate()
            });
        }

        // POST: DailyStatistic/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,InsDate,Date,NumberOfOpenPort,LoadingCount")] DailyStatistic dailyStatistic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyStatistic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(dailyStatistic);
        }

        // GET: DailyStatistic/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.DailyStatistics == null)
            {
                return NotFound();
            }

            var dailyStatistic = await _context.DailyStatistics.FindAsync(id);
            if (dailyStatistic == null)
            {
                return NotFound();
            }

            return View(dailyStatistic);
        }

        // POST: DailyStatistic/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id,
            [Bind("Id,InsDate,Date,NumberOfOpenPort,LoadingCount")] DailyStatistic dailyStatistic)
        {
            if (id != dailyStatistic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyStatistic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyStatisticExists(dailyStatistic.Id))
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

            return View(dailyStatistic);
        }

        // GET: DailyStatistic/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.DailyStatistics == null)
            {
                return NotFound();
            }

            var dailyStatistic = await _context.DailyStatistics
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyStatistic == null)
            {
                return NotFound();
            }

            return View(dailyStatistic);
        }

        // POST: DailyStatistic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.DailyStatistics == null)
            {
                return Problem("Entity set 'PeyDejContext.DailyStatistics'  is null.");
            }

            var dailyStatistic = await _context.DailyStatistics.FindAsync(id);
            if (dailyStatistic != null)
            {
                _context.DailyStatistics.Remove(dailyStatistic);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyStatisticExists(long id)
        {
            return (_context.DailyStatistics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}