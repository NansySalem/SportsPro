using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SportsProAuth.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CustomersController : Controller
    {
        private readonly SportsProContext _context;

        public CustomersController(SportsProContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var sportsProContext = _context.Customers.Include(c => c.Country);
            return View(await sportsProContext.ToListAsync());
        }

        //public JsonResult CheckEmail(String Email)
        //{​​​​​​​
        //    int count = _context.Customers.Where(o => o.Email.Equals(Email)).Count();
        //    bool hasEmail = count > 0;
        //    if (hasEmail)
        //        return Json($"Email address {​​​​​​​Email}​​​​​​​ is already registered");
        //    else
        //    {​​​​​​​
        //        return Json(true);
        //    }​​​​​​​
        //}​​​​​​​

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["CountryID"] = new SelectList(_context.Countries, "CountryID", "CountryID");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,FirstName,LastName,Address,City,State,PostalCode,CountryID,Phone,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                TempData["message"] = $"{customer.FirstName} {customer.LastName} added into database.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryID"] = new SelectList(_context.Countries, "CountryID", "CountryID", customer.CountryID);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["CountryID"] = new SelectList(_context.Countries, "CountryID", "CountryID", customer.CountryID);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,FirstName,LastName,Address,City,State,PostalCode,CountryID,Phone,Email")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerID))
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
            ViewData["CountryID"] = new SelectList(_context.Countries, "CountryID", "CountryID", customer.CountryID);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            TempData["message"] = $"The customer {customer.FirstName} {customer.LastName} was deleted from database.";
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
    }
}