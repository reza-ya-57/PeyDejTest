using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PeyDej.Data;
using PeyDej.Models;
using PeyDej.Service.File;

namespace PeyDej.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IFileInterface _fileInterface;
    private readonly PeyDejContext _context;

    public HomeController(ILogger<HomeController> logger, IFileInterface fileInterface, PeyDejContext context)
    {
        _logger = logger;
        _fileInterface = fileInterface;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [Route("/H/D/{Id:guid}")]
    public FileResult? Download(Guid Id)
    {
        var file = _context.FileInformations.FirstOrDefault(w => w.Id == Id);
        if (file == null)
        {
            return null;
        }
        var path = Path.Combine(file.FileLocation, file.Id.ToString());
        if (Directory.Exists(path))
        {
            return null;
        }
        var byteFile = System.IO.File.ReadAllBytes(path);
        return File(byteFile, file.ContentType, file.FileName + file.FileExtension);
    }

    public IActionResult TaskList()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}