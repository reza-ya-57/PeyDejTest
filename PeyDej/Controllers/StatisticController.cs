using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models.Report;
using PeyDej.Tools;

namespace PeyDej.Controllers
{
    public class StatisticController : Controller
    {
        private readonly PeyDejContext _context;

        public StatisticController(PeyDejContext context)
        {
            _context = context;
        }

        // GET: DailyStatistic
        public IActionResult Index()
        {
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_Index", _context.Set<DailyStatistic>());
            return View();
        }

        // GET: DailyStatistic/Create
        public IActionResult Create()
        {
            return View(new DailyDto()
            {
                Date = PeyDejTools.GetCurPersianDate()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DailyDto model)
        {
            if (ModelState.IsValid)
            {
                var result = _context.DailyStatistics.Add(new DailyStatistic()
                {
                    LoadingCount = model.LoadingCount,
                    Week = model.Week,
                    Date = model.Date,
                    NumberOfOpenPort = model.NumberOfOpenPort,
                });
                _context.SaveChanges();

                //model.ShiftId_99_DepartmentId_106_StopsHour
                _context.ProductionStatistics.Add(new DailyProductionStatistic()
                {
                    DailyStatisticsId = result.Entity.Id,
                    DepartmentId = 106,
                    StopsHour = model.ShiftId_99_DepartmentId_106_StopsHour,
                    ShiftId = 99,
                    ProductionCount = model.ShiftId_99_DepartmentId_106_ProductionCount
                });
                _context.SaveChanges();
                
                //model.ShiftId_100_DepartmentId_106_StopsHour
                _context.ProductionStatistics.Add(new DailyProductionStatistic()
                {
                    DailyStatisticsId = result.Entity.Id,
                    DepartmentId = 106,
                    StopsHour = model.ShiftId_100_DepartmentId_106_StopsHour,
                    ShiftId = 100,
                    ProductionCount = model.ShiftId_100_DepartmentId_106_ProductionCount
                });
                _context.SaveChanges();
                
                //model.ShiftId_99_DepartmentId_107_StopsHour
                _context.ProductionStatistics.Add(new DailyProductionStatistic()
                {
                    DailyStatisticsId = result.Entity.Id,
                    DepartmentId = 107,
                    StopsHour = model.ShiftId_99_DepartmentId_107_StopsHour,
                    ShiftId = 99,
                    ProductionCount = model.ShiftId_99_DepartmentId_107_ProductionCount
                });
                _context.SaveChanges();
                
                //model.ShiftId_100_DepartmentId_107_StopsHour
                _context.ProductionStatistics.Add(new DailyProductionStatistic()
                {
                    DailyStatisticsId = result.Entity.Id,
                    DepartmentId = 106,
                    StopsHour = model.ShiftId_100_DepartmentId_107_StopsHour,
                    ShiftId = 100,
                    ProductionCount = model.ShiftId_100_DepartmentId_107_ProductionCount
                });
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
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
            [Bind("Id,InsDate,Date,NumberOfOpenPort,LoadingCount")]
            DailyStatistic dailyStatistic)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            try
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
                return Json(new { r = true, m = "" });
            }
            catch (Exception ex)
            {
                return Json(new { r = false, m = ex.Message });
            }
        }

        private bool DailyStatisticExists(long id)
        {
            return (_context.DailyStatistics?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}