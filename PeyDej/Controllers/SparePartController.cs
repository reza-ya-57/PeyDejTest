using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Bases;

namespace PeyDej.Controllers
{
    public class SparePartController : Controller
    {
        private readonly PeyDejContext _context;

        public SparePartController(PeyDejContext context)
        {
            _context = context;
        }

        // GET: SparePart
        public IActionResult Index()
        {
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_Index", _context.SpareParts.Where(m => m.GeneralStatusId == GeneralStatus.Active));
            return View();
        }

        // GET: SparePart/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.SpareParts == null)
            {
                return NotFound();
            }

            var sparePart = await _context.SpareParts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sparePart == null)
            {
                return NotFound();
            }

            return View(sparePart);
        }

        // GET: SparePart/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SparePart/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,InsDate,Name,Model,Count,Emplacement,Description,GeneralStatusId")]
            SparePart sparePart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sparePart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(sparePart);
        }

        // GET: SparePart/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.SpareParts == null)
            {
                return NotFound();
            }

            var sparePart = await _context.SpareParts.FindAsync(id);
            if (sparePart == null)
            {
                return NotFound();
            }

            return View(sparePart);
        }

        // POST: SparePart/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id,
            [Bind("Id,InsDate,Name,Model,Count,Emplacement,Description,GeneralStatusId")]
            SparePart sparePart)
        {
            if (id != sparePart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sparePart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SparePartExists(sparePart.Id))
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

            return View(sparePart);
        }

        // POST: SparePart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var sparePart = await _context.SpareParts.FindAsync(id);
            if (sparePart != null)
            {
                sparePart.GeneralStatusId = GeneralStatus.Deleted;
                _context.SpareParts.Update(sparePart);
            }

            await _context.SaveChangesAsync();
            return Json(new { hasError = false, message = "" });
        }

        private bool SparePartExists(long id)
        {
            return (_context.SpareParts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}