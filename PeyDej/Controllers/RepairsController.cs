using Ccms.Common.Utilities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Report;
using PeyDej.Service;

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
        var result = Context.RepairRequests.Where(w => w.UserId == User.GetUserIdPrincipal()).AsEnumerable();
        return View(result);
    }


    private IEnumerable<object>
        Parts() => new SelectList(Context.VwCategories.Where(m => m.CategoryId == 15).ToList(),
        "SubCategoryId", "SubCategoryCaption");
    private IEnumerable<object> Machine() => new SelectList(Context.Machines.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).ToList(), "Id", "Name");

    public IActionResult Create()
    {
        ViewBag.Machine = this.Machine();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(RepairRequest repair)
    {
        if (ModelState.IsValid)
        {
            repair.UserId = User.GetUserIdPrincipal();
            Context.RepairRequests.Add(repair);
            Context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Machine = this.Machine();

        return View(repair);
    }

    public IActionResult Print()
    {
        return View();
    }
    //public IActionResult CreateReport(long id)
    //{
    //    var model = new RepairReport()
    //    {
    //        RepairRequestId = id,
    //        People = Context.Persons.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).ToList()
    //    };
    //    ViewBag.Parts = this.Parts();
    //    return View(model);
    //}
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public IActionResult CreateReport(RepairReport report)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        report.Id = 0;
    //        report.StartDate = (DateTime)report.StartDateDto.ToMiladi();
    //        report.EndDate = report.EndDateDto.ToMiladi();
    //        Context.RepairReports.Add(report);
    //        Context.SaveChanges();
    //        return RedirectToAction(nameof(Index));
    //    }
    //    ViewBag.Parts = this.Parts();
    //    return View(report);
    //}
}
