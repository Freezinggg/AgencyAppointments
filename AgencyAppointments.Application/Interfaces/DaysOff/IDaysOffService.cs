using AgencyAppointments.Application.Common;
using AgencyAppointments.Application.DTO.DaysOff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Application.Interfaces.DaysOff
{
    public interface IDaysOffService
    {
        Task<Result<DaysOffDTO?>> CreateAsync(CreateDaysOffDTO createDaysOffDTO);
        Task<Result<DaysOffDTO?>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<DaysOffDTO>>> GetAllAsync();
        Task<Result<DaysOffDTO?>> GetByDateAsync(DateTime date);
        Task<Result<bool>> IsDayOffAsync(DateTime date);
    }
}
