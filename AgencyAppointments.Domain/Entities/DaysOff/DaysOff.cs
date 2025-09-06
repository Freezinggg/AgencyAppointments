using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Domain.Entities.DaysOff
{
    public class DaysOff
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime DaysOffDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
