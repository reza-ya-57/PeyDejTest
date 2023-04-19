using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using PeyDej.Data;
using PeyDej.Models;

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
        if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_Index", Context.RepairRequests.ToList());
        return View();
    }
}
