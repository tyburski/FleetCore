using FleetCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FleetCore
{
    public class FleetCoreDbContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Bonus> Bonuses { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Refueling> Refuelings { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Notice> Notices { get; set; }

        private string _connectionString =
            "Data Source=mssql.webio.pl,2401;Database=dtyburski0_primasystem;Uid=dtyburski0_dataBaseUser;Password=@r:3Z^49Ta[)YI8$.517tX0/VOCh;TrustServerCertificate=True";

        //@r:3Z^49Ta[)YI8$.517tX0/VOCh
        //"Data Source=mssql.webio.pl,2401;Database=dtyburski0_primasystem;Uid=dtyburski0_dataBaseUser;Password=@r:3Z^49Ta[)YI8$.517tX0/VOCh;TrustServerCertificate=True"

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                .HasMany(x => x.Refuelings)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasMany(x => x.Repairs)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
               .HasMany(x => x.FinishedRepairs)
               .WithOne(x => x.UserFinished)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppUser>()
                .HasOne(x => x.Vehicle)
                .WithMany(x => x.Users)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AppUser>()
               .HasMany(x => x.Notices)
               .WithOne(x => x.User)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
