using Microsoft.EntityFrameworkCore;
using RESERVATION.Models;
using Microsoft.Extensions.Configuration;

namespace RESERVATION.Data
{
    public class ReservationContext : DbContext
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
        }
        public DbSet<RESERVATION.Models.T_COURSE>? T_COURSE { get; set; }
        public DbSet<RESERVATION.Models.T_OPTION>? T_OPTION { get; set; }
        public DbSet<RESERVATION.Models.T_USER>? T_USER { get; set; }
        public DbSet<RESERVATION.Models.T_COURSEM>? T_COURSEM { get; set; }
    }
}