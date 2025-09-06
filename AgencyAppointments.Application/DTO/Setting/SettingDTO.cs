using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Application.DTO.Setting
{
    public class SettingDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Key is required.")]
        public string Key { get; set; }

        [Required(ErrorMessage = "Value is required.")]
        public int Value { get; set; }
    }
}
