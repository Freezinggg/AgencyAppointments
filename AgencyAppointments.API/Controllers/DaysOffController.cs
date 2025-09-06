using AgencyAppointments.Application.Common;
using AgencyAppointments.Application.DTO.DaysOff;
using AgencyAppointments.Application.Interfaces.DaysOff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgencyAppointments.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DaysOffController : ControllerBase
    {
        private readonly IDaysOffService _daysOffService;
        private readonly ILogger<DaysOffController> _logger;
        public DaysOffController(IDaysOffService daysOffService, ILogger<DaysOffController> logger)
        {
            _daysOffService = daysOffService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _daysOffService.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<DaysOffDTO>>.Ok(result.Data));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Get All Day off");
                return StatusCode(500, ApiResponse<object>.Fail("Get all day off error, internal server problem."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var result = await _daysOffService.GetByIdAsync(id);
                return result.IsSuccess ? Ok(ApiResponse<DaysOffDTO?>.Ok(result.Data)) : NotFound(ApiResponse<DaysOffDTO?>.Fail(result.ErrorMessage));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error Get Day Off by Id");
                return StatusCode(500, ApiResponse<object>.Fail("Get day off error, internal server problem."));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDaysOffDTO parameter)
        {
            try
            {
                Result<DaysOffDTO?> result = await _daysOffService.CreateAsync(parameter);
                return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Data.Id }, ApiResponse<Guid>.Ok(result.Data.Id)) : BadRequest(ApiResponse<Guid>.Fail(result.ErrorMessage));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Create Day Off");
                return StatusCode(500, ApiResponse<object>.Fail("Creating day off error, internal server problem."));
            }
        }
    }
}
