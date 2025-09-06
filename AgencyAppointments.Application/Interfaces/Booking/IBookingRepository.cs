using AgencyAppointments.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Application.Interfaces.Booking
{
    public interface IBookingRepository
    {
        Task<Domain.Entities.Booking.Booking?> SaveAsync(Domain.Entities.Booking.Booking item);
        Task<IEnumerable<Domain.Entities.Booking.Booking>> GetAllAsync();
        Task<IEnumerable<Domain.Entities.Booking.Booking>> GetAllAsync(DateTime date);
        Task<int> GetCountByDateAsync(DateTime date);
    }
}
