﻿using Ccms.Common.Utilities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Bases;
using PeyDej.Models.Dtos;
using PeyDej.Models.Parameters;
using PeyDej.Services.Pagination;
using PeyDej.Tools;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        var data2 = _context.MotorISs
            .Join(_context.Machines, motorIs => motorIs.MotorId,
                machine => machine.Id,
                (motorIs, motor) => new { motorIs, motor })
            .Where((m) => m.motor.GeneralStatusId == GeneralStatus.Active
                          && m.motorIs.InspectionDate >= (start_date + "T01:01:00.000").ToGregorianDateTime(false, 1200) &&
                          m.motorIs.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200))
            .Select(m =>
                new InspectionDto()
                {
                    MachineId = m.motorIs.Id,
                    Name = m.motor.Name,
                    Model = m.motor.Model
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
            .Where((m) => m.motor.GeneralStatusId == GeneralStatus.Active
                          && m.motorIs.InspectionDate >= (start_date + "T01:01:00.000").ToGregorianDateTime(false, 1200) &&
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

        var data2 = _context.MachineISs
            .Join(_context.Machines, machineIs => machineIs.MachineId,
                Machine => Machine.Id,
                (machineIs, machine) => new { MachineIS = machineIs, Machine = machine })
            .Where((m) => m.Machine.GeneralStatusId == GeneralStatus.Active
                          && m.MachineIS.InspectionDate >= (start_date + "T01:01:00.000").ToGregorianDateTime(false, 1200) &&
                          m.MachineIS.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200))
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
        List<string> SelectedFruits,
        string btnName)
    {
        SelectedFruits.Remove("on");
        return btnName switch
        {
            "search" => RedirectToAction("Machine", new { start_date, end_date }),
            "print" => RedirectToAction("MachinePrintPage", new { SelectedFruits }),
            "save" => RedirectToAction("Machine", "InspectionReport", new { SelectedFruits }),
            _ => RedirectToAction("Machine", new { start_date, end_date })
        };
    }

    public async Task<IActionResult> MachinePrintPage(List<string> selectedFruits)
    {
        var listId = selectedFruits.Select(long.Parse).ToList();
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");

        var data2 = _context.MachineISs
            .Join(_context.Machines, machineIs => machineIs.MachineId,
                Machine => Machine.Id,
                (machineIs, machine) => new { MachineIS = machineIs, Machine = machine })
            .Where((m) => m.Machine.GeneralStatusId == GeneralStatus.Active
                          && m.MachineIS.InspectionDate >= (start_date + "T01:01:00.000").ToGregorianDateTime(false, 1200) &&
                          m.MachineIS.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200)&&
                          !listId.Any() || listId.Contains(m.MachineIS.Id))
            .Select(m =>
                new InspectionDto()
                {
                    MachineId = m.MachineIS.Id,
                    Name = m.Machine.Name,
                    Model = m.Machine.Model
                });
        

        ViewBag.items = await _context.VwCategories.Where(m => m.CategoryId == 1).ToListAsync();
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(data2);
    }


    #endregion
}