using Ccms.Common.Utilities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Report;

namespace PeyDej.Controllers;

[Authorize]
public class RepairsController : Controller
{
    public PeyDejContext Context { get; }

    public RepairsController(PeyDejContext context)
    {
        Context = context;
    }

    public IActionResult Index()
    {
        var result = Context.RepairRequests.AsEnumerable();
        return View(result);
    }

    private IEnumerable<object>
     CategoryId(int id) => new SelectList(Context.VwCategories.Where(m => m.CategoryId == id).ToList(),
     "SubCategoryId", "SubCategoryCaption");
    private IEnumerable<object>
        Parts() => new SelectList(Context.VwCategories.Where(m => m.CategoryId == 15).ToList(),
        "SubCategoryId", "SubCategoryCaption");
    private IEnumerable<object> Machine() => new SelectList(Context.Machines.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).ToList(), "Id", "Name");
    private IEnumerable<object> Motor() => new SelectList(Context.Motors.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).ToList(), "Id", "Name");
    private IEnumerable<object> SparePart() => new SelectList(Context.SpareParts.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).ToList(), "Id", "Name");


    public IActionResult Create()
    {
        ViewBag.Parts = this.Parts();
        ViewBag.Department = this.CategoryId(2);
        ViewBag.Process = this.CategoryId(3);
        ViewBag.RepairKind = this.CategoryId(14);
        ViewBag.Machine = this.Machine();
        ViewBag.Motor = this.Motor();
        ViewBag.SparePart = this.SparePart();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(RepairRequest repair)
    {
        if (ModelState.IsValid)
        {
            repair.Date = repair.DateDto.ToMiladi();
            Context.RepairRequests.Add(repair);
            Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Parts = this.Parts();
        ViewBag.Department = this.CategoryId(2);
        ViewBag.Process = this.CategoryId(3);
        ViewBag.RepairKind = this.CategoryId(14);
        ViewBag.Machine = this.Machine();
        ViewBag.Motor = this.Motor();
        ViewBag.SparePart = this.SparePart();
        return View(repair);
    }

    public IActionResult CreateReport(long id)
    {
        var model = new RepairReport()
        {
            RepairRequestId = id,
            People = Context.Persons.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).ToList()
        };
        ViewBag.Parts = this.Parts();
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateReport(RepairReport report)
    {
        if (ModelState.IsValid)
        {
            report.Id = 0;
            report.StartDate = (DateTime)report.StartDateDto.ToMiladi();
            report.EndDate = report.EndDateDto.ToMiladi();
            Context.RepairReports.Add(report);
            Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Parts = this.Parts();
        return View(report);
    }
}
