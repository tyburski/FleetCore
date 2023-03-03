using FleetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FleetCore
{
    public class FleetCoreDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Bonus> Bonuses { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<Refueling> Refuelings { get; set; }
        public DbSet<Repair> Repairs { get; set; }

        private string _connectionString =
            "Server=(localdb)\\mssqllocaldb;Database=FleetCoreDb;Trusted_Connection=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Organization>()
                .HasMany(x=>x.Users)
                .WithOne(x => x.Organization)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Organization>()
                .HasMany(x => x.Vehicles)
                .WithOne(x => x.Organization)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            

            modelBuilder.Entity<Vehicle>()
                .HasMany(x => x.Events)
                .WithOne(x => x.Vehicle)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vehicle>()
                .HasMany(x => x.Repairs)
                .WithOne(x => x.Vehicle)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Vehicle>()
                .HasMany(x => x.Refuelings)
                .WithOne(x => x.Vehicle)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany(x => x.Bonuses)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasMany(x => x.Leaves)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);
           
            modelBuilder.Entity<AppUser>()
                .HasMany(x => x.Refuelings)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasMany(x => x.Repairs)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasMany(x => x.Notices)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasOne(x => x.Vehicle)
                .WithMany(x => x.Users)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
