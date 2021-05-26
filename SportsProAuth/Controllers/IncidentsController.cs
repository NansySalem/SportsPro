using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;

namespace SportsProAuth.Controllers
{
 
    public class IncidentsController : Controller
    {

        private readonly SportsProContext _context;


        public IncidentsController(SportsProContext context)
        {
            _context = context;
        }

        // GET: Incidents
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var sportsProContext = _context.Incidents.Include(i => i.Customer)
                                       .Include(i => i.Product)
                                       .Include(i => i.Technician);
            return View(await sportsProContext.ToListAsync());
        }
        public async Task<IActionResult> GetIncidents(string name)
        {
            if (name == "all-incidents")
            {
                var sportsProContext = _context.Incidents.Include(i => i.Customer)
                                       .Include(i => i.Product)
                                       .Include(i => i.Technician);
                return PartialView("_selected-incidents", await sportsProContext.ToListAsync());
            }
            else if (name == "unassigned-incidents")
            {
                var sportsProContext = _context.Incidents.Include(i => i.Customer)
                                    .Include(i => i.Product)
                                    .Include(i => i.Technician).Where(t => t.TechnicianID == null);
                return PartialView("_selected-incidents", await sportsProContext.ToListAsync());
            }
            else if (name == "open-incidents")
            {
                var sportsProContext = _context.Incidents.Include(i => i.Customer)
                                    .Include(i => i.Product)
                                    .Include(i => i.Technician)
                                    .Where(d => d.DateClosed == null);
                return PartialView("_selected-incidents", await sportsProContext.ToListAsync());
            }
            else
                return View();

           

            //ViewData["assigned_incidents"] = _context.Incidents.Select(i => i.TechnicianID == tech.TechnicianID);
            //ViewData["unassigned_incidents"] = _context.Incidents.Select(i => i.TechnicianID == null);
            //ViewData["otherAssigned_incidents"] = _context.Incidents.Select(i => i.TechnicianID != null && i.TechnicianID != tech.TechnicianID);

        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefaultAsync(m => m.IncidentID == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.IncidentID == id);
        }


        // GET: Incidents/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name");
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Name");
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("IncidentID,CustomerID,ProductID,TechnicianID,Title,Description,DateOpened,DateClosed")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incident);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", incident.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", incident.ProductID);
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Name", incident.TechnicianID);
            return View(incident);
        }

        // GET: Incidents/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", incident.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", incident.ProductID);
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Name", incident.TechnicianID);
             return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("IncidentID,CustomerID,ProductID,TechnicianID,Title,Description,DateOpened,DateClosed")] Incident incident)
        {
            if (id != incident.IncidentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentExists(incident.IncidentID))
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
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", incident.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", incident.ProductID);
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Name", incident.TechnicianID);
            return View(incident);
        }
        //edit and create in one view
        /// <summary>
        /// As discussed, created a sample for the logic of having create and edit using one view "upsert.cshtml"
        ///Although the logic works, had to use different views for the edit and create 
        ///to allow different authorization priviledges i.e. create for admin and edit/update for technician
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Upsert(int? id)
        {
            if (id == null)//if id is null, then the request is to create a new incident
            {
                ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName");
                ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name");
                ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Name");
                return View();
            }
            //else, we find the incident to load it
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", incident.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", incident.ProductID);
            ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Name", incident.TechnicianID);
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(int? id, [Bind("IncidentID,CustomerID,ProductID,TechnicianID,Title,Description,DateOpened,DateClosed")] Incident incident)
        {
            if (id != null)//if id is not null, then this is an update
            {
                //if id exitst but not equal to IncidentID return not found
                if (id != incident.IncidentID)
                {
                    return NotFound();
                }
                else //update incident if all checks
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(incident);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!IncidentExists(incident.IncidentID))
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
                    ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", incident.CustomerID);
                    ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", incident.ProductID);
                    ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Name", incident.TechnicianID);
                    return View(incident);
                }
            }
            else//else we create a new incident
            {
                //if there is not incident, create a new incident
                if (ModelState.IsValid)
                {
                    _context.Add(incident);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", incident.CustomerID);
                ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", incident.ProductID);
                ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Name", incident.TechnicianID);
                return View(incident);

            }
        }

        // GET: Incidents/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .Include(i => i.Technician)
                .FirstOrDefaultAsync(m => m.IncidentID == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Technician")]
        public IActionResult ListByTech()
        {
            var techs = _context.Technicians.ToList();
           // ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Name");
            return View(techs);
        }

        [Authorize(Roles = "Technician")]
        public IActionResult techIncidents(string id)

        {
            // ToDo: validate id => true -> proceed -> display error

            var parsedId = int.Parse(id);
            var tech = _context.Technicians.SingleOrDefault(tId => tId.TechnicianID == parsedId);
            var incidents = _context.Incidents.Where(i => i.TechnicianID == tech.TechnicianID)
                .Include(i=> i.Customer)
                .Include(i=> i.Product)
                .ToList();

            
            return PartialView("_techIncidents", incidents);
        }

        // GET: Incidents/Edit/5
        [Authorize(Roles = "Technician, Administrator")]
        public async Task<IActionResult> techEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }
            ViewData["CustomerName"] = _context.Customers.SingleOrDefault(c=> incident.CustomerID == c.CustomerID).FullName;
            ViewData["ProductName"] = _context.Products.SingleOrDefault(p => incident.ProductID == p.ProductID).Name;
            ViewData["TechnicianName"] = _context.Technicians.SingleOrDefault(t => incident.TechnicianID == t.TechnicianID).Name;
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Technician, Administrator")]
        public async Task<IActionResult> techEdit(int id, [Bind("IncidentID,CustomerID,ProductID,TechnicianID,Title,Description,DateOpened,DateClosed")] Incident incident)
        {
            if (id != incident.IncidentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentExists(incident.IncidentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("~/Incidents/ListByTech");
            }
            

            return View(incident);
        }



    }
}
