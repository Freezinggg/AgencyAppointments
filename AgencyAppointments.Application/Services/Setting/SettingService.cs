using AgencyAppointments.Application.Common;
using AgencyAppointments.Application.DTO.DaysOff;
using AgencyAppointments.Application.DTO.Setting;
using AgencyAppointments.Application.Interfaces.Setting;
using AgencyAppointments.Domain.Entities.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Application.Services.Setting
{
    public class SettingService : ISettingService
    {
        private readonly ISettingRepository _repo;
        public SettingService(ISettingRepository settingRepository)
        {
            _repo = settingRepository;
        }

        public async Task<Result<SettingDTO?>> CreateAsync(SettingDTO settingDTO)
        {
            Domain.Entities.Setting.Setting setting = new()
            {
                Id = Guid.NewGuid(),
                Key = settingDTO.Key,
                Value = settingDTO.Value,
            };

            var result = await _repo.CreateAsync(setting);
            if(result == null)
                Result<DaysOffDTO?>.Fail("Failed to create setting.");
            
            return Result<SettingDTO?>.Success(settingDTO);
        }

        public async Task<Result<bool>> UpdateAsync(SettingDTO settingDTO)
        {
            Domain.Entities.Setting.Setting? setting = await _repo.GetByKeyAsync(settingDTO.Key);
            if(setting != null)
            {
                setting.Value = settingDTO.Value;
                bool result = await _repo.UpdateAsync(setting);

                if (!result) return Result<bool>.Fail("Update setting failed.");

                return Result<bool>.Success(result);
            }

            return Result<bool>.NotFound("Setting not found.");
        }

        public async Task<Result<SettingDTO?>> GetByKeyAsync(string key)
        {
            Domain.Entities.Setting.Setting? setting = await _repo.GetByKeyAsync(key);
            if (setting == null)
                return Result<SettingDTO?>.NotFound("Setting not found.");

            return Result<SettingDTO?>.Success(new SettingDTO { Id = setting.Id, Key = setting.Key, Value = setting.Value });
        }

        public Task<Result<IEnumerable<SettingDTO>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
