using Ccms.Common.Utilities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Users;
using PeyDej.Tools;

namespace PeyDej.Controllers;

[Authorize]
public class InspectionController : Controller
{
    private readonly PeyDejContext _context;

    public InspectionController(PeyDejContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Motor(string start_date, string end_date)
    {
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
        var result = await _context.Motors.Where(m => motorIDs.Contains(m.Id)).ToListAsync();
        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);

        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(result);
    }

    public async Task<IActionResult> MotorPrintPage()
    {
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");
        var data = await _context.MotorISs
            .Where(m =>
                m.Status == InspectionStatus.NotOk &&
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

    public async Task<IActionResult> Machine(string start_date, string end_date)
    {
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
        var result = await _context.Machines.Where(m => machineIDs.Contains(m.Id)).ToListAsync();
        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);

        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(result);
    }

    public async Task<IActionResult> MachinePrintPage()
    {
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");
        var data = await _context.MachineISs
            .Where(m =>
                m.Status == InspectionStatus.NotOk &&
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
}