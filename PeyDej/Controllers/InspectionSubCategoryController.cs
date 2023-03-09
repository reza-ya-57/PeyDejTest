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
    public class InspectionSubCategoryController : Controller
    {
        private readonly PeyDejContext _context;

        public InspectionSubCategoryController(PeyDejContext context)
        {
            _context = context;
        }

        // GET: InspectionSubCategory
        public IActionResult Index()
        {
            var peyDejContext = _context.InspectionSubCategories.Include(i => i.InspectionCategory);
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_Index", peyDejContext);
            return View();
        }

        // GET: InspectionSubCategory/Create
        public IActionResult Create()
        {
            ViewData["InspectionCategoryId"] = new SelectList(_context.InspectionCategories, "Id", "Caption");
            return View();
        }

        // POST: InspectionSubCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,InsDate,Caption,InspectionCategoryId,InspectionCycle,GeneralStatusId")]
            InspectionSubCategory inspectionSubCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inspectionSubCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["InspectionCategoryId"] = new SelectList(_context.InspectionCategories, "Id", "Caption",
                inspectionSubCategory.InspectionCategoryId);
            return View(inspectionSubCategory);
        }

        // GET: InspectionSubCategory/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.InspectionSubCategories == null)
            {
                return NotFound();
            }

            var inspectionSubCategory = await _context.InspectionSubCategories.FindAsync(id);
            if (inspectionSubCategory == null)
            {
                return NotFound();
            }

            ViewData["InspectionCategoryId"] = new SelectList(_context.InspectionCategories, "Id", "Caption",
                inspectionSubCategory.InspectionCategoryId);
            return View(inspectionSubCategory);
        }

        // POST: InspectionSubCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id,
            [Bind("Id,InsDate,Caption,InspectionCategoryId,InspectionCycle,GeneralStatusId")]
            InspectionSubCategory inspectionSubCategory)
        {
            if (id != inspectionSubCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inspectionSubCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InspectionSubCategoryExists(inspectionSubCategory.Id))
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

            ViewData["InspectionCategoryId"] = new SelectList(_context.InspectionCategories, "Id", "Caption",
                inspectionSubCategory.InspectionCategoryId);
            return View(inspectionSubCategory);
        }

        // POST: InspectionSubCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var inspectionSubCategory = await _context.InspectionSubCategories.FindAsync(id);
            if (inspectionSubCategory != null)
            {
                inspectionSubCategory.GeneralStatusId = GeneralStatus.Deleted;
                _context.InspectionSubCategories.Update(inspectionSubCategory);
            }

            await _context.SaveChangesAsync();
            return Json(new { hasError = false, message = "" });
        }

        private bool InspectionSubCategoryExists(long id)
        {
            return (_context.InspectionSubCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}