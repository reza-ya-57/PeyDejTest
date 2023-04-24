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


        // GET: LoadingReports/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoadingReportDto loadingReport)
        {
            if (ModelState.IsValid)
            {
                var lr_104 = _context.LoadingReports.Add(new LoadingReport()
                {
                    Date = loadingReport.Date,
                    DayCaption = loadingReport.DayCaption,
                    Description = loadingReport.Description_LoadingIntervalId_104,
                    LoadingIntervalId = 104
                });
                _context.SaveChanges();


                _context.LoadingReportDetails.Add(new LoadingReportDetail()
                {
                    BladKindId = 101,
                    LoadingReportId = lr_104.Entity.Id,
                    Value = loadingReport.BladKindId_101_LoadingIntervalId_104
                });
                _context.SaveChanges();
                _context.LoadingReportDetails.Add(new LoadingReportDetail()
                {
                    BladKindId = 102,
                    LoadingReportId = lr_104.Entity.Id,
                    Value = loadingReport.BladKindId_102_LoadingIntervalId_104
                });
                _context.SaveChanges();
                _context.LoadingReportDetails.Add(new LoadingReportDetail()
                {
                    BladKindId = 103,
                    LoadingReportId = lr_104.Entity.Id,
                    Value = loadingReport.BladKindId_103_LoadingIntervalId_104
                });


                var lr_105 = _context.LoadingReports.Add(new LoadingReport()
                {
                    Date = loadingReport.Date,
                    DayCaption = loadingReport.DayCaption,
                    Description = loadingReport.Description_LoadingIntervalId_105,
                    LoadingIntervalId = 105
                });
                _context.SaveChanges();


                _context.LoadingReportDetails.Add(new LoadingReportDetail()
                {
                    BladKindId = 101,
                    LoadingReportId = lr_105.Entity.Id,
                    Value = loadingReport.BladKindId_101_LoadingIntervalId_105
                });
                _context.SaveChanges();
                _context.LoadingReportDetails.Add(new LoadingReportDetail()
                {
                    BladKindId = 102,
                    LoadingReportId = lr_105.Entity.Id,
                    Value = loadingReport.BladKindId_102_LoadingIntervalId_105
                });
                _context.SaveChanges();
                _context.LoadingReportDetails.Add(new LoadingReportDetail()
                {
                    BladKindId = 103,
                    LoadingReportId = lr_105.Entity.Id,
                    Value = loadingReport.BladKindId_103_LoadingIntervalId_105
                });
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
            if (loadingReport == null)
            {
                return NotFound();
            }
            var model = new LoadingReportDto()
            {

            };
            ViewBag.BladKind = BladKind();
            return View(loadingReport);
        }

        // POST: LoadingReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, LoadingReportDto loadingReport)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var lr_104 = _context.LoadingReports.Update(new LoadingReport()
                    {
                        Id = loadingReport.LoadingReport_104_Id,
                        Date = loadingReport.Date,
                        DayCaption = loadingReport.DayCaption,
                        Description = loadingReport.Description_LoadingIntervalId_104,
                        LoadingIntervalId = 104
                    });
                    _context.SaveChanges();


                    _context.LoadingReportDetails.Update(new LoadingReportDetail()
                    {
                        BladKindId = 101,
                        LoadingReportId = lr_104.Entity.Id,
                        Value = loadingReport.BladKindId_101_LoadingIntervalId_104,
                        Id = loadingReport.BladKindId_101_LoadingIntervalId_104_Id
                    });
                    _context.SaveChanges();
                    _context.LoadingReportDetails.Update(new LoadingReportDetail()
                    {
                        BladKindId = 102,
                        LoadingReportId = lr_104.Entity.Id,
                        Value = loadingReport.BladKindId_102_LoadingIntervalId_104,
                        Id = loadingReport.BladKindId_102_LoadingIntervalId_104_Id
                    });
                    _context.SaveChanges();
                    _context.LoadingReportDetails.Update(new LoadingReportDetail()
                    {
                        BladKindId = 103,
                        LoadingReportId = lr_104.Entity.Id,
                        Value = loadingReport.BladKindId_103_LoadingIntervalId_104,
                        Id = loadingReport.BladKindId_103_LoadingIntervalId_104_Id
                    });


                    var lr_105 = _context.LoadingReports.Update(new LoadingReport()
                    {
                        Id = loadingReport.LoadingReport_105_Id,
                        Date = loadingReport.Date,
                        DayCaption = loadingReport.DayCaption,
                        Description = loadingReport.Description_LoadingIntervalId_104,
                        LoadingIntervalId = 105
                    });
                    _context.SaveChanges();


                    _context.LoadingReportDetails.Update(new LoadingReportDetail()
                    {
                        BladKindId = 101,
                        LoadingReportId = lr_105.Entity.Id,
                        Value = loadingReport.BladKindId_101_LoadingIntervalId_105,
                        Id = loadingReport.BladKindId_101_LoadingIntervalId_105_Id
                    });
                    _context.SaveChanges();
                    _context.LoadingReportDetails.Update(new LoadingReportDetail()
                    {
                        BladKindId = 102,
                        LoadingReportId = lr_105.Entity.Id,
                        Value = loadingReport.BladKindId_102_LoadingIntervalId_105,
                        Id = loadingReport.BladKindId_102_LoadingIntervalId_105_Id
                    });
                    _context.SaveChanges();
                    _context.LoadingReportDetails.Update(new LoadingReportDetail()
                    {
                        BladKindId = 103,
                        LoadingReportId = lr_105.Entity.Id,
                        Value = loadingReport.BladKindId_103_LoadingIntervalId_105,
                        Id = loadingReport.BladKindId_103_LoadingIntervalId_105_Id
                    });
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.BladKind = BladKind();
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
