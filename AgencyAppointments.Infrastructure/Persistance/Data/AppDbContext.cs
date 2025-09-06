using AgencyAppointments.Domain.Entities.Booking;
using AgencyAppointments.Domain.Entities.DaysOff;
using AgencyAppointments.Domain.Entities.Setting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Infrastructure.Persistance.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<DaysOff> DaysOff { get; set; } = null!;
        public DbSet<Setting> Settings { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).IsRequired();
                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.Email).IsRequired();
                entity.Property(x => x.Token).IsRequired();
                entity.Property(x => x.BookingDate).IsRequired();
                entity.Property(x => x.CreatedDate);
            });

            modelBuilder.Entity<DaysOff>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).IsRequired();
                entity.Property(x => x.Description).IsRequired();
                entity.Property(x => x.DaysOffDate).IsRequired();
                entity.Property(x => x.CreatedDate);
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id).IsRequired();
                entity.Property(x => x.Key).IsRequired();
                entity.Property(x => x.Value).IsRequired();
                entity.Property(x => x.CreatedDate);
            });
        }
    }
}
