using AgencyAppointments.Application.Common;
using AgencyAppointments.Application.DTO.Booking;
using AgencyAppointments.Application.Interfaces.Booking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace AgencyAppointments.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingService _bookingService;

        public BookingController(ILogger<BookingController> logger, IBookingService bookingService)
        {
            _logger = logger;
            _bookingService = bookingService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _bookingService.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<BookingDTO>>.Ok(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Get All Booking");
                return StatusCode(500, ApiResponse<object>.Fail("Get booking error, internal server problem."));
            }
        }

        [HttpGet("by-date")]
        public async Task<IActionResult> GetAllByDate([FromQuery] DateTime date)
        {
            try
            {
                var result = await _bookingService.GetAllAsync(date);
                return Ok(ApiResponse<IEnumerable<BookingDTO>>.Ok(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Get All Booking");
                return StatusCode(500, ApiResponse<object>.Fail("Get booking error, internal server problem."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment([FromBody] CreateBookingDTO parameter)
        {
            try
            {
                var result = await _bookingService.BookingAppointment(parameter);

                return result.IsSuccess ? Ok(ApiResponse<BookingDTO?>.Ok(result.Data)) : BadRequest(ApiResponse<object>.Fail(result.ErrorMessage));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Book Appointment");
                return StatusCode(500, ApiResponse<object>.Fail("Book Appointment error, internal server problem."));
            }
        }
    }
}
