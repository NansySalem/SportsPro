using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using Microsoft.AspNetCore.Authorization;

namespace SportsProAuth.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class RegistrationsController : Controller
    {
        private readonly SportsProContext _context;

        public RegistrationsController(SportsProContext context)
        {
            _context = context;
        }

        // GET: Registrations/Create
        //[Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name");
            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegistrationID,CustomerID,ProductID")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", registration.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", registration.ProductID);
            return View(registration);
        }

       

        // GET: Registrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .Include(r => r.Customer)
                .Include(r => r.Product)
                .FirstOrDefaultAsync(m => m.RegistrationID == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Registrations/Details/5
        //  [Authorize(Roles = "Technician")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .Include(r => r.Customer)
                .Include(r => r.Product)
                .FirstOrDefaultAsync(m => m.RegistrationID == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // GET: Registrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", registration.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", registration.ProductID);
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RegistrationID,CustomerID,ProductID")] Registration registration)
        {
            if (id != registration.RegistrationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.RegistrationID))
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
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", registration.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", registration.ProductID);
            return View(registration);
        }

        //[Authorize(Roles = "Technician")]
        public async Task<IActionResult> Index()
        {
            var sportsProContext = _context.Registrations.Include(r => r.Customer).Include(r => r.Product);
            return View(await sportsProContext.ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult ListByCustomer()
        {
            var custs = _context.Customers.ToList();
           
            return View(custs);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult customerProducts(string id)

        {
          

            var parsedId = int.Parse(id);
            var cust = _context.Customers.SingleOrDefault(cId => cId.CustomerID == parsedId);
            var registrations = _context.Registrations.Where(i => i.CustomerID == cust.CustomerID)
                .Include(i => i.Customer)
                .Include(i => i.Product)
                .ToList();

       

             return PartialView("_customerProducts", registrations); 
            
           
            

          
        }
        //edit and create in one view
        /// <summary>
        /// As discussed, created a sample for the logic of having create and edit using one view "upsert.cshtml"
        ///Although the logic works, had to use different views for the edit and create 
        ///to allow different authorization priviledges i.e. create for admin and edit/update for technician
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)//if id is null, then the request is to create a new registration
            {
                ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName");
                ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name");

                return View();
            }
            //else, we find the registration to load it
            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", registration.CustomerID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", registration.ProductID);

            return View(registration);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, [Bind("RegistrationID,CustomerID,ProductID")] Registration registration)
        {
            if (id != null)//if id is not null, then this is an update
            {
                //if id exitst but not equal to registrationID return not found
                if (id != registration.RegistrationID)
                {
                    return NotFound();
                }
                else //update registration if all checks
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            _context.Update(registration);
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!RegistrationExists(registration.RegistrationID))
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
                    ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", registration.CustomerID);
                    ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", registration.ProductID);

                    return View(registration);
                }
            }
            else//else we create a new registration
            {
                //if there is not registration, create a new incident
                if (ModelState.IsValid)
                {
                    _context.Add(registration);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "FullName", registration.CustomerID);
                ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "Name", registration.ProductID);

                return View(registration);

            }
        }

        private bool RegistrationExists(int id)
        {
            return _context.Registrations.Any(e => e.RegistrationID == id);
        }
    }
}
