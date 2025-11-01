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
    public class ReliefProjectsController : Controller
    {
        private readonly GiftOfGiversContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ReliefProjectsController(GiftOfGiversContext context,UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ReliefProjects
        public async Task<IActionResult> Index()
        {
            var giftOfGiversContext = _context.ReliefProjects.Include(r => r.User);
            return View(await giftOfGiversContext.ToListAsync());
        }

        // GET: ReliefProjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reliefProject = await _context.ReliefProjects
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReliefProjectID == id);
            if (reliefProject == null)
            {
                return NotFound();
            }

            return View(reliefProject);
        }

        // GET: ReliefProjects/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Email");
            return View();
        }

        // POST: ReliefProjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReliefProjectID,ProjectName,Description,Location,StartDate,EndDate,Status,UserID")] ReliefProject reliefProject)
        {
            var userLoggedin = await _userManager.GetUserAsync(User);
            var user = _context.Users.FirstOrDefault(u => u.Email == userLoggedin.Email);
            reliefProject.User = user;
            reliefProject.UserID = user.UserID;
            _context.Add(reliefProject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        // GET: ReliefProjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reliefProject = await _context.ReliefProjects.FindAsync(id);
            if (reliefProject == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "Email", reliefProject.UserID);
            return View(reliefProject);
        }

        // POST: ReliefProjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReliefProjectID,ProjectName,Description,Location,StartDate,EndDate,Status,UserID")] ReliefProject reliefProject)
        {
            if (id != reliefProject.ReliefProjectID)
            {
                return NotFound();
            }
            var userLoggedin = await _userManager.GetUserAsync(User);
            var user = _context.Users.FirstOrDefault(u => u.Email == userLoggedin.Email);
            reliefProject.User = user;
            reliefProject.UserID = user.UserID;

            try
                {
                    _context.Update(reliefProject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReliefProjectExists(reliefProject.ReliefProjectID))
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

        // GET: ReliefProjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reliefProject = await _context.ReliefProjects
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.ReliefProjectID == id);
            if (reliefProject == null)
            {
                return NotFound();
            }

            return View(reliefProject);
        }

        // POST: ReliefProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reliefProject = await _context.ReliefProjects.FindAsync(id);
            if (reliefProject != null)
            {
                _context.ReliefProjects.Remove(reliefProject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReliefProjectExists(int id)
        {
            return _context.ReliefProjects.Any(e => e.ReliefProjectID == id);
        }
    }
}
