using Ccms.Common.Utilities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Bases;
using PeyDej.Models.Dtos;
using PeyDej.Models.Parameters;

using System.Data;

namespace PeyDej.Controllers;

[Authorize(Roles = "Admin,Lubricator")]
public class InspectionReportController : Controller
{
    private readonly PeyDejContext _context;

    public InspectionReportController(PeyDejContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Motor(List<string> selectedFruits)
    {
        var listId = selectedFruits.Select(long.Parse).ToList();
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");

        var data2 = _context.MotorISs
            .Join(_context.Motors, motorIs => motorIs.MotorId,
                machine => machine.Id,
                (motorIs, motor) => new { motorIs, motor })
            .Where((m) => m.motor.GeneralStatusId == GeneralStatus.Active &&
                m.motorIs.Status == InspectionStatus.NotOk &&
                m.motorIs.InspectionDate >= (start_date + "T00:00:00.000").ToGregorianDateTime(false, 1200) &&
                m.motorIs.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200) &&
                !listId.Any() || listId.Contains(m.motorIs.Id))
            .OrderByDescending(o => o.motorIs.InspectionFinishedDate)
            .Select(m =>
                new InspectionDto()
                {
                    MachineId = m.motorIs.Id,
                    Name = m.motor.Name,
                    //Model = m.motor.Model
                }).AsEnumerable();

        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);

        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        var person = await _context.Persons.Where(m => m.GeneralStatusId == GeneralStatus.Active)
            .Select(m => new { m.Id, Name = m.FirstName + " " + m.LastName })
            .ToListAsync();
        ViewBag.person = new SelectList(person, "Id", "Name");
        return View(data2);
    }


    public async Task<IActionResult> HistoryMotor(List<string> selectedFruits)
    {
        var listId = selectedFruits.Select(long.Parse).ToList();
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");


        var data2 = _context.MotorISs
            .Join(_context.Motors, motorIs => motorIs.MotorId,
                machine => machine.Id,
                (motorIs, motor) => new { motorIs, motor })
            .Join(_context.MotorIRs,
                motor => motor.motorIs.Id,
                motorIr => motorIr.MotorInspectionId,
                (motor, motorIr) => new { motor.motorIs, motor.motor, motorIr })
            .Where(m => m.motor.GeneralStatusId == GeneralStatus.Active &&
                        m.motorIs.Status == InspectionStatus.Ok &&
                        m.motorIs.InspectionDate >= (start_date + "T00:00:00.000").ToGregorianDateTime(false, 1200) &&
                        m.motorIs.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200) &&
                        (!listId.Any() || listId.Contains(m.motorIs.Id)))
            .OrderByDescending(o => o.motorIs.InspectionFinishedDate)
            .Select(m => new InspectionDto()
            {
                MachineId = m.motorIs.Id,
                Name = m.motor.Name,
                //Model = m.motor.Model,
                Status = m.motorIr.Status,
                Description = m.motorIr.Description
            }).AsEnumerable();
        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);

        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        var person = await _context.Persons.Where(m => m.GeneralStatusId == GeneralStatus.Active)
            .Select(m => new { m.Id, Name = m.FirstName + " " + m.LastName })
            .ToListAsync();
        ViewBag.person = new SelectList(person, "Id", "Name");
        return View(data2);
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
                query += $"({item.Id},{person},{status},'{item.Description}'), ";
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


    public async Task<IActionResult> Machine(List<string> selectedFruits, long machineCheckListCategoryId)
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
                }).AsEnumerable();

        var person = await _context.Persons.Where(m => m.GeneralStatusId == GeneralStatus.Active)
            .Select(m => new { m.Id, Name = m.FirstName + " " + m.LastName })
            .ToListAsync();



        ViewBag.person = new SelectList(person, "Id", "Name");
        ViewBag.items = await _context.VwCategories.Where(m => m.CategoryId == machineCheckListCategoryId).ToListAsync();
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(data2);
    }



    public async Task<IActionResult> HistoryMachine(List<string> selectedFruits, long machineCheckListCategoryId)
    {
        var listId = selectedFruits.Select(long.Parse).ToList();
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");

        var data2 = _context.MachineISs
            .Join(_context.Machines, machineIs => machineIs.MachineId,
                machine => machine.Id,
                (machineIs, machine) => new { MachineIS = machineIs, Machine = machine })
            .Where(m =>
                m.Machine.GeneralStatusId == GeneralStatus.Active &&
                m.MachineIS.Status == InspectionStatus.Ok &&
                m.MachineIS.InspectionDate >= (start_date + "T00:00:00.000").ToGregorianDateTime(false, 1200) &&
                m.MachineIS.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200) &&
                !listId.Any() || listId.Contains(m.MachineIS.Id))
            .Where(w => machineCheckListCategoryId == 0 || w.Machine.MachineInspectionTypeCategoryId == machineCheckListCategoryId)
            .Select(m =>
                new InspectionDto()
                {
                    MachineId = m.MachineIS.Id,
                    Name = m.Machine.Name,
                    Model = m.Machine.Model,
                }).ToList();
        var machineIrs = _context.MachineIRs.Where(m => listId.Any() || listId.Contains(m.Id)).ToList();
        for (int i = 0; i < data2.Count; i++)
        {
            var machineIr = machineIrs.FirstOrDefault(f => f.MachineInspectionId == data2[i].MachineId);
            data2[i].Status = machineIr.Status;
            data2[i].PersonId = machineIr.PersonId;
            data2[i].Description = machineIr.Description;
        }
        var person = await _context.Persons.Where(m => m.GeneralStatusId == GeneralStatus.Active)
            .Select(m => new { m.Id, Name = m.FirstName + " " + m.LastName })
            .ToListAsync();
        ViewBag.person = new SelectList(person, "Id", "Name");

        ViewBag.items = await _context.VwCategories.Where(m => m.CategoryId == machineCheckListCategoryId).ToListAsync();
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(data2);
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
                m.InspectionDate >= (start_date + "T00:00:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionFinishedDate == null
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