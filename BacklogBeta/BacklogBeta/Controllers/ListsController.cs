using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BacklogBeta.Data;
using BacklogBeta.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BacklogBeta.Controllers
{
    [Authorize]
    public class ListsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ListsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Movie
        public async Task<IActionResult> Index(string errorMessage)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var applicationDbContext = _context
                                            .List
                                            .Include(list => list.User)
                                            .Where(list => list.UserId == user.Id);
            ViewBag.Error = errorMessage;
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: Lists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (id == null)
            {
                return NotFound();
            }

            var list = await _context.List
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.ListId == id);
            if (list == null)
            {
                return NotFound();
            }

            if (list.UserId != user.Id)
            {
                return NotFound();
            }

            return View(list);
        }

        // GET: List/Create
        public IActionResult Create()
        {
            //ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: List/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List list)
        {
            ModelState.Remove("User");
            ModelState.Remove("UserId");
            
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                list.UserId = user.Id;
                list.User = user;
                _context.Add(list);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", list.UserId);
            return View(list);
        }
        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _context.List.FindAsync(id);
            if (list == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", list.UserId);
            return View(list);
        }

        // POST: List/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, List list)
        {
            if (id != list.ListId)
            {
                return NotFound();
            }
            ModelState.Remove("UserId");
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    list.UserId = user.Id;
                    list.User = user;
                    _context.Update(list);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListExists(list.ListId))
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
            //ViewData["ProductTypeId"] = new SelectList(_context.ProductType, "ProductTypeId", "Label", viewModel.Product.ProductTypeId);
            return View(list);
        }

        // GET: Lists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (id == null)
            {
                return NotFound();
            }

            var list = await _context.List
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.ListId == id);
            if (list == null)
            {
                return NotFound();
            }

            if (list.UserId != user.Id)
            {
                return NotFound();
            }
            return View(list);
        }

        // POST: Lists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var list = await _context.List.FindAsync(id);
            _context.List.Remove(list);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListExists(int id)
        {
            return _context.List.Any(e => e.ListId == id);
        }
    }
}
