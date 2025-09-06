using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Application.Interfaces.Setting
{
    public interface ISettingRepository
    {
        Task<Domain.Entities.Setting.Setting?> CreateAsync(Domain.Entities.Setting.Setting setting);
        Task<bool> UpdateAsync(Domain.Entities.Setting.Setting setting);
        Task<Domain.Entities.Setting.Setting?> GetByKeyAsync(string key);
        Task<IEnumerable<Domain.Entities.Setting.Setting?>> GetAllAsync();
    }
}
