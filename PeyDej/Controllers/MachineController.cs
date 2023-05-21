using System.Data;
using Ccms.Common.Utilities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Dapper;
using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Bases;
using PeyDej.Models.Dtos;
using PeyDej.Services.Pagination;
using PeyDej.Tools;
using PeyDej.Models.Inspection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace PeyDej.Controllers
{
    [Authorize]
    public class MachineController : Controller
    {
        private readonly PeyDejContext _context;
        private IConfiguration Configuration { get; }
        public MachineController(PeyDejContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View(_context.Machines.Where(m => m.GeneralStatusId == GeneralStatus.Active).AsEnumerable());
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machine = await _context.Machines
                .FirstOrDefaultAsync(m => m.Id == id);

            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        public async Task<IActionResult> Create()
        {
            IDbConnection connectionDb = new SqlConnection(Configuration.GetConnectionString("PeyDejContext_Online"));
            connectionDb.Open();
            var data = new Machine()
            {
                Department = null,
                Process = null,
                DepartmentIds = _context.VwCategories.Where(w => w.CategoryId == 2).Select(s => new CategoryDto()
                {
                    Id = s.SubCategoryId,
                    Name = s.SubCategoryCaption
                }).AsEnumerable(),
                ProcessIds = _context.VwCategories.Where(w => w.CategoryId == 3).Select(s => new CategoryDto()
                {
                    Id = s.SubCategoryId,
                    Name = s.SubCategoryCaption
                }).AsEnumerable(),
                MachineCheckListCategoryList = await connectionDb.QueryAsync<CategoryResutl>("Base.GetMachineInspectionTypes", commandType: CommandType.StoredProcedure),
            };
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Machine machine)
        {

            machine.LubricationStartDate = machine.LubricationStartDateDto == null ? DateTime.Now : PeyDejTools.PersianStringToDateTime(machine.LubricationStartDateDto);
            machine.InspectionStartDate = machine.InspectionStartDateDto == null ? DateTime.Now : PeyDejTools.PersianStringToDateTime(machine.InspectionStartDateDto);
            machine.UtilizationDate = machine.UtilizationDateDto == null ? DateTime.Now : PeyDejTools.PersianStringToDateTime(machine.UtilizationDateDto);

            if (ModelState.IsValid)
            {
                var result = _context.Add(machine);
                await _context.SaveChangesAsync();



                var result2 = _context.MachineISs.Add(new Models.Inspection.MachineIS()
                {
                    InspectionDate = PeyDejTools.PersianStringToDateTime(machine.InspectionStartDateDto),
                    MachineId = result.Entity.Id,
                    Status = 0,
                    InsDate = DateTime.Now,
                    InspectionFinishedDate = null
                });
                _context.SaveChanges();
                if (machine.LubricationStartDateDto != null)
                {
                    var result3 = _context.MachineLubrications.Add(new Models.Inspection.MachineLubricationIS()
                    {
                        InspectionDate = PeyDejTools.PersianStringToDateTime(machine.LubricationStartDateDto),
                        MachineId = result.Entity.Id,
                        Status = 0,
                        InsDate = DateTime.Now,
                        InspectionFinishedDate = null
                    });
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }

            IDbConnection connectionDb = new SqlConnection(Configuration.GetConnectionString("PeyDejContext_Online"));
            connectionDb.Open();
            machine.DepartmentIds = _context.VwCategories.Where(w => w.CategoryId == 2)
                .Select(s => new CategoryDto() { Id = s.SubCategoryId, Name = s.SubCategoryCaption }).AsEnumerable();
            machine.ProcessIds = _context.VwCategories.Where(w => w.CategoryId == 3)
                .Select(s => new CategoryDto() { Id = s.SubCategoryId, Name = s.SubCategoryCaption }).AsEnumerable();
            machine.MachineCheckListCategoryList =
                await connectionDb.QueryAsync<CategoryResutl>("Base.GetMachineInspectionTypes",
                    commandType: CommandType.StoredProcedure);
            return View(machine);
        }


        public async Task<IActionResult> ListMachineReport(
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

            var partMachines = _context.SparePartMachines.Where(w => w.MachineId == id).AsQueryable();
            var machineMotors = _context.MachineMotors.Where(w => w.MachineId == id).AsQueryable();
            var listSparePartIds = partMachines.Select(w => w.SparePartId).ToList();
            var Motors = _context.Motors.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).AsQueryable();
            var listMotorIds = machineMotors.Select(w => w.MotorId).ToList();

            ViewData["SpareParts"] = _context.SpareParts.Where(w => !listSparePartIds.Contains(w.Id) && w.GeneralStatusId != null).AsEnumerable();
            ViewData["SparePartAll"] = _context.SpareParts.Where(w => w.GeneralStatusId != null).AsEnumerable();
            ViewData["Motors"] = Motors.Where(w => !listMotorIds.Contains(w.Id)).AsEnumerable();
            ViewData["MotorsAll"] = Motors.AsEnumerable();
            ViewData["CurrentFilter"] = searchString;
            ViewData["Machine"] = _context.Machines.Where(w => w.Id == id).FirstOrDefault();

            var partMachineResult = await PaginatedList<SparePartMachine>.CreateAsync(partMachines, pageIndex: pageNumber, pageSize);
            var machineMotorResult = await PaginatedList<MachineMotor>.CreateAsync(machineMotors, pageIndex: pageNumber, pageSize);
            return View((partMachineResult, machineMotorResult));
        }

        [HttpPost]
        public IActionResult SaveMachineReport(long machineId, long sparePartId, int sparePartCount)
        {
            var sparePart = _context.SpareParts.Where(w => w.Id == sparePartId).FirstOrDefault();
            var machine = _context.Machines.Where(w => w.Id == machineId).FirstOrDefault();
            _context.SparePartMachines.Add(new SparePartMachine()
            {
                InsDate = DateTime.Now,
                MachineId = machineId,
                SparePartCount = sparePartCount,
                SparePartId = sparePartId
            });
            _context.SaveChanges();
            return Json(new { SparePartName = sparePart.Name, MachineName = machine.Name });
        }

        [HttpPost]
        public IActionResult SaveMachineMotor(long machineId, string[] motorIds)
        {
            foreach (var motorId in motorIds)
            {
                _context.MachineMotors.Add(new MachineMotor()
                {
                    InsDate = DateTime.Now,
                    MachineId = machineId,
                    MotorId = long.Parse(motorId),
                });
            }
            _context.SaveChanges();
            return Json("true");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteMachineMotor(long id)
        {
            var machineMotor = await _context.MachineMotors.FindAsync(id);
            if (machineMotor != null)
            {
                _context.MachineMotors.Remove(machineMotor);
            }

            await _context.SaveChangesAsync();
            return Json(new { hasError = false, message = "" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSparePartMachine(long id)
        {
            var partMachine = await _context.SparePartMachines.FindAsync(id);
            if (partMachine != null)
            {
                _context.SparePartMachines.Remove(partMachine);
            }

            await _context.SaveChangesAsync();
            return Json(new { hasError = false, message = "" });
        }


        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IDbConnection connectionDb = new SqlConnection(Configuration.GetConnectionString("PeyDejContext_Online"));
            connectionDb.Open();
            var machine = await _context.Machines.FindAsync(id);
            machine.InspectionStartDateDto = machine.InspectionStartDate.ToShamsi();
            machine.UtilizationDateDto = machine.UtilizationDate.ToShamsi();
            machine.LubricationStartDateDto = machine.LubricationStartDate.ToShamsi();
            if (machine == null)
            {
                return NotFound();
            }
            machine.DepartmentIds = _context.VwCategories.Where(w => w.CategoryId == 2).Select(s => new CategoryDto()
            {
                Id = s.SubCategoryId,
                Name = s.SubCategoryCaption
            }).AsEnumerable();
            machine.ProcessIds = _context.VwCategories.Where(w => w.CategoryId == 3).Select(s => new CategoryDto()
            {
                Id = s.SubCategoryId,
                Name = s.SubCategoryCaption
            }).AsEnumerable();

            machine.MachineCheckListCategoryList =
                await connectionDb.QueryAsync<CategoryResutl>("Base.GetMachineInspectionTypes",
                    commandType: CommandType.StoredProcedure);


            return View(machine);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Machine machine)
        {
            if (id != machine.Id)
            {
                return NotFound();
            }

            IDbConnection connectionDb = new SqlConnection(Configuration.GetConnectionString("PeyDejContext_Online"));
            connectionDb.Open();
            //_context.Entry().State = EntityState.Detached;
            if (ModelState.IsValid)
            {
                // get current machine field to compare with new machine field (decide wether to update InspectionDate in Inspecton.MachineIS or not)

                machine.LubricationStartDate = machine.LubricationStartDateDto.ToGregorianDateTime(false, 1200);
                machine.InspectionStartDate = machine.InspectionStartDateDto.ToGregorianDateTime(false, 1200);
                machine.UtilizationDate = machine.UtilizationDateDto.ToGregorianDateTime(false, 1200);

                var l = _context.Machines.AsNoTracking().Where(w => w.Id == machine.Id).FirstOrDefault();
                DateTime? oldMachineInspectionStartDate = l?.InspectionStartDate;
                DateTime? oldMachineLubricationStartDate = l?.LubricationStartDate;
                if (machine.LubricationStartDate == null && oldMachineLubricationStartDate != null)
                {
                    ModelState.AddModelError("LubricationStartDate", "تاریخ شروع روغن کاری اشتباه است");

                    machine.DepartmentIds = _context.VwCategories.Where(w => w.CategoryId == 2).Select(s => new CategoryDto()
                    {
                        Id = s.SubCategoryId,
                        Name = s.SubCategoryCaption
                    }).AsEnumerable();
                    machine.ProcessIds = _context.VwCategories.Where(w => w.CategoryId == 3).Select(s => new CategoryDto()
                    {
                        Id = s.SubCategoryId,
                        Name = s.SubCategoryCaption
                    }).AsEnumerable();

                    machine.MachineCheckListCategoryList =
                        await connectionDb.QueryAsync<CategoryResutl>("Base.GetMachineInspectionTypes",
                            commandType: CommandType.StoredProcedure);

                    return View(machine);
                }
                try
                {
                    _context.Update(machine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MachineExists(machine.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                try
                {

                    // if old value of InspectionStartDate equal to new value then we should not update Inspection.MachineIS
                    // it mean that user do not edite InspectionStartDate of machine 
                    // get machine Inspection that not completed yet
                    var MachineIS = _context.MachineISs.Where(m => m.MachineId == machine.Id && m.Status == 0).FirstOrDefault();
                    var MachineLubricationIS = _context.MachineLubrications.Where(m => m.MachineId == machine.Id && m.Status == 0).FirstOrDefault();
                    if (!DateTime.Equals(oldMachineInspectionStartDate, machine.InspectionStartDate))
                    {
                        var newInspectionDate = machine.InspectionStartDateDto.ToGregorianDateTime(false, 1200);
                        MachineIS.InspectionDate = (DateTime)(newInspectionDate);
                        _context.SaveChanges();
                    }
                    // do the same as above for lubrication
                    if (!DateTime.Equals(oldMachineLubricationStartDate, machine.LubricationStartDate))
                    {
                        var newLubricationDate = machine.LubricationStartDateDto.ToGregorianDateTime(false, 1200);
                        MachineLubricationIS.InspectionDate = (DateTime)(newLubricationDate);
                        _context.SaveChanges();
                    }


                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return RedirectToAction(nameof(Index));
            }

            machine.DepartmentIds = _context.VwCategories.Where(w => w.CategoryId == 2).Select(s => new CategoryDto()
            {
                Id = s.SubCategoryId,
                Name = s.SubCategoryCaption
            }).AsEnumerable();
            machine.ProcessIds = _context.VwCategories.Where(w => w.CategoryId == 3).Select(s => new CategoryDto()
            {
                Id = s.SubCategoryId,
                Name = s.SubCategoryCaption
            }).AsEnumerable();

            machine.MachineCheckListCategoryList =
                await connectionDb.QueryAsync<CategoryResutl>("Base.GetMachineInspectionTypes",
                    commandType: CommandType.StoredProcedure);

            return View(machine);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine != null)
            {
                machine.GeneralStatusId = GeneralStatus.Deleted;
                _context.Machines.Update(machine);
            }

            await _context.SaveChangesAsync();
            return Json(new { hasError = false, message = "" });
        }

        private bool MachineExists(long id)
        {
            return (_context.Machines?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}