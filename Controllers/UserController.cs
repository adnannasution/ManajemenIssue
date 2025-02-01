using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NamaProyekAnda.Data;
using NamaProyekAnda.Models;
using Microsoft.Extensions.Logging;

namespace NamaProyekAnda.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(ApplicationDbContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
         
                var user = await _context.User.ToListAsync();
                return View(user);
            
       
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        


    }
}
