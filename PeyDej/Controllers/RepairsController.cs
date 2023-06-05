using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Report;
using PeyDej.Service;

using System.Linq.Expressions;

namespace PeyDej.Controllers;


[Authorize]
public class RepairsController : Controller
{
    public PeyDejContext Context { get; }

    public RepairsController(PeyDejContext context)
    {
        Context = context;
    }

    public IActionResult RequestIndex()
    {
        Expression<Func<RepairRequest, bool>> where = w => true;
        if (User.IsInRole("Admin"))
        {
            where = w => !Context.RepairUnitAgendumOrders.Any(order => order.RepairRequestId == w.Id);
        }
        else if (User.IsInRole("Lubricator"))
        {
            where = w => w.UserId == User.GetUserIdPrincipal();
        }
        var result = Context.RepairRequests.Where(where).AsEnumerable();
        return View(result);
    }
    
    public IActionResult RepairUnitAgendumOrderIndex()
    {
        var result = Context.RepairUnitAgendumOrders.AsEnumerable();
        return View(result);
    }


    private IEnumerable<object>
        Parts() => new SelectList(Context.VwCategories.Where(m => m.CategoryId == 15).ToList(),
        "SubCategoryId", "SubCategoryCaption");
    private IEnumerable<object> Machine() => new SelectList(Context.Machines.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).ToList(), "Id", "Name");

    public IActionResult RequestCreate()
    {
        ViewBag.Machine = this.Machine();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RequestCreate(RepairRequest repair)
    {
        if (ModelState.IsValid)
        {
            repair.UserId = User.GetUserIdPrincipal();
            Context.RepairRequests.Add(repair);
            Context.SaveChanges();
            return RedirectToAction(nameof(RequestIndex));
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
    //

    public IActionResult CreateRepairUnitAgendumOrders(long id)
    {
        var model = new RepairUnitAgendumOrder()
        {
            RepairRequestId = id,
            ActionKinds = Context.SubCategories.Where(w => w.CategoryId == 14),
            Locations = Context.SubCategories.Where(w => w.CategoryId == 19),
            Persons = Context.Persons.Where(w => w.GeneralStatusId == GeneralStatus.Active),
        };
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateRepairUnitAgendumOrders(RepairUnitAgendumOrder report)
    {
        if (ModelState.IsValid)
        {
            report.Id = 0;
            report.InsDate = DateTime.Now;
            Context.RepairUnitAgendumOrders.Add(report);
            Context.SaveChanges();
            return RedirectToAction(nameof(RequestIndex));
        }

        report.ActionKinds = Context.SubCategories.Where(w => w.CategoryId == 14);
        report.Locations = Context.SubCategories.Where(w => w.CategoryId == 19);
        report.Persons = Context.Persons.Where(w => w.GeneralStatusId == GeneralStatus.Active);
        return View(report);
    }

    public IActionResult CreateRepairUnitAgendumOrderActionType(long id)
    {
        var model = new RepairUnitAgendumOrderActionType()
        {
            RepairUnitAgendumOrderId = id,
            ActionTypes = Context.SubCategories.Where(w => w.CategoryId == 18),
        };
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateRepairUnitAgendumOrderActionType(RepairUnitAgendumOrderActionType report)
    {
        if (ModelState.IsValid)
        {
            report.Id = 0;
            report.InsDate = DateTime.Now;
            Context.RepairUnitAgendumOrderActionTypes.Add(report);
            Context.SaveChanges();
            return RedirectToAction(nameof(RequestIndex));
        }

        report.ActionTypes = Context.SubCategories.Where(w => w.CategoryId == 18);
        return View(report);
    }




    // POST: InspectionCategory/Delete/5
    [HttpPost, ActionName("SetStatusRepairRequest")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetStatusRepairRequest(long id)
    {
        var request = await Context.RepairRequests.FindAsync(id);
        if (request != null)
        {
            request.Status = 1;
            Context.SaveChanges();
        }
        return Json(new { hasError = false, message = "" });
    }
}
