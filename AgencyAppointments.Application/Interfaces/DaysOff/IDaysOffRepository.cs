using AgencyAppointments.Domain.Entities.DaysOff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Application.Interfaces.DaysOff
{
    public interface IDaysOffRepository
    {
        Task<bool> IsDayOffAsync(DateTime date);
        Task<Domain.Entities.DaysOff.DaysOff?> GetByIdAsync(Guid id);
        Task<Domain.Entities.DaysOff.DaysOff?> GetByDateAsync(DateTime date);
        Task<Domain.Entities.DaysOff.DaysOff?> CreateAsync(Domain.Entities.DaysOff.DaysOff item);
        Task<IEnumerable<Domain.Entities.DaysOff.DaysOff>> GetAllAsync();
    }
}
