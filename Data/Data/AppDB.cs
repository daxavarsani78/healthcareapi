using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data
{
    public class AppDB : IdentityDbContext<Tbl_Users>
    {
        public AppDB(DbContextOptions<AppDB> options) : base(options)
        {

        }
        public DbSet<Tbl_Users> Tbl_Users { get; set; }
        public DbSet<Tbl_Enquiry> Tbl_Enquiry { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Modified)
                {
                    ((BaseEntity)entityEntry.Entity).UpdatedOn = DateTime.Now;
                }

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedOn = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}
