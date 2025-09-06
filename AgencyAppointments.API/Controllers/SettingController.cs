using AgencyAppointments.Application.Common;
using AgencyAppointments.Application.DTO.Setting;
using AgencyAppointments.Application.Interfaces.Setting;
using AgencyAppointments.Application.Services.Setting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgencyAppointments.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settingService;
        private readonly ILogger<SettingController> _logger;

        public SettingController(ISettingService settingService, ILogger<SettingController> logger)
        {
            _settingService = settingService;
            _logger = logger;
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SettingDTO parameter)
        {
            try
            {

                Result<bool> result = await _settingService.UpdateAsync(parameter);
                if (result.IsNotFound)
                    return NotFound("Setting doesnt exist.");

                return result.IsSuccess ? Ok(ApiResponse<object>.Ok("Update Setting success.")) : BadRequest(ApiResponse<object>.Fail(result.ErrorMessage));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Update Catalog");
                return StatusCode(500, ApiResponse<object>.Fail("Updating error, internal server problem."));
            }
        }
    }
}
