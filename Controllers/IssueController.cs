using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NamaProyekAnda.Data;
using NamaProyekAnda.Models;
using OfficeOpenXml;



namespace NamaProyekAnda.Controllers
{
    public class IssuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IssuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Issues
        public async Task<IActionResult> Index()
        {
            // Filter issues where Status is not 'done'
            var issues = await _context.Issues
                                       .Where(i => i.Status != "done")
                                       .ToListAsync();

            return View(issues);
        }




        // GET: Issues
        public async Task<IActionResult> Done()
        {
            // Filter issues where Status is not 'done'
            var issues = await _context.Issues
                                       .Where(i => i.Status == "done")
                                       .ToListAsync();

            return View(issues);
        }


        // GET: Issues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issues
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // GET: Issues/Create
        public IActionResult Create()
        {
            return View();
        }

        // // POST: Issues/Create
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("Id,Kode,Tgl,Jenis,DetailIssue,NamaBrand,NamaVendor,JenisVendor,Status,Pic")] Issue issue)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(issue);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(issue);
        // }


        // POST: Issues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Kode,Tgl,Jenis,DetailIssue,NamaBrand,NamaVendor,JenisVendor,Status,Pic,ImageData")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(issue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(issue);
        }




        // GET: Issues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issues.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }
            return View(issue);
        }

        // POST: Issues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Kode,Tgl,Jenis,DetailIssue,NamaBrand,NamaVendor,JenisVendor,Status,Pic,Tindakan, ImageData")] Issue issue)
        {
            if (id != issue.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(issue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueExists(issue.Id))
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
            return View(issue);
        }

        // GET: Issues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issues
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null)
            {
                return NotFound();  // Handle the case where the issue doesn't exist
            }

            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssueExists(int id)
        {
            return _context.Issues.Any(e => e.Id == id);
        }



        public IActionResult Import()
        {
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a valid Excel file.");
                return View("Index");
            }

            var list = new List<Issue>();

            using (var stream = new MemoryStream())
            {
                await excelFile.CopyToAsync(stream);
                stream.Position = 0; // Pastikan stream berada di posisi awal

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        ModelState.AddModelError(string.Empty, "No worksheet found in the Excel file.");
                        return View("Index");
                    }

                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        try
                        {
                            var issue = new Issue
                            {

                                Tgl = worksheet.Cells[row, 1].Text,

                                Jenis = worksheet.Cells[row, 2].Text,
                                DetailIssue = worksheet.Cells[row, 3].Text,
                                NamaBrand = worksheet.Cells[row, 4].Text,
                                NamaVendor = worksheet.Cells[row, 5].Text,
                                JenisVendor = worksheet.Cells[row, 6].Text,
                                Status = worksheet.Cells[row, 7].Text,
                                Pic = worksheet.Cells[row, 8].Text
                            };

                            list.Add(issue);
                        }
                        catch (FormatException ex)
                        {
                            ModelState.AddModelError(string.Empty, $"Error parsing data at row {row}: {ex.Message}");
                            continue;
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError(string.Empty, $"Unexpected error at row {row}: {ex.Message}");
                            continue;
                        }
                    }
                }
            }

            if (list.Any())
            {
                // Ambil semua record dari database untuk pengecekan duplikat
                var existingRecords = await _context.Issues.ToListAsync();

                // Filter daftar yang akan ditambahkan
                var newRecords = list.Where(issue => !existingRecords.Any(existing =>

                    existing.Tgl == issue.Tgl &&
                    existing.Jenis == issue.Jenis &&
                    existing.DetailIssue == issue.DetailIssue &&
                    existing.NamaBrand == issue.NamaBrand &&
                    existing.NamaVendor == issue.NamaVendor &&
                    existing.JenisVendor == issue.JenisVendor &&
                    existing.Status == issue.Status &&
                    existing.Pic == issue.Pic
                )).ToList();

                // Tambahkan hanya record baru yang belum ada di database
                if (newRecords.Any())
                {
                    _context.Issues.AddRange(newRecords);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Index", "Issues");
        }


public async Task<IActionResult> Chart()
{
    // Data for the doughnut chart
    var chartData = await _context.Issues
        .GroupBy(i => i.Jenis ?? "Undefined")
        .Select(g => new { Jenis = g.Key, Count = g.Count() })
        .ToListAsync();

    // Data for the status cards
    var statusCounts = await _context.Issues
        .GroupBy(i => i.Status)
        .Select(g => new { Status = g.Key, Count = g.Count() })
        .ToListAsync();

    // Prepare data for the cards
    var newIssueCount = statusCounts.FirstOrDefault(s => s.Status == "new issue")?.Count ?? 0;
    var progressCount = statusCounts.FirstOrDefault(s => s.Status == "progress")?.Count ?? 0;
    var doneCount = statusCounts.FirstOrDefault(s => s.Status == "done")?.Count ?? 0;

    ViewBag.ChartData = chartData;
    ViewBag.NewIssueCount = newIssueCount;
    ViewBag.ProgressCount = progressCount;
    ViewBag.DoneCount = doneCount;

    return View();
}





    }
}
