using System;
using System.Collections.Generic;
using System.Text;
using BacklogActual.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BacklogActual.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<List> List { get; set; }
        public DbSet<MovieList> MovieList { get; set; }
    }
}
