using AgencyAppointments.Application.Common;
using AgencyAppointments.Application.DTO.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Application.Interfaces.Setting
{
    public interface ISettingService
    {
        Task<Result<SettingDTO?>> CreateAsync(SettingDTO settingDTO);
        Task<Result<bool>> UpdateAsync(SettingDTO settingDTO);
        Task<Result<SettingDTO?>> GetByKeyAsync(string key);
        Task<Result<IEnumerable<SettingDTO>>> GetAllAsync();
    }
}
