using AgencyAppointments.Application.Interfaces.Setting;
using AgencyAppointments.Domain.Entities.Setting;
using AgencyAppointments.Infrastructure.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Infrastructure.Persistance
{
    public class SettingRepository : ISettingRepository
    {
        private readonly AppDbContext _db;

        public SettingRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Setting?> CreateAsync(Setting setting)
        {
            try
            {
                _db.Add(setting);
                await _db.SaveChangesAsync();

                return setting;
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<Setting?>> GetAllAsync()
        {
            return await _db.Settings.AsNoTracking().ToListAsync();
        }

        public async Task<Setting?> GetByKeyAsync(string key)
        {
            return await _db.Settings.FirstOrDefaultAsync(x => x.Key == key);
        }

        public async Task<bool> UpdateAsync(Setting setting)
        {
            _db.Update(setting);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
