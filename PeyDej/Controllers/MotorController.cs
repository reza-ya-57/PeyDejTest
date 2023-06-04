using Ccms.Common.Utilities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Bases;
using PeyDej.Models.Dtos;
using PeyDej.Services.Pagination;
using PeyDej.Tools;

using System.Reflection.PortableExecutable;
using System.Security.Claims;

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
            var data = new Motor()
            {
                DepartmentIds = _context.VwCategories.Where(w => w.CategoryId == 2).Select(s => new CategoryDto()
                {
                    Id = s.SubCategoryId,
                    Name = s.SubCategoryCaption
                }).AsEnumerable(),
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
                motor.CreatorUserId = User.Claims.Where(w => w.Type == ClaimTypes.Sid)?.FirstOrDefault()?.Value;
                motor.InspectionStartDate = PeyDejTools.PersianStringToDateTime(motor.InspectionStartDateDto);
                var result = _context.Add(motor);
                await _context.SaveChangesAsync();

                foreach (var sparePartId in motor.SparePartIds)
                {
                    _context.Set<SparePartMotor>().Add(new SparePartMotor(result.Entity.Id, sparePartId));
                    _context.SaveChanges();
                }

                var result2 = _context.MotorISs.Add(new Models.Inspection.MotorIS()
                {
                    InspectionDate = PeyDejTools.PersianStringToDateTime(motor.InspectionStartDateDto),
                    MotorId = result.Entity.Id,
                    Status = 0,
                    InsDate = DateTime.Now,
                    InspectionFinishedDate = null
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            motor.DepartmentIds = _context.VwCategories.Where(w => w.CategoryId == 2).Select(s => new CategoryDto()
            {
                Id = s.SubCategoryId,
                Name = s.SubCategoryCaption
            }).AsEnumerable();
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
            motor.InspectionStartDateDto = motor.InspectionStartDate.ToShamsi();
            if (motor == null)
            {
                return NotFound();
            }
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
                    motor.LastEditorUserId = User.Claims.Where(w => w.Type == ClaimTypes.Sid).FirstOrDefault().Value;
                    motor.LastEditDate = DateTime.Now;
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







        public async Task<IActionResult> ListMotorReport(
            long id,
            string sortOrder,
            string currentFilter,
            string searchString,
            int pageNumber = 1,
            int pageSize = 100)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var partMachines = _context.SparePartMotors.Where(w => w.MotorId == id).AsQueryable();
            var listSparePartIds = partMachines.Select(w => w.SparePartId).ToList();
            ViewData["SpareParts"] = _context.SpareParts.Where(w => !listSparePartIds.Contains(w.Id) && w.GeneralStatusId != null).AsEnumerable();
            ViewData["SparePartAll"] = _context.SpareParts.Where(w => w.GeneralStatusId != null).AsEnumerable();
            ViewData["CurrentFilter"] = searchString;
            ViewData["Motor"] = _context.Motors.Where(w => w.Id == id).FirstOrDefault();

            var partMachineResult = await PaginatedList<SparePartMotor>.CreateAsync(partMachines, pageIndex: pageNumber, pageSize);
            return View(partMachineResult);
        }

        [HttpPost]
        public IActionResult SaveMotorReport(long motorId, int sparePartId, int sparePartCount)
        {

            _context.SparePartMotors.Add(new SparePartMotor()
            {
                InsDate = DateTime.Now,
                MotorId = motorId,
                SparePartId = sparePartId,
                SparePartCount = sparePartCount
            });

            _context.SaveChanges();
            return Json(true);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSparePartMotors(long id)
        {
            var sparePartMotor = await _context.SparePartMotors.FindAsync(id);
            if (sparePartMotor != null)
            {
                _context.SparePartMotors.Remove(sparePartMotor);
            }

            await _context.SaveChangesAsync();
            return Json(new { hasError = false, message = "" });
        }
    }
}