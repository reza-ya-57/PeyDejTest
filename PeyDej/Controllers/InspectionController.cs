using Ccms.Common.Utilities;

using Dapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Bases;
using PeyDej.Models.Dtos;
using PeyDej.Services.Pagination;
using PeyDej.Tools;

using System.Data;

namespace PeyDej.Controllers;

[Authorize(Roles = "Admin")]
public class InspectionController : Controller
{
    private readonly PeyDejContext _context;

    public InspectionController(PeyDejContext context, IConfiguration configuration)
    {
        _context = context;
        Configuration = configuration;
    }
    private IConfiguration Configuration { get; }
    #region HistoryMotor
    public async Task<IActionResult> HistoryMotor(
        string start_date,
        string end_date,
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
        start_date ??= PeyDejTools.GetCurPersianDate();
        end_date ??= PeyDejTools.GetCurPersianDate();
        var data2 = _context.MotorISs
            .Join(_context.Motors, motorIs => motorIs.MotorId,
                machine => machine.Id,
                (motorIs, motor) => new { motorIs, motor })
            .Where((m) => m.motor.GeneralStatusId == GeneralStatus.Active &&
                          m.motorIs.Status == InspectionStatus.Ok &&
                          m.motorIs.InspectionDate >= (start_date + "T00:00:00.000").ToGregorianDateTime(false, 1200) &&
                          m.motorIs.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200))
            .OrderByDescending(s => s.motorIs.InspectionFinishedDate)
            .Select(m =>
                new InspectionDto()
                {
                    MachineId = m.motorIs.Id,
                    Name = m.motor.Name,
                    //Model = m.motor.Model
                });
        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        ViewData["CurrentFilter"] = searchString;

