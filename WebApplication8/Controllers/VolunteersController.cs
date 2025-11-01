using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Models;

namespace WebApplication8.Controllers
{
    public class VolunteersController : Controller
    {
        private readonly GiftOfGiversContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public VolunteersController(GiftOfGiversContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Volunteers
        public async Task<IActionResult> Index()
        {
            var giftOfGiversContext = _context.Volunteers.Include(v => v.User);
            return View(await giftOfGiversContext.ToListAsync());
        }

        // GET: Volunteers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.Volunteer_ID == id);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // GET: Volunteers/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Email");
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Volunteer_ID,FirstName,LastName,Location,AvailabilityStatus,UserID")] Volunteer volunteer)
        {
            var userLoggedin = await _userManager.GetUserAsync(User);
            var user = _context.Users.FirstOrDefault(u => u.Email == userLoggedin.Email);
            volunteer.User = user;
            volunteer.UserID = user.UserID;

            _context.Add(volunteer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        // GET: Volunteers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Email", volunteer.UserID);
            return View(volunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Volunteer_ID,FirstName,LastName,Location,AvailabilityStatus,UserID")] Volunteer volunteer)
        {
            if (id != volunteer.Volunteer_ID)
            {
                return NotFound();
            }

            var userLoggedin = await _userManager.GetUserAsync(User);
            var user = _context.Users.FirstOrDefault(u => u.Email == userLoggedin.Email);
            volunteer.User = user;
            volunteer.UserID = user.UserID;
            try
                {
                    _context.Update(volunteer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VolunteerExists(volunteer.Volunteer_ID))
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

        // GET: Volunteers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var volunteer = await _context.Volunteers
                .Include(v => v.User)
                .FirstOrDefaultAsync(m => m.Volunteer_ID == id);
            if (volunteer == null)
            {
                return NotFound();
            }

            return View(volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer != null)
            {
                _context.Volunteers.Remove(volunteer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VolunteerExists(int id)
        {
            return _context.Volunteers.Any(e => e.Volunteer_ID == id);
        }
    }
}
