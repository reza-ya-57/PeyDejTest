using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Report;
using PeyDej.Service;

using System.Linq.Expressions;
using Ccms.Common.Utilities;
using System.Composition;

namespace PeyDej.Controllers;


[Authorize]
public class RepairsController : Controller
{
    public PeyDejContext Context { get; }

    public RepairsController(PeyDejContext context)
    {
        Context = context;
    }

    public IActionResult RequestIndex() => View(Context.RepairRequests.Where(w => w.UserId == User.GetUserIdPrincipal()&& w.Status == 0).AsEnumerable());

    public IActionResult UnitAgendumOrderIndex() => View(Context.RepairRequests.Where(w => !Context.RepairUnitAgendumOrders.Any(order => order.RepairRequestId == w.Id)).AsEnumerable());

    public IActionResult RepairUnitAgendumOrderIndex() => View(Context.RepairUnitAgendumOrders.AsEnumerable());

    private IEnumerable<object> Parts() => new SelectList(Context.VwCategories.Where(m => m.CategoryId == 15).ToList(), "SubCategoryId", "SubCategoryCaption");
    
    private IEnumerable<object> Machine() => new SelectList(Context.Machines.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).ToList(), "Id", "Name");

    public IActionResult RequestCreate()
    {
        ViewBag.Machine = Machine();
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
        ViewBag.Machine = Machine();

        return View(repair);
    }

    public IActionResult Print()
    {
        return View();
    }

    public IActionResult RepairReportIndex() => View(Context.RepairReports);
    public IActionResult CreateReport(long id)
    {
        var model = new RepairReport()
        {
            RepairUnitAgendumOrderId = id,
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
            report.StartRepairDate = (DateTime)report.StartRepairDateDto.ToMiladi();
            report.EndRepairDate = (DateTime)report.EndRepairDateDto.ToMiladi();
            Context.RepairReports.Add(report);
            Context.SaveChanges();
            return RedirectToAction(nameof(RepairReportIndex));
        }
        ViewBag.Parts = this.Parts();
        return View(report);
    }


    public IActionResult CreateRepairUnitAgendumOrders(long id)
    {
        var model = new RepairUnitAgendumOrder()
        {
            RepairRequestId = id,
            ActionTypeList = Context.SubCategories.Where(w => w.CategoryId == 18),
            ActionKinds = Context.SubCategories.Where(w => w.CategoryId == 14),
            Locations = Context.SubCategories.Where(w => w.CategoryId == 19),
            Persons = Context.Persons.Where(w => w.GeneralStatusId == GeneralStatus.Active),
        };
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateRepairUnitAgendumOrders(RepairUnitAgendumOrder report,RepairReportSparePart spare)
    {
        if (ModelState.IsValid)
        {
            report.Id = 0;
            report.InsDate = DateTime.Now;
            report.PersonIds = string.Join(",", report.PersonList);

            Context.RepairUnitAgendumOrders.Add(report);
            Context.RepairUnitAgendumOrderActionTypes.AddRange(
                report.ActionTypes.Select(s => new RepairUnitAgendumOrderActionType()
            {
                ActionTypeId = s,
                InsDate = DateTime.Now,
                RepairUnitAgendumOrderId = report.Id
            }));

            Context.SaveChanges();
            return RedirectToAction(nameof(RequestIndex));
        }

        report.ActionTypeList = Context.SubCategories.Where(w => w.CategoryId == 18);
        report.ActionKinds = Context.SubCategories.Where(w => w.CategoryId == 14);
        report.Locations = Context.SubCategories.Where(w => w.CategoryId == 19);
        report.Persons = Context.Persons.Where(w => w.GeneralStatusId == GeneralStatus.Active);
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
