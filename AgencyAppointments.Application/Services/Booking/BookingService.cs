using AgencyAppointments.Application.Common;
using AgencyAppointments.Application.DTO.Booking;
using AgencyAppointments.Application.DTO.DaysOff;
using AgencyAppointments.Application.Interfaces.Booking;
using AgencyAppointments.Application.Interfaces.DaysOff;
using AgencyAppointments.Application.Interfaces.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AgencyAppointments.Application.Services.Booking
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IDaysOffRepository _daysOffRepo;
        private readonly ISettingRepository _settingRepo;

        public BookingService(IBookingRepository bookingRepository, IDaysOffRepository daysOffRepository, ISettingRepository settingRepository)
        {
            _bookingRepo = bookingRepository;
            _daysOffRepo = daysOffRepository;
            _settingRepo = settingRepository;
        }

        public async Task<Result<BookingDTO?>> BookingAppointment(CreateBookingDTO dto)
        {
            var isBookingAvailable = false;

            DateTime currentBookingDate = dto.BookingDate.Date;

            //Check holiday
            var isDayOffResult = await IsDayOff(currentBookingDate);
            if (!isDayOffResult.IsSuccess) return Result<BookingDTO?>.Fail("Booking date is a day off, cannot proceed to book");

            //Loop until booking date is available, potential infinite loop might need to add max retry date ex:check 30 days ahead only
            //TODO: Might need lock to avoid race condition
            while (!isBookingAvailable)
            {
                var isBookingAvailableResult = await IsBookingAvailable(currentBookingDate);

                if(isBookingAvailableResult.IsSuccess) isBookingAvailable = true;
                else currentBookingDate = currentBookingDate.AddDays(1);
            }

            Domain.Entities.Booking.Booking booking = new()
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                BookingDate = currentBookingDate, //to avoid npgsql tz error
            };

            booking.Token = await GenerateToken(booking.BookingDate);

            var result = await _bookingRepo.SaveAsync(booking);
            if (result != null)
                return Result<BookingDTO?>.Success(new BookingDTO()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Email = result.Email,
                    BookingDate = result.BookingDate,
                    Token = result.Token,
                });

            return Result<BookingDTO?>.Fail("Booking appointment failed, due to internal server error. Please try different later.");
        }

        public async Task<Result<IEnumerable<BookingDTO>>> GetAllAsync()
        {
            var result = await _bookingRepo.GetAllAsync();

            IEnumerable<BookingDTO> dtos = result.Select(x => new BookingDTO
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                BookingDate = x.BookingDate,
                Token = x.Token,
            });

            return Result<IEnumerable<BookingDTO>>.Success(dtos.OrderBy(x => x.BookingDate));
        }

        public async Task<Result<IEnumerable<BookingDTO>>> GetAllAsync(DateTime date)
        {
            var result = await _bookingRepo.GetAllAsync(date);

            IEnumerable<BookingDTO> dtos = result.Select(x => new BookingDTO
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                BookingDate = x.BookingDate.Date,
                Token = x.Token,
            });

            return Result<IEnumerable<BookingDTO>>.Success(dtos.OrderBy(x => x.BookingDate));
        }

        private async Task<Result<bool>> IsBookingAvailable(DateTime bookingDate)
        {
            Domain.Entities.Setting.Setting? setting = await _settingRepo.GetByKeyAsync("MAX_BOOKING_COUNT_DAILY");
            if (setting != null)
            {
                //Check maximum daily cap
                int currentBookingCount = await _bookingRepo.GetCountByDateAsync(bookingDate);
                if (currentBookingCount >= setting.Value) return Result<bool>.Fail("Booking already reached max capacity for today.");

                return Result<bool>.Success(true);
            }

            return Result<bool>.Error("Configuration for [MAX_BOOKING_COUNT_DAILY] is not yet configured.");
        }

        private async Task<Result<bool>> IsDayOff(DateTime bookingDate)
        {
            Domain.Entities.DaysOff.DaysOff? daysOff = await _daysOffRepo.GetByDateAsync(bookingDate);
            if (daysOff != null) return Result<bool>.Fail("Booking date is a day-off day.");

            return Result<bool>.Success(true);
        }

        private async Task<string> GenerateToken(DateTime bookingDate)
        {
            int count = await _bookingRepo.GetCountByDateAsync(bookingDate);
            string random = Guid.NewGuid().ToString("N")[..4].ToUpper();
            return $"{bookingDate:yyyyMMdd}-{count + 1:D3}-{random}";
        }

    }
}
