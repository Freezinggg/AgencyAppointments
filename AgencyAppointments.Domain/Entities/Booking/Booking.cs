using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Domain.Entities.Booking
{
    public class Booking
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
