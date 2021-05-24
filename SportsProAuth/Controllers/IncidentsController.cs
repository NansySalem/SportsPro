using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Index()
        {
            var sportsProContext = _context.Incidents.Include(i => i.Customer).Include(i => i.Product).Include(i => i.Technician);
            return View(await sportsProContext.ToListAsync());
        }

        // GET: Incidents/Details/5
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
        //edit and create in one view
        public async Task<IActionResult> Upsert(int? id)
        {
            if (id == null)//if id is null, then the request is to create a new incident
            {
                ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", null);
                ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", null);
                ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Name", null);
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
            if (incident.TechnicianID != null)
                ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Name", incident.TechnicianID);
            else
                ViewData["TechnicianID"] = new SelectList(_context.Technicians, "TechnicianID", "Name", null);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
