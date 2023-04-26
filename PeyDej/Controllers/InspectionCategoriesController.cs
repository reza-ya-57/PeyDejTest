using DNTPersianUtils.Core;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Parameters;
using PeyDej.Tools;

namespace PeyDej.Controllers;

public class InspectionCategoriesController : Controller
{
    private readonly PeyDejContext _context;

    public InspectionCategoriesController(PeyDejContext context)
    {
        _context = context;
    }

    // GET
    public async Task<IActionResult> Index(int id, string start_date, string end_date)
    {
        start_date ??= PeyDejTools.GetCurPersianDate();
        end_date ??= PeyDejTools.GetCurPersianDate();
        var data = await _context.VwInspectionSubCategoryISs
            .Where(m =>
                m.Status == false &&
                m.InspectionDate >= (start_date + "T01:01:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionCategoryId == id
            ).ToListAsync();

        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);
        HttpContext.Session.SetInt32("id", id);

        ViewBag.title = _context.InspectionCategories.FirstOrDefault(m => m.Id == id)!.Caption;
        SessionExtensions.SetString(HttpContext.Session, "title", ViewBag.title);
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        return View(data);
    }

    public async Task<IActionResult> PrintPage()
    {
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");
        var title = HttpContext.Session.GetString("title");
        var id = HttpContext.Session.GetInt32("id") ?? 0;

        var data = await _context.VwInspectionSubCategoryISs
            .Where(m =>
                m.Status == false &&
                m.InspectionDate >= (start_date + "T01:01:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionCategoryId == id
            ).ToListAsync();

        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);
        HttpContext.Session.SetString("title", title);
        HttpContext.Session.SetInt32("id", id);
        ViewBag.title = _context.InspectionCategories.FirstOrDefault(m => m.Id == id)!.Caption;
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        ViewBag.title = title;
        return View(data);
    }


    public async Task<IActionResult> GetDataReport()
    {
        var start_date = HttpContext.Session.GetString("start_date");
        var end_date = HttpContext.Session.GetString("end_date");
        var title = HttpContext.Session.GetString("title");
        var id = HttpContext.Session.GetInt32("id") ?? 0;

        var data = await _context.VwInspectionSubCategoryISs
            .Where(m =>
                m.Status == false &&
                m.InspectionDate >= (start_date + "T01:01:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionDate <= (end_date + "T23:59:00.000").ToGregorianDateTime(false, 1200) &&
                m.InspectionCategoryId == id
            ).ToListAsync();

        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);
        HttpContext.Session.SetString("title", title);
        HttpContext.Session.SetInt32("id", id);
        ViewBag.title = _context.InspectionCategories.FirstOrDefault(m => m.Id == id)!.Caption;
        var person = await _context.Persons.Where(m => m.GeneralStatusId == GeneralStatus.Active)
            .Select(m => new { m.Id, Name = m.FirstName + " " + m.LastName })
            .ToListAsync();
        ViewBag.person = new SelectList(person, "Id", "Name");
        ViewBag.startDate = start_date;
        ViewBag.endDate = end_date;
        ViewBag.title = title;
        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> SaveDataReport(int person, ReportMotorStatusParameter[] data)
    {
        try
        {
            var query = "DECLARE @mi Inspection.InspectionSubCategoryTT;\n";
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

            query += "EXEC [Inspection].[InspectionSubCategorySetInspection] @InspectionSubCategory = @mi;\n";

            await _context.Database.ExecuteSqlRawAsync(query);
            return Json(new { r = true });
        }
        catch (Exception ex)
        {
            return Json(new { r = false, m = ex.Message });
        }
    }
}