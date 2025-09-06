using AgencyAppointments.Application.Common;
using AgencyAppointments.Application.DTO.DaysOff;
using AgencyAppointments.Application.Interfaces.DaysOff;
using AgencyAppointments.Domain.Entities.DaysOff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Application.Services.DaysOff
{
    public class DaysOffService : IDaysOffService
    {
        private readonly IDaysOffRepository _repo;
        public DaysOffService(IDaysOffRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<DaysOffDTO?>> CreateAsync(CreateDaysOffDTO createDaysOffDTO)
        {
            try
            {
                //Check for existing days off
                if (await _repo.GetByDateAsync(createDaysOffDTO.Date) == null)
                {
                    Domain.Entities.DaysOff.DaysOff item = new()
                    {
                        Id = Guid.NewGuid(),
                        Description = createDaysOffDTO.Description,
                        DaysOffDate = createDaysOffDTO.Date,
                    };

                    var result = await _repo.CreateAsync(item);
                    if (result == null)
                        Result<DaysOffDTO?>.Fail("Failed to create days off.");

                    return Result<DaysOffDTO?>.Success(new DaysOffDTO() { Id = result.Id, Date = result.DaysOffDate });
                }

                return Result<DaysOffDTO?>.Fail("Failed to create days off because picked date already exist.");
            }
            catch
            {
                return Result<DaysOffDTO?>.Error("Internal server error, failed to create days off.");
            }
        }

        public async Task<Result<IEnumerable<DaysOffDTO>>> GetAllAsync()
        {
            var result = await _repo.GetAllAsync();

            IEnumerable<DaysOffDTO> dtos = result.Select(x => new DaysOffDTO
            {
                Id = x.Id,
                Description = x.Description,
                Date = x.DaysOffDate.ToLocalTime()
            });

            return Result<IEnumerable<DaysOffDTO>>.Success(dtos);
        }

        public async Task<Result<DaysOffDTO?>> GetByDateAsync(DateTime date)
        {
            var result = await _repo.GetByDateAsync(date);
            if (result == null) return Result<DaysOffDTO?>.NotFound($"No days off found for date: {date}.");

            return Result<DaysOffDTO?>.Success(new DaysOffDTO { Id = result.Id, Date = result.DaysOffDate });
        }

        public async Task<Result<DaysOffDTO?>> GetByIdAsync(Guid id)
        {
            Domain.Entities.DaysOff.DaysOff? dysoff = await _repo.GetByIdAsync(id);
            if (dysoff == null) return Result<DaysOffDTO>.NotFound("Days Off not found.");

            DaysOffDTO dto = new()
            {
                Id = dysoff.Id,
                Description = dysoff.Description,
                Date = dysoff.DaysOffDate.ToLocalTime()
            };

            return Result<DaysOffDTO?>.Success(dto);


        }

        public async Task<Result<bool>> IsDayOffAsync(DateTime date)
        {
            //Check first if days off exist or not
            var exist = await _repo.GetByDateAsync(date);
            if (exist == null) return Result<bool>.NotFound($"No days off found for date: {date}.");

            var result = await _repo.IsDayOffAsync(date);
            return Result<bool>.Success(result);
        }
    }
}
