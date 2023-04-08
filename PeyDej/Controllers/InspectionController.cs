using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Users;
using PeyDej.Tools;

namespace PeyDej.Controllers;

public class InspectionController : Controller
{
    private readonly PeyDejContext _context;

    public InspectionController(PeyDejContext context)
    {
        this._context = context;
    }

    public async Task<IActionResult> Motor(string start_date, string end_date)
    {
        start_date ??= PeyDejTools.GetCurPersianDate();
        end_date ??= PeyDejTools.GetCurPersianDate();
        var data = await _context.MotorISs
            .Where(m =>
                m.Status == InspectionStatus.NotOk &&
                m.InspectionDate >= PeyDejTools.PersianStringToDateTime(start_date) &&
                m.InspectionDate <= PeyDejTools.PersianStringToDateTime(end_date)
            ).ToListAsync();

        var motorIDs = data.Select(item => item.MotorId).ToList();
        var result = await _context.Motors.Where(m => motorIDs.Contains(m.Id)).ToListAsync();
        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);

        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(result);
    }

    public async Task<IActionResult> PrintPage()
    {
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");
        var data = await _context.MotorISs
            .Where(m =>
                m.Status == InspectionStatus.NotOk &&
                m.InspectionDate >= PeyDejTools.PersianStringToDateTime(start_date) &&
                m.InspectionDate <= PeyDejTools.PersianStringToDateTime(end_date)
            ).ToListAsync();

        var motorIDs = data.Select(item => item.MotorId).ToList();
        var result = await _context.Motors.Where(m => motorIDs.Contains(m.Id)).ToListAsync();

        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(result);
    }
}