using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    public class PersonController : Controller
    {
        private readonly PeyDejContext _context;

        public PersonController(PeyDejContext context)
        {
            _context = context;
        }

        // GET: Person
        public IActionResult Index()
        {
            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_Index",
                    _context.Persons.Where(m => m.GeneralStatusId == GeneralStatus.Active).ToList());
            return View();
        }

        private IEnumerable<object> Gender() =>
            new SelectList(_context.VwCategories.Where(m => m.CategoryId == 5).ToList(), "SubCategoryId",
                "SubCategoryCaption");

        private IEnumerable<object>
            Department() => new SelectList(_context.VwCategories.Where(m => m.CategoryId == 2).ToList(),
            "SubCategoryId", "SubCategoryCaption");

        // GET: Person/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: Person/Create
        public IActionResult Create()
        {
            ViewBag.Gender = this.Gender();
            ViewBag.Department = this.Department();
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Id,InsDate,FirstName,LastName,NationalCode,PhoneNumber,DepartmentId,GenderId,Description,GeneralStatusId")]
            Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Gender = this.Gender();
            ViewBag.Department = this.Department();
            return View(person);
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Persons == null)
            {
                return NotFound();
            }

            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            ViewBag.Gender = this.Gender();
            ViewBag.Department = this.Department();
            return View(person);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id,
            [Bind(
                "Id,InsDate,FirstName,LastName,NationalCode,PhoneNumber,DepartmentId,GenderId,Description,GeneralStatusId")]
            Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
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

            ViewBag.Gender = this.Gender();
            ViewBag.Department = this.Department();
            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var persons = await _context.Persons.FindAsync(id);
            if (persons != null)
            {
                persons.GeneralStatusId = GeneralStatus.Deleted;
                _context.Persons.Update(persons);
            }

            await _context.SaveChangesAsync();
            return Json(new { hasError = false, message = "" });
        }

        private bool PersonExists(long id)
        {
            return (_context.Persons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}