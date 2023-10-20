using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TeamUserManagementSystem.Data;
using TeamUserManagementSystem.Models;

namespace TeamUserManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;

        public HomeController(Context context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            var teams = _context.Teams.ToList();

            var viewModel = new HomeViewModel
            {
                Users = users,
                Teams = teams
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}