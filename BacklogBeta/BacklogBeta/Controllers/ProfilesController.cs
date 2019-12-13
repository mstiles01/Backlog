using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BacklogBeta.Data;
using BacklogBeta.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BacklogBeta.Controllers
{

    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfilesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        // GET: Products/Details/5
        public async Task<IActionResult> ProfileDetails()
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);

            user.Lists = _context.List
               .Where(l => l.UserId == user.Id).ToList();

            return View(user);
        }
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            user.Id = id;

            if (User == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.C:\Users\marla\workspace\cSharp\groupProjects\bangazon-site-cotton-headed-ninny-muggins\Bangazon\Views\Users\
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser applicationUser)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            user.FirstName = applicationUser.FirstName;
            user.LastName = applicationUser.LastName;
            user.CityState = applicationUser.CityState;



            if (ModelState.IsValid)
            {
                try
                {

                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(ProfileDetails));
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                            ModelState.AddModelError("", error.Description);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProfileDetails));
            }
            return View(user);
        }



        private bool UserExists(string id)
        {
            return _context.ApplicationUsers.Any(e => e.Id == id);
        }
    }
}

