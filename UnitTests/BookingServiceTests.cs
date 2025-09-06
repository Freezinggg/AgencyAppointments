using AgencyAppointments.Application.Common;
using AgencyAppointments.Application.DTO.Booking;
using AgencyAppointments.Application.Interfaces.Booking;
using AgencyAppointments.Application.Interfaces.DaysOff;
using AgencyAppointments.Application.Interfaces.Setting;
using AgencyAppointments.Application.Services.Booking;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class BookingServiceTests
    {
        [Fact]
        public async Task BookingAppointment_ShouldReturnSuccess_WhenBookingAvailable()
        {
            // Arrange
            var bookingRepoMock = new Mock<IBookingRepository>();
            var daysOffRepoMock = new Mock<IDaysOffRepository>();
            var settingRepoMock = new Mock<ISettingRepository>();

            var maxBookingSetting = new AgencyAppointments.Domain.Entities.Setting.Setting
            {
                Key = "MAX_BOOKING_COUNT_DAILY",
                Value = 10
            };

            // Mock repository methods
            settingRepoMock.Setup(x => x.GetByKeyAsync("MAX_BOOKING_COUNT_DAILY"))
                .ReturnsAsync(maxBookingSetting);

            daysOffRepoMock.Setup(x => x.GetByDateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync((AgencyAppointments.Domain.Entities.DaysOff.DaysOff?)null);

            bookingRepoMock.Setup(x => x.GetCountByDateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(0);

            bookingRepoMock.Setup(x => x.SaveAsync(It.IsAny<AgencyAppointments.Domain.Entities.Booking.Booking>()))
                .ReturnsAsync((AgencyAppointments.Domain.Entities.Booking.Booking b) => b);

            var service = new BookingService(bookingRepoMock.Object, daysOffRepoMock.Object, settingRepoMock.Object);

            var dto = new CreateBookingDTO
            {
                Name = "Test User",
                Email = "test@example.com",
                BookingDate = DateTime.Today
            };

            // Act
            var result = await service.BookingAppointment(dto);

            // Assert
            result.Status.Should().Be(ResultStatus.Success);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("Test User", result.Data!.Name);
            Assert.Equal("test@example.com", result.Data.Email);
        }

        [Fact]
        public async Task BookingAppointment_ShouldFail_WhenDayIsOff()
        {
            // Arrange
            var mockBookingRepo = new Mock<IBookingRepository>();
            var mockDaysOffRepo = new Mock<IDaysOffRepository>();
            var mockSettingRepo = new Mock<ISettingRepository>();

            // setup: day is off (holiday found)
            mockDaysOffRepo
                .Setup(x => x.GetByDateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new AgencyAppointments.Domain.Entities.DaysOff.DaysOff
                {
                    Id = Guid.NewGuid(),
                    DaysOffDate = DateTime.Today
                });

            var service = new BookingService(
                mockBookingRepo.Object,
                mockDaysOffRepo.Object,
                mockSettingRepo.Object
            );

            var dto = new CreateBookingDTO
            {
                Name = "Test User",
                Email = "test@email.com",
                BookingDate = DateTime.Today
            };

            // Act
            var result = await service.BookingAppointment(dto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Status.Should().Be(ResultStatus.Fail);
            result.ErrorMessage.Should().Be("Booking date is a day off, cannot proceed to book");
            mockBookingRepo.Verify(x => x.SaveAsync(It.IsAny<AgencyAppointments.Domain.Entities.Booking.Booking>()), Times.Never);
        }
    }
}
