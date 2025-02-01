using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NamaProyekAnda.Data;
using NamaProyekAnda.Models;

namespace YourNamespace.Controllers
{
  public class TindakanController : Controller
  {
    private readonly ApplicationDbContext _context;

    public TindakanController(ApplicationDbContext context)
    {
      _context = context;
    }

    // GET: Tindakan
    public IActionResult Index(int? id)
    {
      var tindakanList = _context.Tindakan
                                 .Where(t => t.Idissue == id)
                                 .ToList();

      return View(tindakanList);
    }

    // GET: Tindakan/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var tindakan = await _context.Tindakan
          .FirstOrDefaultAsync(m => m.Id == id);
      if (tindakan == null)
      {
        return NotFound();
      }

      return View(tindakan);
    }



    public IActionResult Create(int idissue)
{
    // Pass the Idissue to the view or use it in your logic
    ViewBag.Idissue = idissue;
    return View();
}



   // POST: Tindakan/Create
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Idissue,Tgltindakan,Tindakanku")] Tindakan tindakan)
{
    if (ModelState.IsValid)
    {
        try
        {
            _context.Add(tindakan);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            // Log the exception (consider using a logging framework)
            // Example: _logger.LogError(ex, "An error occurred while saving the Tindakan.");

            // Add a user-friendly error message to the ModelState
            ModelState.AddModelError("", "Unable to save changes. Please try again later.");
        }
    }

    // If we got this far, something failed, redisplay the form with validation errors
    return View(tindakan);
}


    // GET: Tindakan/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var tindakan = await _context.Tindakan.FindAsync(id);
      if (tindakan == null)
      {
        return NotFound();
      }
      return View(tindakan);
    }

    // POST: Tindakan/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Idissue,Tgltindakan,Tindakanku")] Tindakan tindakan)
    {
      if (id != tindakan.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(tindakan);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!TindakanExists(tindakan.Id))
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
      return View(tindakan);
    }

    // GET: Tindakan/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var tindakan = await _context.Tindakan
          .FirstOrDefaultAsync(m => m.Id == id);
      if (tindakan == null)
      {
        return NotFound();
      }

      return View(tindakan);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var tindakan = await _context.Tindakan.FindAsync(id);

      if (tindakan == null)
      {
        // Optionally return an error view or redirect to a different page if the entity is not found
        return NotFound();
      }

      _context.Tindakan.Remove(tindakan);
      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }


    private bool TindakanExists(int id)
    {
      return _context.Tindakan.Any(e => e.Id == id);
    }
  }
}
