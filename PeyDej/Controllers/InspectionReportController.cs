using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Parameters;
using PeyDej.Models.Users;
using PeyDej.Tools;

namespace PeyDej.Controllers;

// [Authorize]
public class InspectionReportController : Controller
{
    private readonly PeyDejContext _context;

    public InspectionReportController(PeyDejContext context)
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
                m.InspectionDate >= PeyDejTools.PersianStringToDateTime(start_date) &&
                m.InspectionDate <= PeyDejTools.PersianStringToDateTime(end_date)
            ).ToListAsync();

        var motorIDs = data.Select(mis => mis.MotorId).ToList();

        var result = await _context.Motors.Where(m => motorIDs.Contains(m.Id)).ToListAsync();
        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);

        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        var person = await _context.Persons.Where(m => m.GeneralStatusId == GeneralStatus.Active)
            .Select(m => new { m.Id, Name = m.FirstName + " " + m.LastName })
            .ToListAsync();
        ViewBag.person = new SelectList(person, "Id", "Name");
        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveMotorReport(int person, ReportStatusParameter[] data)
    {
        try
        {
            var query = "DECLARE @mi Inspection.MotorInspectionTT;\n";
            query += "INSERT INTO @mi (InspectionId,  PersonId , [Status] , [Description]) Values ";
            foreach (var item in data)
            {
                var status = (int)(item.status ? InspectionStatus.Ok : InspectionStatus.NotOk);
                query += $"({item.Id},{person},{status},NULL), ";
            }

            if (data.Length > 0)
            {
                query = query[..^2];
                query += ";\n";
            }

            query += "EXEC [Inspection].[MotorSetInspection] @MotorInspection = @mi;\n";

            await _context.Database.ExecuteSqlRawAsync(query);
            return Json(new { r = true });
        }
        catch (Exception ex)
        {
            return Json(new { r = false, m = ex.Message });
        }
    }
}