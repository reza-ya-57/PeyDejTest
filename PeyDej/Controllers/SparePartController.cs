using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Enums;
using PeyDej.Models;
using PeyDej.Models.Bases;
using PeyDej.Service.File;

using System.Reflection.PortableExecutable;
using System.Security.Claims;

namespace PeyDej.Controllers
{
    [Authorize]
    public class SparePartController : Controller
    {
        private readonly PeyDejContext _context;
        private IFileInterface FileInterface { get; }

        public SparePartController(PeyDejContext context, IFileInterface fileInterface)
        {
            _context = context;
            FileInterface = fileInterface;
        }

        public IActionResult Index()
        {
            return View(_context.SpareParts.Where(m => m.GeneralStatusId == GeneralStatus.Active).AsEnumerable());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SparePart sparePart)
        {
            if (!ModelState.IsValid) return View(sparePart);
            if (sparePart.FormFiles is not null)
            {
                sparePart.FileId = FileInterface.AddFile(UploadFor.SparePart, sparePart.FormFiles);
            }
            sparePart.CreatorUserId = User.Claims.Where(w => w.Type == ClaimTypes.Sid)?.FirstOrDefault()?.Value;
            _context.Add(sparePart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: SparePart/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, SparePart sparePart)
        {
            if (id != sparePart.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid) return View(sparePart);
            try
            {
                if (sparePart.FormFiles is not null)
                {
                    sparePart.FileId = FileInterface.EditFile(UploadFor.SparePart, sparePart.FormFiles, sparePart.FileId ?? Guid.Empty);
                }
                sparePart.LastEditorUserId = User.Claims.Where(w => w.Type == ClaimTypes.Sid).FirstOrDefault().Value;
                sparePart.LastEditDate = DateTime.Now;
                _context.Update(sparePart);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SparePartExists(sparePart.Id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));

        }

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