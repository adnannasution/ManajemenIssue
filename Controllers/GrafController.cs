using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NamaProyekAnda.Data;

public class GrafController : Controller
{
    private readonly ApplicationDbContext _context;

    public GrafController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult ChartData()
    {
        var data = _context.Issues
            .GroupBy(i => i.Jenis)
            .Select(g => new 
            {
                Jenis = g.Key,
                Count = g.Count()
            })
            .ToList();

        return Json(data);
    }

    public IActionResult Index()
    {
        return View(); // This will load Views/Graf/Chart.cshtml by default
    }
}
