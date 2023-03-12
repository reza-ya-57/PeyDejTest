using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
using PeyDej.Models;

namespace PeyDej.Controllers;

public class InspectionController : Controller
{
    private readonly PeyDejContext _context;

    public InspectionController(PeyDejContext _context)
    {
        this._context = _context;
    }

    public async Task<IActionResult> Motor(da)
    {
        var data = await _context.MachineISs
            .Where(m => m.Status == InspectionStatus.NotOk && m.InspectionDate >= DateTime.Today).ToListAsync();
        ViewBag["listOfMachines"] = data;
        return View();
    }
}