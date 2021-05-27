using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportsPro.Models;
using SportsProAuth.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SportsProAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SportsProContext _context;

        public HomeController(ILogger<HomeController> logger, SportsProContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Technician"))
            {
                var incidents = _context.Incidents
                    .Include(i => i.Customer)
                    .Include(i => i.Product)
                    .Include(i => i.Technician)
                    .Where(t => t.Technician.Email == User.Identity.Name).ToList();
                var tech = _context.Technicians.SingleOrDefault(t => t.Email == User.Identity.Name);
                ViewData["name"] = tech.Name.ToString();
                return View(incidents);
            }
            else
                return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetTechIncidents(string name)
        {
            if (name == "all-incidents")
            {
                var sportsProContext = _context.Incidents.Include(i => i.Customer)
                                       .Include(i => i.Product)
                                       .Include(i => i.Technician)
                                       .Where(t => t.Technician.Email == User.Identity.Name);
                return PartialView("_selected-incidents", await sportsProContext.ToListAsync());
            }

            else if (name == "open-incidents")
            {
                var sportsProContext = _context.Incidents.Include(i => i.Customer)
                                    .Include(i => i.Product)
                                    .Include(i => i.Technician)
                                    .Where(t => t.DateClosed == null &&
                                    t.Technician.Email == User.Identity.Name);
                return PartialView("_selected-incidents", await sportsProContext.ToListAsync());
            }
            else
                return View();

        }
    }
}
