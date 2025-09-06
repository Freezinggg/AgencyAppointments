using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Domain.Entities.Setting
{
    public class Setting
    {
        public Guid Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public int Value { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
