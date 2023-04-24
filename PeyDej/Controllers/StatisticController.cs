using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models.Report;
using PeyDej.Tools;

using System.Security.Cryptography;

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
            return View(_context.DailyStatistics.AsEnumerable());
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
                    DepartmentId = 107,
                    StopsHour = model.ShiftId_100_DepartmentId_107_StopsHour,
                    ShiftId = 100,
                    ProductionCount = model.ShiftId_100_DepartmentId_107_ProductionCount
                });
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

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

            var model = new DailyDto()
            {
                Id = dailyStatistic.Id,
                LoadingCount = dailyStatistic.LoadingCount,
                Week = dailyStatistic.Week,
                Date = dailyStatistic.Date,
                NumberOfOpenPort = dailyStatistic.NumberOfOpenPort,
            };
            var productionStatisticsList = _context.ProductionStatistics.Where(w => w.DailyStatisticsId == id);
            if (productionStatisticsList.Any())
            {
                foreach (var statistic in productionStatisticsList)
                {
                    if (statistic.ShiftId == 99 && statistic.DepartmentId == 106)
                    {
                        model.ShiftId_99_DepartmentId_106_StopsHour = statistic.StopsHour ?? 0;
                        model.ShiftId_99_DepartmentId_106_ProductionCount = statistic.ProductionCount ?? 0;
                        model.ShiftId_99_DepartmentId_106_Id = statistic.Id;
                    }
                    if (statistic.ShiftId == 100 && statistic.DepartmentId == 106)
                    {
                        model.ShiftId_100_DepartmentId_106_StopsHour = statistic.StopsHour ?? 0;
                        model.ShiftId_100_DepartmentId_106_ProductionCount = statistic.ProductionCount ?? 0;
                        model.ShiftId_100_DepartmentId_106_Id = statistic.Id;
                    }
                    ///
                    if (statistic.ShiftId == 99 && statistic.DepartmentId == 107)
                    {
                        model.ShiftId_99_DepartmentId_107_StopsHour = statistic.StopsHour ?? 0;
                        model.ShiftId_99_DepartmentId_107_ProductionCount = statistic.ProductionCount ?? 0;
                        model.ShiftId_99_DepartmentId_107_Id = statistic.Id;
                    }
                    if (statistic.ShiftId == 100 && statistic.DepartmentId == 107)
                    {
                        model.ShiftId_100_DepartmentId_107_StopsHour = statistic.StopsHour ?? 0;
                        model.ShiftId_100_DepartmentId_107_ProductionCount = statistic.ProductionCount ?? 0;
                        model.ShiftId_100_DepartmentId_107_Id = statistic.Id;
                    }
                }
            }
            return View(model);
        }

        // POST: DailyStatistic/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, DailyDto model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = _context.DailyStatistics.Update(new DailyStatistic()
                    {
                        Id = id,
                        LoadingCount = model.LoadingCount,
                        Week = model.Week,
                        Date = model.Date,
                        NumberOfOpenPort = model.NumberOfOpenPort,
                    });
                    _context.SaveChanges();

                    //model.ShiftId_99_DepartmentId_106_StopsHour
                    _context.ProductionStatistics.Update(new DailyProductionStatistic()
                    {
                        Id = model.ShiftId_99_DepartmentId_106_Id,
                        DailyStatisticsId = result.Entity.Id,
                        DepartmentId = 106,
                        StopsHour = model.ShiftId_99_DepartmentId_106_StopsHour,
                        ShiftId = 99,
                        ProductionCount = model.ShiftId_99_DepartmentId_106_ProductionCount
                    });
                    _context.SaveChanges();

                    //model.ShiftId_100_DepartmentId_106_StopsHour
                    _context.ProductionStatistics.Update(new DailyProductionStatistic()
                    {
                        Id = model.ShiftId_100_DepartmentId_106_Id,
                        DailyStatisticsId = result.Entity.Id,
                        DepartmentId = 106,
                        StopsHour = model.ShiftId_100_DepartmentId_106_StopsHour,
                        ShiftId = 100,
                        ProductionCount = model.ShiftId_100_DepartmentId_106_ProductionCount
                    });
                    _context.SaveChanges();

                    //model.ShiftId_99_DepartmentId_107_StopsHour
                    _context.ProductionStatistics.Update(new DailyProductionStatistic()
                    {
                        Id = model.ShiftId_99_DepartmentId_107_Id,
                        DailyStatisticsId = result.Entity.Id,
                        DepartmentId = 107,
                        StopsHour = model.ShiftId_99_DepartmentId_107_StopsHour,
                        ShiftId = 99,
                        ProductionCount = model.ShiftId_99_DepartmentId_107_ProductionCount
                    });
                    _context.SaveChanges();

                    //model.ShiftId_100_DepartmentId_107_StopsHour
                    _context.ProductionStatistics.Update(new DailyProductionStatistic()
                    {
                        Id = model.ShiftId_100_DepartmentId_107_Id,
                        DailyStatisticsId = result.Entity.Id,
                        DepartmentId = 107,
                        StopsHour = model.ShiftId_100_DepartmentId_107_StopsHour,
                        ShiftId = 100,
                        ProductionCount = model.ShiftId_100_DepartmentId_107_ProductionCount
                    });
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyStatisticExists(model.Id))
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

            return View(model);
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