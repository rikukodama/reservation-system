using Microsoft.EntityFrameworkCore;
using RESERVATION.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace RESERVATION.Data
{
    public class ReservationContext : IdentityDbContext
    {
        public ReservationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<T_COURSE>()
                .HasKey(c => new { c.courceId });
            modelBuilder.Entity<T_OPTION>()
                .HasKey(c => new { c.OptionId });
            modelBuilder.Entity<T_USER>()
                .HasKey(c => new { c.Id });
            modelBuilder.Entity<DateViewModel>()
                .HasNoKey();
            modelBuilder.Entity<OptionViewModel>()
                .HasNoKey();
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
        }
        
        public DbSet<RESERVATION.Models.T_COURSE>? T_COURSE { get; set; }
        public DbSet<RESERVATION.Models.T_OPTION>? T_OPTION { get; set; }
        public DbSet<RESERVATION.Models.T_USER>? T_USER { get; set; }
        public DbSet<RESERVATION.Models.T_COURSEM>? T_COURSEM { get; set; }
        public DbSet<RESERVATION.Models.T_RESERVATION>? T_RESERVATION { get; set; }
    }
}