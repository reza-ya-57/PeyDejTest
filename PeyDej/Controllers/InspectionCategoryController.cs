using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
using PeyDej.Models.Bases;

namespace PeyDej.Controllers
{
    [Authorize]
    public class InspectionCategoryController : Controller
    {
        private readonly PeyDejContext _context;

        public InspectionCategoryController(PeyDejContext context)
        {
            _context = context;
        }

        // GET: InspectionCategory
        public IActionResult Index()
        {
            return View(_context.InspectionCategories.AsEnumerable());
        }

        // GET: InspectionCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InspectionCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InsDate,Caption")] InspectionCategory inspectionCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inspectionCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inspectionCategory);
        }

        // GET: InspectionCategory/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.InspectionCategories == null)
            {
                return NotFound();
            }

            var inspectionCategory = await _context.InspectionCategories.FindAsync(id);
            if (inspectionCategory == null)
            {
                return NotFound();
            }
            return View(inspectionCategory);
        }

        // POST: InspectionCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,InsDate,Caption")] InspectionCategory inspectionCategory)
        {
            if (id != inspectionCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inspectionCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InspectionCategoryExists(inspectionCategory.Id))
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
            return View(inspectionCategory);
        }

        // POST: InspectionCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var inspectionCategory = await _context.InspectionCategories.FindAsync(id);
            if (inspectionCategory != null)
            {
                _context.InspectionCategories.Remove(inspectionCategory);
            }
            
            await _context.SaveChangesAsync();
            return Json(new { hasError = false, message = "" });
        }

        private bool InspectionCategoryExists(long id)
        {
          return (_context.InspectionCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
