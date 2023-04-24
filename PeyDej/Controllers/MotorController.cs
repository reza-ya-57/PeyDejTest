using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using NuGet.Protocol.Core.Types;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Bases;

namespace PeyDej.Controllers
{
    [Authorize]
    public class MotorController : Controller
    {
        private readonly PeyDejContext _context;

        public MotorController(PeyDejContext context)
        {
            _context = context;
        }

        // GET: Motor
        public IActionResult Index()
        {
            return View(_context.Motors.Where(m => m.GeneralStatusId == GeneralStatus.Active).AsEnumerable());
        }

        // GET: Motor/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var motor = await _context.Motors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (motor == null)
            {
                return NotFound();
            }

            return View(motor);
        }

        // GET: Motor/Create
        public IActionResult Create()
        {
            Motor data = new()
            {
                SpareParts = _context.SpareParts.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).AsEnumerable()
            };
            return View(data);
        }

        // POST: Motor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Motor motor)
        {
            if (ModelState.IsValid)
            {
                var result = _context.Add(motor);
                await _context.SaveChangesAsync();

                foreach (var sparePartId in motor.SparePartIds)
                {
                    _context.Set<SparePartMotor>().Add(new SparePartMotor(result.Entity.Id, sparePartId));
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }

            motor.SpareParts = _context.SpareParts.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).AsEnumerable();
            return View(motor);
        }

        // GET: Motor/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Motors == null)
            {
                return NotFound();
            }

            var motor = await _context.Motors.FindAsync(id);
            if (motor == null)
            {
                return NotFound();
            }
            motor.SpareParts = _context.SpareParts.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).AsEnumerable();
            motor.SparePartIds = _context.SparePartMotors.Where(w => w.MotorId == id).Select(s => s.SparePartId).ToList();
            return View(motor);
        }

        // POST: Motor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Motor motor)
        {
            if (id != motor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = _context.Update(motor);
                    await _context.SaveChangesAsync();

                    await _context.SparePartMotors.Where(w => w.MotorId == result.Entity.Id).ExecuteDeleteAsync();
                    foreach (var sparePartId in motor.SparePartIds)
                    {
                        await _context.SparePartMotors.AddAsync(new SparePartMotor(motorId: result.Entity.Id, sparePartId));
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MotorExists(motor.Id))
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

            motor.SpareParts = _context.SpareParts.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).AsEnumerable();
            return View(motor);
        }


        // POST: Motor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var motor = await _context.Motors.FindAsync(id);
            if (motor != null)
            {
                motor.GeneralStatusId = GeneralStatus.Deleted;
                _context.Motors.Update(motor);
            }

            await _context.SaveChangesAsync();
            return Json(new { hasError = false, message = "" });
        }

        private bool MotorExists(long id)
        {
            return (_context.Motors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}