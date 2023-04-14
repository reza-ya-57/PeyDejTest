using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
using PeyDej.Models;
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
                m.InspectionDate >= PeyDejTools.PersianStringToDateTime(start_date) &&
                m.InspectionDate <= PeyDejTools.PersianStringToDateTime(end_date) &&
                m.InspectionCategoryId == id
            ).ToListAsync();

        HttpContext.Session.SetString("start_date", start_date);
        HttpContext.Session.SetString("end_date", end_date);
        HttpContext.Session.SetInt32("id", id);

        ViewBag.title = _context.InspectionCategories.FirstOrDefault(m => m.Id == id)!.Caption;
        HttpContext.Session.SetString("title", ViewBag.title);
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
                m.InspectionDate >= PeyDejTools.PersianStringToDateTime(start_date) &&
                m.InspectionDate <= PeyDejTools.PersianStringToDateTime(end_date) &&
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
}