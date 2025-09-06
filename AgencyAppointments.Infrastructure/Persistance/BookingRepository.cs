using AgencyAppointments.Application.Interfaces.Booking;
using AgencyAppointments.Domain.Entities.Booking;
using AgencyAppointments.Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Infrastructure.Persistance
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _db;
        public BookingRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await _db.Bookings.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllAsync(DateTime date)
        {
            return await _db.Bookings.Where(x => x.BookingDate.Date == date.Date).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetByDateAsync(DateTime date)
        {
            return await _db.Bookings.Where(x => x.BookingDate.Date == date.Date).AsNoTracking().ToListAsync();
        }

        public async Task<int> GetCountByDateAsync(DateTime date)
        {
            return await _db.Bookings.Where(x => x.BookingDate.Date == date.Date).AsNoTracking().CountAsync();
        }

        public async Task<Booking?> SaveAsync(Booking item)
        {
            try
            {
                _db.Add(item);
                await _db.SaveChangesAsync();

                return item;
            }
            catch
            {
                return null;
            }
        }
    }
}
