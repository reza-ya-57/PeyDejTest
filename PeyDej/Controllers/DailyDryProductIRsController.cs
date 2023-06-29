using Ccms.Common.Utilities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Report;
using PeyDej.Tools;

using System.Data;

namespace PeyDej.Controllers
{
    [Authorize(Roles = "Admin,Production")]
    public class DailyDryProductIRsController : Controller
    {
        private readonly PeyDejContext _context;

        public DailyDryProductIRsController(PeyDejContext context)
        {
            _context = context;
        }

        // GET: DailyDryProductIrs
        public async Task<IActionResult> Index()
        {
            var result = await _context.DailyDryProductIrs.Where(w => w.GeneralStatusId == GeneralStatus.Active).ToListAsync();
            foreach(var item in result)
            {
                item.DateDto = item.Date.ToShamsi().Split(' ')[0];
            }
            return View(result);
        }

        // GET: DailyDryProductIrs/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.DailyDryProductIrs == null)
            {
                return NotFound();
            }

            var dailyDryProductIR = await _context.DailyDryProductIrs
                .FirstOrDefaultAsync(m => m.Id == id);
            dailyDryProductIR.ShiftOperatorPersonName = _context.Persons.Where(person => person.Id == dailyDryProductIR.ShiftOperatorPersonId).Select(person => person.FullName).FirstOrDefault();
            if (dailyDryProductIR == null)
            {
                return NotFound();
            }

            return View(dailyDryProductIR);
        }

        // GET: DailyDryProductIrs/Create
        public IActionResult Create()
        {
            DailyDryProductIR dailyWetProductIr = new()
            {
                TemplateKindList = _context.SubCategories.Where(w => w.CategoryId == 21),
                ShiftOperatorPersons = _context.Persons.Where(w => w.GeneralStatusId == GeneralStatus.Active)
            };
            return View(dailyWetProductIr);
        }

        // POST: DailyDryProductIrs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DailyDryProductIR dailyDryProductIR)
        {
            if (ModelState.IsValid)
            {
                dailyDryProductIR.Date = PeyDejTools.PersianStringToDateTime(dailyDryProductIR.DateDto);
                _context.Add(dailyDryProductIR);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            dailyDryProductIR.TemplateKindList = _context.SubCategories.Where(w => w.CategoryId == 21);
            dailyDryProductIR.ShiftOperatorPersons = _context.Persons.Where(w => w.GeneralStatusId == GeneralStatus.Active);
            return View(dailyDryProductIR);
        }

        // GET: DailyDryProductIrs/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.DailyDryProductIrs == null)
            {
                return NotFound();
            }

            var dailyDryProductIR = await _context.DailyDryProductIrs.FindAsync(id);
            dailyDryProductIR.ShiftOperatorPersons = _context.Persons.Where(w => w.GeneralStatusId == GeneralStatus.Active);
            dailyDryProductIR.DateDto = dailyDryProductIR.Date.ToShamsi().Split(' ')[0];
            if (dailyDryProductIR == null)
            {
                return NotFound();
            }
            dailyDryProductIR.TemplateKindList = _context.SubCategories.Where(w => w.CategoryId == 21);
            dailyDryProductIR.ShiftOperatorPersons = _context.Persons.Where(w => w.GeneralStatusId == GeneralStatus.Active);
            return View(dailyDryProductIR);
        }

        // POST: DailyDryProductIrs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, DailyDryProductIR dailyDryProductIR)
        {
            if (id != dailyDryProductIR.Id)
            {
                return NotFound();
            }
            dailyDryProductIR.Date = (DateTime)dailyDryProductIR.DateDto.ToMiladi();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyDryProductIR);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyWetProductIRExists(dailyDryProductIR.Id))
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
            dailyDryProductIR.TemplateKindList = _context.SubCategories.Where(w => w.CategoryId == 21);
            dailyDryProductIR.ShiftOperatorPersons = _context.Persons.Where(w => w.GeneralStatusId == GeneralStatus.Active);
            return View(dailyDryProductIR);
        }

        // GET: DailyDryProductIrs/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.DailyDryProductIrs == null)
            {
                return NotFound();
            }

            var dailyDryProductIR = await _context.DailyDryProductIrs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyDryProductIR == null)
            {
                return NotFound();
            }

            return Ok();
            // return View(dailyDryProductIR);
        }

        // POST: LoadingReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var dailyDryProductIR = await _context.DailyDryProductIrs.FindAsync(id);
            if (dailyDryProductIR != null)
            {
                _context.DailyDryProductIrs.Remove(dailyDryProductIR);
            }
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return Json(new { r = true });
        }

        private bool DailyWetProductIRExists(long id)
        {
            return (_context.DailyDryProductIrs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
