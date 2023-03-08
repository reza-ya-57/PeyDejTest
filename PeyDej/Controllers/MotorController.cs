using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
using PeyDej.Models.Bases;

namespace PeyDej.Controllers
{
    public class MotorController : Controller
    {
        private readonly PeyDejContext _context;

        public MotorController(PeyDejContext context)
        {
            _context = context;
        }

        // GET: Motor
        public async Task<IActionResult> Index()
        {
              return _context.Motors != null ? 
                          View(await _context.Motors.ToListAsync()) :
                          Problem("Entity set 'PeyDejContext.Motors'  is null.");
        }

        // GET: Motor/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Motors == null)
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
            return View();
        }

        // POST: Motor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InsDate,SerialNumber,Emplacement,Manufacturer,Kw,V,BeltSerial,BeltCount,Fooli,ChainSerial,Type,Gear,MachineId,InspectionCycle,Description,GeneralStatusId")] Motor motor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(motor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            return View(motor);
        }

        // POST: Motor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,InsDate,SerialNumber,Emplacement,Manufacturer,Kw,V,BeltSerial,BeltCount,Fooli,ChainSerial,Type,Gear,MachineId,InspectionCycle,Description,GeneralStatusId")] Motor motor)
        {
            if (id != motor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(motor);
                    await _context.SaveChangesAsync();
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
            return View(motor);
        }

        // GET: Motor/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Motors == null)
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

        // POST: Motor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Motors == null)
            {
                return Problem("Entity set 'PeyDejContext.Motors'  is null.");
            }
            var motor = await _context.Motors.FindAsync(id);
            if (motor != null)
            {
                _context.Motors.Remove(motor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MotorExists(long id)
        {
          return (_context.Motors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
