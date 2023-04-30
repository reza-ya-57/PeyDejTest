using Ccms.Common.Utilities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Bases;
using PeyDej.Models.Parameters;
using PeyDej.Services.Pagination;
using PeyDej.Tools;

using System.Collections.Generic;

namespace PeyDej.Controllers;

[Authorize]
public class InspectionController : Controller
{
    private readonly PeyDejContext _context;

    public InspectionController(PeyDejContext context)
    {
        _context = context;
    }

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
        var data = await _context.MotorISs
            .Where(m =>
                m.Status == InspectionStatus.NotOk &&
                m.InspectionDate >= (start_date + "T01:01:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionFinishedDate == null
            ).ToListAsync();

        var motorIDs = data.Select(item => item.MotorId).ToList();
        var students = _context.Motors.Where(m => motorIDs.Contains(m.Id)).AsQueryable();
        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);

        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        ViewData["CurrentFilter"] = searchString;

        var result = await PaginatedList<Motor>.CreateAsync(students, pageIndex: pageNumber, pageSize);
        return View(result);
    }

    [HttpPost]
    public IActionResult Motor(
        string start_date,
        string end_date,
        List<string> SelectedFruits,
        string btnName)
    {
        if (btnName == "search")
        {
            return RedirectToAction("Motor", new { start_date, end_date });
        }
        else if (btnName == "print")
        {
            return RedirectToAction("MotorPrintPage", new { SelectedFruits });

        }
        else if (btnName == "save")
        {
            return RedirectToAction("Motor", "InspectionReport", new { SelectedFruits });
        }
        return RedirectToAction("Motor", new { start_date, end_date });
    }
    public async Task<IActionResult> MotorPrintPage(List<string> selectedFruits)
    {
        var listId = selectedFruits.Select(long.Parse).ToList();
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");
        var data = await _context.MotorISs
            .Where(m =>
                m.Status == InspectionStatus.NotOk &&
                !listId.Any() || listId.Contains(m.MotorId ?? 0) &&
                m.InspectionDate >= (start_date + "T01:01:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionFinishedDate == null
            ).ToListAsync();

        var motorIDs = data.Select(item => item.MotorId).ToList();
        var result = await _context.Motors.Where(m => motorIDs.Contains(m.Id)).ToListAsync();

        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(result);
    }
    #endregion

    #region Machine

    public async Task<IActionResult> Machine(
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
        var data = await _context.MachineISs
            .Where(m =>
                m.Status == InspectionStatus.NotOk &&
                m.InspectionDate >= (start_date + "T01:01:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionFinishedDate == null
            ).ToListAsync();

        var machineIDs = data.Select(item => item.MachineId).ToList();
        var students = _context.Machines.Where(m => machineIDs.Contains(m.Id)).AsQueryable();
        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);

        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        ViewData["CurrentFilter"] = searchString;

        var result = await PaginatedList<Machine>.CreateAsync(students, pageIndex: pageNumber, pageSize);
        return View(result);
    }


    [HttpPost]
    public IActionResult Machine(
        string start_date,
        string end_date,
        List<string> SelectedFruits,
        string btnName)
    {
        if (btnName == "search")
        {
            return RedirectToAction("Machine", new { start_date, end_date });
        }
        else if (btnName == "print")
        {
            return RedirectToAction("MachinePrintPage", new { SelectedFruits });

        }
        else if (btnName == "save")
        {
            return RedirectToAction("Machine", "InspectionReport", new { SelectedFruits });
        }
        return RedirectToAction("Machine", new { start_date, end_date });
    }

    public async Task<IActionResult> MachinePrintPage(List<string> selectedFruits)
    {
        var listId = selectedFruits.Select(long.Parse).ToList();
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");
        var data = await _context.MachineISs
            .Where(m =>
                m.Status == InspectionStatus.NotOk &&
                !listId.Any() || listId.Contains(m.MachineId) &&
                m.InspectionDate >= (start_date + "T01:01:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionFinishedDate == null
            ).ToListAsync();

        var machineIDs = data.Select(item => item.MachineId).ToList();
        var result = await _context.Machines.Where(m => machineIDs.Contains(m.Id)).ToListAsync();

        ViewBag.items = await _context.VwCategories.Where(m => m.CategoryId == 1).ToListAsync();
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(result);
    }


    #endregion
}