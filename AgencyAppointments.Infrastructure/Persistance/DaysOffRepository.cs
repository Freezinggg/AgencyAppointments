using AgencyAppointments.Application.Interfaces.DaysOff;
using AgencyAppointments.Domain.Entities.DaysOff;
using AgencyAppointments.Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Infrastructure.Persistance
{
    public class DaysOffRepository : IDaysOffRepository
    {
        private readonly AppDbContext _db;
        public DaysOffRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<DaysOff?> CreateAsync(DaysOff item)
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

        public async Task<IEnumerable<DaysOff>> GetAllAsync()
        {
            return await _db.DaysOff.AsNoTracking().ToListAsync();
        }

        public async Task<DaysOff?> GetByDateAsync(DateTime date)
        {
            return await _db.DaysOff
            .FirstOrDefaultAsync(x => x.DaysOffDate.Date == date.Date);
        }

        public async Task<DaysOff?> GetByIdAsync(Guid id)
        {
            return await _db.DaysOff.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsDayOffAsync(DateTime date)
        {
            return await _db.DaysOff.AnyAsync(x => x.DaysOffDate.Date == date.Date);
        }
    }
}
