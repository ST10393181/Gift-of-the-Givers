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
    public class DonationsController : Controller
    {
        private readonly GiftOfGiversContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DonationsController(GiftOfGiversContext context,UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Donations
        public async Task<IActionResult> Index()
        {
            var giftOfGiversContext = _context.Donations.Include(d => d.Disaster).Include(d => d.Resource);
            return View(await giftOfGiversContext.ToListAsync());
        }

        // GET: Donations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.Disaster)
                .Include(d => d.Resource)
                .FirstOrDefaultAsync(m => m.Donation_ID == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // GET: Donations/Create
        public IActionResult Create()
        {
            ViewData["Disaster_ID"] = new SelectList(_context.Disasters, "Disaster_ID", "Name");
            ViewData["Resource_ID"] = new SelectList(_context.Resources, "Resource_ID", "Name");
            return View();
        }

        // POST: Donations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Donation_ID,DonorName,Amount,DonationDate,Resource_ID,Disaster_ID")] Donation donation)
        {
            donation.Disaster = _context.Disasters.Find(donation.Disaster_ID);
            var user = await _userManager.GetUserAsync(User);
            var userDetails = _context.Users.FirstOrDefault(u => u.Email == user.Email);


            Resource resource = new Resource
            {
                Name = "Monetary Resource",
                Type = "Financial Support",
                Quantity = 1,
                StorageLocation = "Facilty 2",
                LastUpdated = DateTime.UtcNow,
                UserID = userDetails.UserID
            };
            donation.Resource = resource;
            donation.Resource_ID = donation.Resource.Resource_ID;

            _context.Add(donation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        // GET: Donations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }
            ViewData["Disaster_ID"] = new SelectList(_context.Disasters, "Disaster_ID", "Name", donation.Disaster_ID);
            ViewData["Resource_ID"] = new SelectList(_context.Resources, "Resource_ID", "Name", donation.Resource_ID);
            return View(donation);
        }

        // POST: Donations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Donation_ID,DonorName,Amount,DonationDate,Resource_ID,Disaster_ID")] Donation donation)
        {
            if (id != donation.Donation_ID)
            {
                return NotFound();
            }

            donation.Disaster = _context.Disasters.Find(donation.Disaster_ID);
            var user = await _userManager.GetUserAsync(User);
            var userDetails = _context.Users.FirstOrDefault(u => u.Email == user.Email);


            donation.Resource = new Resource
            {
                Name = "Monetary Resource",
                Type = "Financial Support",
                Quantity = 1,
                StorageLocation = "Facilty 2",
                LastUpdated = DateTime.UtcNow,
                UserID = userDetails.UserID,
            };
            donation.Resource_ID = donation.Resource.Resource_ID;

            try
                {

                    _context.Update(donation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DonationExists(donation.Donation_ID))
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

        // GET: Donations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donation = await _context.Donations
                .Include(d => d.Disaster)
                .Include(d => d.Resource)
                .FirstOrDefaultAsync(m => m.Donation_ID == id);
            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        // POST: Donations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation != null)
            {
                _context.Donations.Remove(donation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonationExists(int id)
        {
            return _context.Donations.Any(e => e.Donation_ID == id);
        }
    }
}
