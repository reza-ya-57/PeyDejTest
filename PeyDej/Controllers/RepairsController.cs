using Ccms.Common.Utilities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Report;
using PeyDej.Service;

namespace PeyDej.Controllers;


public class RepairsController : Controller
{
    public PeyDejContext Context { get; }

    public RepairsController(PeyDejContext context)
    {
        Context = context;
    }
    [Authorize]
    public IActionResult RequestIndex() => View(Context.RepairRequests.Where(w => w.UserId == User.GetUserIdPrincipal() && w.Status == 0).AsEnumerable());
    [Authorize(Roles = "Admin,Production")]
    public IActionResult UnitAgendumOrderIndex() => View(Context.RepairRequests.Where(w => !Context.RepairUnitAgendumOrders.Any(order => order.RepairRequestId == w.Id)).AsEnumerable());


    [Authorize(Roles = "Admin,Electrical,Mechanical,Facility")]
    public IActionResult RepairUnitAgendumOrderIndex() => View(Context.RepairUnitAgendumOrders.AsEnumerable());


    [Authorize(Roles = "Admin,Electrical,Mechanical,Facility,Production")]
    public IActionResult RequestCreate()
    {
        ViewBag.Machine = Machine();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Electrical,Mechanical,Facility,Production")]
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

    [Authorize(Roles = "Admin,Electrical,Mechanical,Facility,Production")]
    public IActionResult Print()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    public IActionResult RepairReportIndex() => View(Context.RepairReports);
    [Authorize(Roles = "Admin,Electrical,Mechanical,Facility")]
    public IActionResult CreateReport(long id)
    {
        var repairUnitAgendumOrder = Context.RepairUnitAgendumOrders.Where(w => w.Id == id).FirstOrDefault();
        repairUnitAgendumOrder.ActionTypeList =
            Context.SubCategories.Where(w => Context.RepairUnitAgendumOrderActionTypes.Where(
                w => w.RepairUnitAgendumOrderId == repairUnitAgendumOrder.Id)
                .Select(s => s.ActionTypeId)
                .ToList()
                .Contains(w.Id));

        var repairRequest = Context.RepairRequests.Where(w => w.Id == repairUnitAgendumOrder.RepairRequestId).FirstOrDefault();
        var model = new RepairReport()
        {
            AgendumOrder = repairUnitAgendumOrder,
            RepairUnitAgendumOrderId = id,
            RepairRequest = repairRequest,
        };
        ViewBag.Parts = this.Parts();
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin,Electrical,Mechanical,Facility")]
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


    [Authorize(Roles = "Admin,Production")]
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
    [Authorize(Roles = "Admin,Production")]
    public IActionResult CreateRepairUnitAgendumOrders(RepairUnitAgendumOrder report, RepairReportSparePart spare)
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
    [Authorize(Roles = "Admin,Production")]
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




    private IEnumerable<object> Parts() => new SelectList(Context.VwCategories.Where(m => m.CategoryId == 15).ToList(), "SubCategoryId", "SubCategoryCaption");

    private IEnumerable<object> Machine() => new SelectList(Context.Machines.Where(w => w.GeneralStatusId != GeneralStatus.Deleted).ToList(), "Id", "Name");
}
