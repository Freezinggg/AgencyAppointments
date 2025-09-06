using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Application.DTO.DaysOff
{
    public class CreateDaysOffDTO
    {
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Day Off Date is required.")]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
