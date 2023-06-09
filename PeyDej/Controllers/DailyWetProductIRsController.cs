using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ccms.Common.Utilities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Bases;
using PeyDej.Models.Report;
using PeyDej.Tools;

namespace PeyDej.Controllers
{
    public class DailyWetProductIRsController : Controller
    {
        private readonly PeyDejContext _context;

        public DailyWetProductIRsController(PeyDejContext context)
        {
            _context = context;
        }

        // GET: DailyWetProductIRs
        public async Task<IActionResult> Index()
        {
            return View(await _context.DailyWetProductIrs.Where(w => w.GeneralStatusId == GeneralStatus.Active).ToListAsync());
        }

        // GET: DailyWetProductIRs/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.DailyWetProductIrs == null)
            {
                return NotFound();
            }

            var dailyWetProductIR = await _context.DailyWetProductIrs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyWetProductIR == null)
            {
                return NotFound();
            }

            return View(dailyWetProductIR);
        }

        // GET: DailyWetProductIRs/Create
        public IActionResult Create()
        {
            DailyWetProductIR dailyWetProductIr = new()
            {
                SoilTypeList = _context.SubCategories.Where(w => w.CategoryId == 17),
                DaneBandiGarkList = _context.SubCategories.Where(w => w.CategoryId == 22),
                TemplateKindList = _context.SubCategories.Where(w => w.CategoryId == 21),
                ShiftOperatorPerson = _context.Persons.Where(w => w.GeneralStatusId == GeneralStatus.Active)
            };
            return View(dailyWetProductIr);
        }

        // POST: DailyWetProductIRs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DailyWetProductIR dailyWetProductIR)
        {
            if (ModelState.IsValid)
            {
                dailyWetProductIR.Date = PeyDejTools.PersianStringToDateTime(dailyWetProductIR.DateDto);
                _context.Add(dailyWetProductIR);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            dailyWetProductIR.SoilTypeList = _context.SubCategories.Where(w => w.CategoryId == 17);
            dailyWetProductIR.DaneBandiGarkList = _context.SubCategories.Where(w => w.CategoryId == 22);
            dailyWetProductIR.TemplateKindList = _context.SubCategories.Where(w => w.CategoryId == 21);
            dailyWetProductIR.ShiftOperatorPerson = _context.Persons.Where(w => w.GeneralStatusId == GeneralStatus.Active);
            return View(dailyWetProductIR);
        }

        // GET: DailyWetProductIRs/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.DailyWetProductIrs == null)
            {
                return NotFound();
            }

            var dailyWetProductIR = await _context.DailyWetProductIrs.FindAsync(id);
            dailyWetProductIR.SoilTypeList = _context.SubCategories.Where(w => w.CategoryId == 17);
            dailyWetProductIR.DaneBandiGarkList = _context.SubCategories.Where(w => w.CategoryId == 22);
            dailyWetProductIR.TemplateKindList = _context.SubCategories.Where(w => w.CategoryId == 21);
            dailyWetProductIR.ShiftOperatorPerson = _context.Persons.Where(w => w.GeneralStatusId == GeneralStatus.Active);
            dailyWetProductIR.DateDto = dailyWetProductIR.Date.ToShamsi().Split(' ')[0];
            if (dailyWetProductIR == null)
            {
                return NotFound();
            }
            return View(dailyWetProductIR);
        }

        // POST: DailyWetProductIRs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, DailyWetProductIR dailyWetProductIR)
        {
            if (id != dailyWetProductIR.Id)
            {
                return NotFound();
            }
            dailyWetProductIR.Date = (DateTime)dailyWetProductIR.DateDto.ToMiladi();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyWetProductIR);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyWetProductIRExists(dailyWetProductIR.Id))
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
            dailyWetProductIR.SoilTypeList = _context.SubCategories.Where(w => w.CategoryId == 17);
            dailyWetProductIR.DaneBandiGarkList = _context.SubCategories.Where(w => w.CategoryId == 22);
            dailyWetProductIR.TemplateKindList = _context.SubCategories.Where(w => w.CategoryId == 21);
            dailyWetProductIR.ShiftOperatorPerson = _context.Persons.Where(w => w.GeneralStatusId == GeneralStatus.Active);
            return View(dailyWetProductIR);
        }

        // GET: DailyWetProductIRs/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.DailyWetProductIrs == null)
            {
                return NotFound();
            }

            var dailyWetProductIR = await _context.DailyWetProductIrs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyWetProductIR == null)
            {
                return NotFound();
            }

            return View(dailyWetProductIR);
        }

        // POST: LoadingReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var dailyWetProductIR = await _context.DailyWetProductIrs.FindAsync(id);
            if (dailyWetProductIR != null)
            {
                _context.DailyWetProductIrs.Remove(dailyWetProductIR);
            }
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return Json(new { r = true });
        }

        private bool DailyWetProductIRExists(long id)
        {
            return (_context.DailyWetProductIrs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
