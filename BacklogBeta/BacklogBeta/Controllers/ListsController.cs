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
using BacklogBeta.Models.ViewModel;

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

        // GET: List
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

            var listDetails = new MovieListDetailsViewModel()
            {

                List = await _context.List
                .Include(l => l.User)
                .Include(l => l.MovieList).ThenInclude(l => l.Movie)
                .FirstOrDefaultAsync(m => m.ListId == id)

            };


            if (listDetails == null)
            {
                return NotFound();
            }




            if (listDetails.List.UserId != user.Id)
            {
                return NotFound();
            }


            return View(listDetails);
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
        // GET: List/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);


            if (id == null)
            {
                return NotFound();
            }


            
            var movielistList = await _context.MovieList.Include(m => m.Movie).Include(m => m.List).Where(m => m.ListId == id).ToListAsync();

            var viewModel = new MovieListEditViewModel()

            {
                AllMovies = await _context.Movie.Where(m => m.User == user).ToListAsync(),
                MovieList = movielistList,
                SelectedMovies = movielistList.Select(ml => ml.MovieId).ToList(),
                List = await _context.List.FindAsync(id),
               


            };
            if (viewModel.AllMovieOptions == null)
            {
                return NotFound();
            }

            if (viewModel.MovieList == null)
            {
                return NotFound();
            }

            
            return View(viewModel);
        }


        // POST: List/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MovieListEditViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);





            if (id != viewModel.List.ListId)
            {
                return NotFound();
            }
            ModelState.Remove("List.UserId");
            ModelState.Remove("List.User");
            ModelState.Remove("Movie.User");
            ModelState.Remove("Movie.Title");
            ModelState.Remove("Movie.Description");
            ModelState.Remove("Movie.Director");
            ModelState.Remove("Movie.UserId");





            if (ModelState.IsValid)
            {
               


                try
                {

                    var moviesInList = _context.MovieList.Where(mIL => mIL.ListId == id).ToList();

                    foreach (var originalMovie in moviesInList)
                    {
                        var originalMovieId = originalMovie.MovieId;
                        var movieInSelectedList = viewModel.SelectedMovies.Find(mISL => mISL == originalMovieId);

                        if (movieInSelectedList == 0)
                        {
                            _context.MovieList.Remove(originalMovie);
                        }
                    }



                    foreach (var selectedMovie in viewModel.SelectedMovies)
                    {
                        var movieInOriginalList = moviesInList.Find(m => m.MovieId == selectedMovie);

                        if(movieInOriginalList == null)
                        {
                            var SelectedMovieInstance = new MovieList
                            {
                                
                                ListId = id,
                                MovieId = selectedMovie
                            };

                            _context.MovieList.Add(SelectedMovieInstance);
                        }

                    }  


                    viewModel.List.UserId = user.Id;
                    _context.Update(viewModel.List);
                    await _context.SaveChangesAsync();
                  
                }




                catch (DbUpdateConcurrencyException)
                {
                    if (!ListExists(viewModel.List.ListId))
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
            return View(viewModel.List);
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
                .Where(l => l.ListId == id)
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
            var ListRefIds = _context.MovieList.Where(lrfi => lrfi.List.ListId == id);
            foreach (var listId in ListRefIds)
            {
                _context.MovieList.Remove(listId);

            }

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