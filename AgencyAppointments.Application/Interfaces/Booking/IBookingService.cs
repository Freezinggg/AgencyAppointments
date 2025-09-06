using AgencyAppointments.Application.Common;
using AgencyAppointments.Application.DTO.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Application.Interfaces.Booking
{
    public interface IBookingService
    {
        Task<Result<BookingDTO?>> BookingAppointment(CreateBookingDTO dto);
        Task<Result<IEnumerable<BookingDTO>>> GetAllAsync();
        Task<Result<IEnumerable<BookingDTO>>> GetAllAsync(DateTime date);
    }
}
