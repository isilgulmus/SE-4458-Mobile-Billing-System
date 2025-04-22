using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileBillingSystem.DTO;
using MobileBillingSystem.DTO.MobileBillingSystem.DTOs;
using MobileBillingSystem.Models;
using MobileBillingSystem.Service;

namespace MobileBillingSystem.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]

    public class BillController : ControllerBase
    {
        private readonly BillService _billService;

        public BillController(BillService billService)
        {
            _billService = billService;
        }

        [HttpPost("CalculateBill")]
        [Authorize]
        public async Task<IActionResult> CalculateBill([FromBody] CalculateBillDTO dto)
        {
            var result = await _billService.CalculateBill(dto);
            return Ok(result);
        }

        [HttpGet("QueryBill")]
        public async Task<IActionResult> QueryBill([FromQuery] int subscriberNo, [FromQuery] int month, [FromQuery] int year)
        {
            var dto = new QueryBillDTO
            {
                SubscriberNo = subscriberNo,
                Month = month,
                Year = year
            };

            var result = await _billService.QueryBill(dto);
            return Ok(result);
        }

        [HttpGet("QueryDetailedBill")]
        [Authorize]
        public async Task<IActionResult> QueryBillDetailed([FromQuery] int subscriberNo, [FromQuery] int month, [FromQuery] int year, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var dto = new QueryBillDetailedDTO
            {
                SubscriberNo = subscriberNo,
                Month = month,
                Year = year,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _billService.QueryBillDetailed(dto);
            return Ok(result);
        }

        [HttpPost("PayBill")]
        public async Task<IActionResult> PayBill([FromBody] PayBillDTO dto)
        {
            var result = await _billService.PayBill(dto);
            return Ok(result);
        }

    }
}
