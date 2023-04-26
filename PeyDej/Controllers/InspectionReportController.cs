using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.ActiveModels;
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

    public async Task<IActionResult> Motor()
    {
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");
        var data = await _context.MotorISs
            .Where(m =>
                m.Status == InspectionStatus.NotOk &&
                m.InspectionDate >= PeyDejTools.PersianStringToDateTime(start_date) &&
                m.InspectionDate <= PeyDejTools.PersianStringToDateTime(end_date)
            ).Join(_context.Motors, mIS => mIS.MotorId, mo => mo.Id, (mIS, motor) => new MotorReport
            {
                Id = mIS.Id,
                Name = motor.Name,
                Description = motor.Description
            }).ToListAsync();

        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);

        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        var person = await _context.Persons.Where(m => m.GeneralStatusId == GeneralStatus.Active)
            .Select(m => new { m.Id, Name = m.FirstName + " " + m.LastName })
            .ToListAsync();
        ViewBag.person = new SelectList(person, "Id", "Name");
        return View(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveMotorReport(int person, ReportMotorStatusParameter[] data)
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


    public async Task<IActionResult> Machine()
    {
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");

        var data = await _context.MachineISs
            .Where(m =>
                m.Status == InspectionStatus.NotOk &&
                m.InspectionDate >= PeyDejTools.PersianStringToDateTime(start_date) &&
                m.InspectionDate <= PeyDejTools.PersianStringToDateTime(end_date)
            ).ToListAsync();

        var machineIDs = data.Select(item => item.MachineId).ToList();
        var result = await _context.Machines.Where(m => machineIDs.Contains(m.Id)).ToListAsync();
        var person = await _context.Persons.Where(m => m.GeneralStatusId == GeneralStatus.Active)
            .Select(m => new { m.Id, Name = m.FirstName + " " + m.LastName })
            .ToListAsync();
        ViewBag.person = new SelectList(person, "Id", "Name");
        ViewBag.items = await _context.VwCategories.Where(m => m.CategoryId == 1).ToListAsync();
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveMachineReport(int person, ReportMachineStatusParameter[] data)
    {
        try
        {
            var query = "DECLARE @mi Inspection.MachineInspectionTT;\n";
            query += "INSERT INTO @mi (InspectionId,  CriteriaId, PersonId , [Status] , [Description]) Values ";
            foreach (var item in data)
            {
                var status = (int)(item.value == 1 ? InspectionStatus.Ok : InspectionStatus.NotOk);
                query += $"({item.Id},{item.subId},{person},{status},NULL), ";
            }

            if (data.Length > 0)
            {
                query = query[..^2];
                query += ";\n";
            }

            query += "EXEC [Inspection].[MachineSetInspection] @MachineInspection = @mi;\n";

            await _context.Database.ExecuteSqlRawAsync(query);
            return Json(new { r = true });
        }
        catch (Exception ex)
        {
            return Json(new { r = false, m = ex.Message });
        }
    }




    public async Task<IActionResult> MachineLubrications()
    {
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");

        var data = await _context.MachineLubrications
            .Where(m =>
                m.Status == 0 &&
                m.InspectionDate >= PeyDejTools.PersianStringToDateTime(start_date) &&
                m.InspectionDate <= PeyDejTools.PersianStringToDateTime(end_date)
            ).ToListAsync();

        var machineIDs = data.Select(item => item.MachineId).ToList();
        var result = await _context.Machines.Where(m => machineIDs.Contains(m.Id)).ToListAsync();
        var person = await _context.Persons.Where(m => m.GeneralStatusId == GeneralStatus.Active)
            .Select(m => new { m.Id, Name = m.FirstName + " " + m.LastName })
            .ToListAsync();
        ViewBag.person = new SelectList(person, "Id", "Name");
        ViewBag.items = await _context.VwCategories.Where(m => m.CategoryId == 1).ToListAsync();
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveMachineLubricationsReport(int person, ReportMachineStatusParameter[] data)
    {
        try
        {
            var query = "DECLARE @mi Inspection.MachineLubricationInspectionTT;\n";
            query += "INSERT INTO @mi (InspectionId,  CriteriaId, PersonId , [Status] , [Description]) Values ";
            foreach (var item in data)
            {
                var status = (int)(item.value == 1 ? InspectionStatus.Ok : InspectionStatus.NotOk);
                query += $"({item.Id},{item.subId},{person},{status},NULL), ";
            }

            if (data.Length > 0)
            {
                query = query[..^2];
                query += ";\n";
            }

            query += "EXEC [Inspection].[MachineLubricationSetInspection] @MachineLubricationInspection = @mi;\n";

            await _context.Database.ExecuteSqlRawAsync(query);
            return Json(new { r = true });
        }
        catch (Exception ex)
        {
            return Json(new { r = false, m = ex.Message });
        }
    }
}