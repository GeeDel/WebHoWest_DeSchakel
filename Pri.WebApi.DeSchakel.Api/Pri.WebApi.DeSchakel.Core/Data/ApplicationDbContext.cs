using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pri.WebApi.DeSchakel.Core.Data.Seeding;
using Pri.WebApi.DeSchakel.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Pri.WebApi.DeSchakel.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        
        public DbSet<Company> Companies { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        //
        public DbSet<NavigationItem> Navigations { get; set; }



        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // seeding
            Seeder.Seed(builder);
            //
            base.OnModelCreating(builder);
        }
    }
}
