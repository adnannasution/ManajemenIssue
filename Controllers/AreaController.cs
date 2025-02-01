using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NamaProyekAnda.Models;

namespace NamaProyekAnda.Controllers;

public class AreaController : Controller
{
    private readonly ILogger<AreaController> _logger;

    public AreaController(ILogger<AreaController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

        public IActionResult Tambah()
    {
        return View();
    }

 

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
