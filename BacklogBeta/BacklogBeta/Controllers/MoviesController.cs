﻿using System;
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
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoviesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Movie
        public async Task<IActionResult> Index(string errorMessage)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var applicationDbContext = _context
                                            .Movie
                                            .Include(movie => movie.User)
                                            .Where(movie => movie.UserId == user.Id);
           

            ViewBag.Error = errorMessage;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(movie => movie.User)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            if (movie.UserId != user.Id)
            {
                return NotFound();
            }


            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            //ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Movie movie)
        {
            ModelState.Remove("User");
            ModelState.Remove("UserId");
            ModelState.Remove("MovieId");
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                movie.UserId = user.Id;
                movie.User = user;
                _context.Add(movie);
                await _context.SaveChangesAsync();
               
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", movie.UserId);
            return View(movie);
        }
        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            if (movie.UserId != user.Id)
            {
                return NotFound();
            }

           

            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", movie.UserId);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (id != movie.MovieId)
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
                    movie.UserId = user.Id;
                    movie.User = user;
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
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
            return View(movie);
        }


        // GET: Movies/Delete/5
        public async Task<IActionResult> FromListDel(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (id == null)
            {
                return NotFound();
            }

            var movieListDelete = await _context.MovieList
                .Include(m => m.Movie)
                .Where(m => m.MovieListId == id)
                .FirstOrDefaultAsync()
                ;
            if (movieListDelete == null)
            {
                return NotFound();
            }

            if (movieListDelete.Movie.UserId != user.Id)
            {
                return NotFound();
            }

            return View(movieListDelete);
        }

        // POST: MovieList/Delete/5
        [HttpPost, ActionName("ListDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ListConfirmedDel(int MovieListId)
       
        {
            
            var movieListRecord = await _context.MovieList.FindAsync(MovieListId);
            var listId = movieListRecord.ListId;
            _context.MovieList.Remove(movieListRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Lists", new { Id = listId });
        }


        private bool MovieExists(int MovieListId)
        {
            return _context.Movie.Any(e => e.MovieId == MovieListId);
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            if (movie.UserId != user.Id)
            {
                return NotFound();
            }
            return View(movie);

           

        }

        // POST: Lists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)

       

        {
            var MovieRefIds =  _context.MovieList.Where(mrfi => mrfi.Movie.MovieId == id);
            foreach (var movieId in MovieRefIds)
            {
               _context.MovieList.Remove(movieId);
              
            }
            { 
                
                var movie = await _context.Movie.FindAsync(id);
                _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
                }
            

        }


    }
}
    

