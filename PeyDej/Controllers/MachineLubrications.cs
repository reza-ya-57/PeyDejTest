using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;

using PeyDej.Tools;

using System.Data;

namespace PeyDej.Controllers
{
    [Authorize(Roles = "Admin,Lubricator")]
    public class MachineLubrications : Controller
    {
        public PeyDejContext _context { get; }

        public MachineLubrications(PeyDejContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> MachineLubricationsIS(string start_date, string end_date)
        {
            start_date ??= PeyDejTools.GetCurPersianDate();
            end_date ??= PeyDejTools.GetCurPersianDate();
            var data = await _context.MachineLubrications
                .Where(m =>
                    m.Status == 0 &&
                    m.InspectionDate >= PeyDejTools.PersianStringToDateTime(start_date) &&
                    m.InspectionDate <= PeyDejTools.PersianStringToDateTime(end_date)
                ).ToListAsync();

            var machineIDs = data.Select(item => item.MachineId).ToList();
            var result = await _context.Machines.Where(m => machineIDs.Contains(m.Id)).ToListAsync();
            HttpContext.Session.SetString("start_date", start_date);
            HttpContext.Session.SetString("end_date", end_date);

            ViewBag.startDate = start_date;
            ViewBag.endDate = end_date;
            return View(result);
        }

        public async Task<IActionResult> MachineLubricationsPrintPage()
        {
            var start_date = HttpContext.Session.GetString("start_date");
            var end_date = HttpContext.Session.GetString("end_date");
            var data = await _context.MachineLubrications
                .Where(m =>
                    m.Status == 0 &&
                    m.InspectionDate >= PeyDejTools.PersianStringToDateTime(start_date) &&
                    m.InspectionDate <= PeyDejTools.PersianStringToDateTime(end_date)
                ).ToListAsync();

            var machineIDs = data.Select(item => item.MachineId).ToList();
            var result = await _context.Machines.Where(m => machineIDs.Contains(m.Id)).ToListAsync();

            ViewBag.items = await _context.VwCategories.Where(m => m.CategoryId == 1).ToListAsync();
            ViewBag.startDate = start_date;
            ViewBag.endDate = end_date;
            return View(result);
        }
    }
}
