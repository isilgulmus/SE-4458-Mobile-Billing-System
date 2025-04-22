using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using MobileBillingSystem.DTO;
using MobileBillingSystem.Service;

namespace MobileBillingSystem.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
 
    public class UsageController : ControllerBase
    {
        private readonly UsageService _usageService;

        public UsageController(UsageService usageService)
        {
            _usageService = usageService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddUsage([FromBody] DTO.AddUsageDTO dto)
        {
            var result = await _usageService.AddUsage(dto);
            return Ok(result);
        }
    }
}