        var result = await PaginatedList<InspectionDto>.CreateAsync(data2, pageIndex: pageNumber, pageSize);
        return View(result);
    }
    [HttpPost]
    public IActionResult HistoryMotor(
        string start_date,
        string end_date,
        List<string> SelectedFruits,
        string btnName)
    {
        SelectedFruits.Remove("on");
        return btnName switch
        {
            "search" => RedirectToAction("HistoryMotor", new { start_date, end_date }),
            "save" => RedirectToAction("HistoryMotor", "InspectionReport", new { SelectedFruits }),
            _ => RedirectToAction("HistoryMotor", new { start_date, end_date })
        };
    }
    #endregion

    #region Motor
    public async Task<IActionResult> Motor(
        string start_date,
        string end_date,
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
        start_date ??= PeyDejTools.GetCurPersianDate();
        end_date ??= PeyDejTools.GetCurPersianDate();
        var data2 = _context.MotorISs
            .Join(_context.Motors, motorIs => motorIs.MotorId,
                machine => machine.Id,
                (motorIs, motor) => new { motorIs, motor })
            .Where((m) => m.motor.GeneralStatusId == GeneralStatus.Active &&
                          m.motorIs.Status == InspectionStatus.NotOk &&
                          m.motorIs.InspectionDate >= (start_date + "T00:00:00.000").ToGregorianDateTime(false, 1200) &&
                          m.motorIs.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200))
            .Select(m =>
                new InspectionDto()
                {
                    MachineId = m.motorIs.Id,
                    Name = m.motor.Name,
                    //Model = m.motor.Model
                });
        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        ViewData["CurrentFilter"] = searchString;

        var result = await PaginatedList<InspectionDto>.CreateAsync(data2, pageIndex: pageNumber, pageSize);
        return View(result);
    }

    [HttpPost]
    public IActionResult Motor(
        string start_date,
        string end_date,
        List<string> SelectedFruits,
        string btnName)
    {
        SelectedFruits.Remove("on");
        return btnName switch
        {
            "search" => RedirectToAction("Motor", new { start_date, end_date }),
            "print" => RedirectToAction("MotorPrintPage", new { SelectedFruits }),
            "save" => RedirectToAction("Motor", "InspectionReport", new { SelectedFruits }),
            _ => RedirectToAction("Motor", new { start_date, end_date })
        };
    }
    public async Task<IActionResult> MotorPrintPage(List<string> selectedFruits)
    {
        var listId = selectedFruits.Select(long.Parse).ToList();
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");

        var data2 = _context.MotorISs
            .Join(_context.Machines, motorIs => motorIs.MotorId,
                machine => machine.Id,
                (motorIs, motor) => new { motorIs, motor })
            .Where((m) => m.motor.GeneralStatusId == GeneralStatus.Active &&
                          m.motorIs.Status == InspectionStatus.NotOk &&
                          m.motorIs.InspectionDate >= (start_date + "T00:00:00.000").ToGregorianDateTime(false, 1200) &&
                          m.motorIs.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200) &&
                          !listId.Any() || listId.Contains(m.motorIs.Id))
            .Select(m =>
                new InspectionDto()
                {
                    MachineId = m.motorIs.Id,
                    Name = m.motor.Name,
                    Model = m.motor.Model
                });

        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(data2);
    }
    #endregion

    #region HsitoryMachine

    public async Task<IActionResult> HistoryMachine(
        string start_date,
        string end_date,
        long machineCheckListCategoryId,
        string sortOrder,
        string currentFilter,
        string searchString,
        int pageNumber = 1,
        int pageSize = 100)
    {
        ViewBag.machineCheckListCategoryId = machineCheckListCategoryId;
        IDbConnection connectionDb = new SqlConnection(Configuration.GetConnectionString("PeyDejContext_Online"));
        connectionDb.Open();
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
        ViewData["MachineInspectionTypes"] =
            await connectionDb.QueryAsync<CategoryResutl>("Base.GetMachineInspectionTypes",
                commandType: CommandType.StoredProcedure);
        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }
        start_date ??= PeyDejTools.GetCurPersianDate();
        end_date ??= PeyDejTools.GetCurPersianDate();

        var data2 = _context.MachineISs
            .Join(_context.Machines, machineIs => machineIs.MachineId,
                Machine => Machine.Id,
                (machineIs, machine) => new { MachineIS = machineIs, Machine = machine })
            .Where((m) => m.Machine.GeneralStatusId == GeneralStatus.Active &&
                          m.MachineIS.Status == InspectionStatus.Ok &&
                          m.MachineIS.InspectionDate >= (start_date + "T00:00:00.000").ToGregorianDateTime(false, 1200) &&
                          m.MachineIS.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200))
            .Where(w => machineCheckListCategoryId == 0 || w.Machine.MachineInspectionTypeCategoryId == machineCheckListCategoryId)
            .OrderByDescending(o => o.MachineIS.InspectionFinishedDate)
            .Select(m =>
                new InspectionDto()
                {
                    MachineId = m.MachineIS.Id,
                    Name = m.Machine.Name,
                    Model = m.Machine.Model
                });
        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        ViewData["CurrentFilter"] = searchString;

        var result = await PaginatedList<InspectionDto>.CreateAsync(data2, pageIndex: pageNumber, pageSize);
        return View(result);
    }


    [HttpPost]
    public IActionResult HistoryMachine(
        string start_date,
        string end_date,
        long machineCheckListCategoryId,
        List<string> SelectedFruits,
        string btnName)
    {
        ViewBag.machineCheckListCategoryId = machineCheckListCategoryId;
        SelectedFruits.Remove("on");
        return btnName switch
        {
            "search" => RedirectToAction("HistoryMachine", new { start_date, end_date, machineCheckListCategoryId }),
            "save" => RedirectToAction("HistoryMachine", "InspectionReport", new { SelectedFruits, machineCheckListCategoryId }),
            _ => RedirectToAction("HistoryMachine", new { start_date, end_date })
        };
    }

    #endregion

    #region Machine

    public async Task<IActionResult> Machine(
        string start_date,
        string end_date,
        long machineCheckListCategoryId,
        string sortOrder,
        string currentFilter,
        string searchString,
        int pageNumber = 1,
        int pageSize = 100)
    {
        ViewBag.machineCheckListCategoryId = machineCheckListCategoryId;
        IDbConnection connectionDb = new SqlConnection(Configuration.GetConnectionString("PeyDejContext_Online"));
        connectionDb.Open();
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
        ViewData["MachineInspectionTypes"] =
            await connectionDb.QueryAsync<CategoryResutl>("Base.GetMachineInspectionTypes",
                commandType: CommandType.StoredProcedure);
        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }
        start_date ??= PeyDejTools.GetCurPersianDate();
        end_date ??= PeyDejTools.GetCurPersianDate();

        var data2 = _context.MachineISs
            .Join(_context.Machines, machineIs => machineIs.MachineId,
                Machine => Machine.Id,
                (machineIs, machine) => new { MachineIS = machineIs, Machine = machine })
            .Where((m) => m.Machine.GeneralStatusId == GeneralStatus.Active &&
                          m.MachineIS.Status == InspectionStatus.NotOk &&
                          m.MachineIS.InspectionDate >= (start_date + "T00:00:00.000").ToGregorianDateTime(false, 1200) &&
                          m.MachineIS.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200))
            .Where(w => machineCheckListCategoryId == 0 || w.Machine.MachineInspectionTypeCategoryId == machineCheckListCategoryId)
            .Select(m =>
                new InspectionDto()
                {
                    MachineId = m.MachineIS.Id,
                    Name = m.Machine.Name,
                    Model = m.Machine.Model
                });
        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        ViewData["CurrentFilter"] = searchString;

        var result = await PaginatedList<InspectionDto>.CreateAsync(data2, pageIndex: pageNumber, pageSize);
        return View(result);
    }


    [HttpPost]
    public IActionResult Machine(
        string start_date,
        string end_date,
        long machineCheckListCategoryId,
        List<string> SelectedFruits,
        string btnName)
    {
        ViewBag.machineCheckListCategoryId = machineCheckListCategoryId;
        SelectedFruits.Remove("on");
        return btnName switch
        {
            "search" => RedirectToAction("Machine", new { start_date, end_date, machineCheckListCategoryId }),
            "print" => RedirectToAction("MachinePrintPage", new { SelectedFruits, machineCheckListCategoryId, target = "_blank" }),
            "save" => RedirectToAction("Machine", "InspectionReport", new { SelectedFruits, machineCheckListCategoryId }),
            _ => RedirectToAction("Machine", new { start_date, end_date })
        };
    }

    public async Task<IActionResult> MachinePrintPage(List<string> selectedFruits, long machineCheckListCategoryId)
    {
        var listId = selectedFruits.Select(long.Parse).ToList();
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");

        var data2 = _context.MachineISs
            .Join(_context.Machines, machineIs => machineIs.MachineId,
                Machine => Machine.Id,
                (machineIs, machine) => new { MachineIS = machineIs, Machine = machine })
            .Where((m) => m.Machine.GeneralStatusId == GeneralStatus.Active &&
                          m.MachineIS.Status == InspectionStatus.NotOk &&
                          m.MachineIS.InspectionDate >= (start_date + "T00:00:00.000").ToGregorianDateTime(false, 1200) &&
                          m.MachineIS.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200) &&
                          !listId.Any() || listId.Contains(m.MachineIS.Id))
            .Where(w => machineCheckListCategoryId == 0 || w.Machine.MachineInspectionTypeCategoryId == machineCheckListCategoryId)
            .Select(m =>
                new InspectionDto()
                {
                    MachineId = m.MachineIS.Id,
                    Name = m.Machine.Name,
                    Model = m.Machine.Model
                });


        ViewBag.items = await _context.VwCategories.Where(m => m.CategoryId == machineCheckListCategoryId).ToListAsync();
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(data2);
    }


    #endregion
}