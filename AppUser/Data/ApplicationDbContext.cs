using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppUser.Models;

namespace AppUser.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<ProfilesModel> Profiles { get; set; }
        public DbSet<WebSitesMenuModel> WebSitesMenu { get; set; }
        public DbSet<WebSitesModel> WebSites { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WebSitesMenuModel>()
                .HasIndex(m => new { m.ProfileId, m.SequenceNumber });
        }
    }
}
