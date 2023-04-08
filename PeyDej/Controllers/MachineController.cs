using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Models.Bases;

namespace PeyDej.Controllers
{
  [Authorize]
  public class MachineController : Controller
  {
    private readonly PeyDejContext _context;

    public MachineController(PeyDejContext context)
    {
      _context = context;
    }

    // GET: Machine
    public IActionResult Index()
    {
      if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        return PartialView("_Index", _context.Machines.Where(m => m.GeneralStatusId == GeneralStatus.Active).ToList());
      return View();
    }

    // GET: Machine/Details/5
    public async Task<IActionResult> Details(long? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var machine = await _context.Machines
          .FirstOrDefaultAsync(m => m.Id == id);
      if (machine == null)
      {
        return NotFound();
      }

      return View(machine);
    }

    // GET: Machine/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Machine/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
                "Id,InsDate,Name,Model,Department,SerialNumber,Country,Company,Process,PurchaseStatus,UtilizationDate,CompanyAddress,AgencyAddress,EnergyType,EnergyConsumption,OilType,OilConsumption,OilReplacementPeriod,GreaseType,OilLocation,GreaseConsumption,GreaseReplacementPeriod,GreaseLocation,GreaseCount,InspectionCycle,LubricationCycle,GeneralStatusId")]
            Machine machine)
    {
      if (ModelState.IsValid)
      {
        _context.Add(machine);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }

      return View(machine);
    }

    // GET: Machine/Edit/5
    public async Task<IActionResult> Edit(long? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var machine = await _context.Machines.FindAsync(id);
      if (machine == null)
      {
        return NotFound();
      }

      return View(machine);
    }

    // POST: Machine/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id,
        [Bind(
                "Id,InsDate,Name,Model,Department,SerialNumber,Country,Company,Process,PurchaseStatus,UtilizationDate,CompanyAddress,AgencyAddress,EnergyType,EnergyConsumption,OilType,OilConsumption,OilReplacementPeriod,GreaseType,OilLocation,GreaseConsumption,GreaseReplacementPeriod,GreaseLocation,GreaseCount,InspectionCycle,LubricationCycle,GeneralStatusId")]
            Machine machine)
    {
      if (id != machine.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(machine);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!MachineExists(machine.Id))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }

        return RedirectToAction(nameof(Index));
      }

      return View(machine);
    }

    // POST: Machine/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
      var machine = await _context.Machines.FindAsync(id);
      if (machine != null)
      {
        machine.GeneralStatusId = GeneralStatus.Deleted;
        _context.Machines.Update(machine);
      }

      await _context.SaveChangesAsync();
      return Json(new { hasError = false, message = "" });
    }

    private bool MachineExists(long id)
    {
      return (_context.Machines?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}