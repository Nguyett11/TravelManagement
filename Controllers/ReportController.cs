using BookTourProcess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BookTourProcess.Service.ReportServiceClient;

namespace BookTourProcess.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ReportServiceClient _reportServiceClient;

        public ReportController(ReportServiceClient reportServiceClient)
        {
            _reportServiceClient = reportServiceClient;
        }

        public class TokenRequest
        {
            public string Token { get; set; }
        }

        // Phương thức POST để tạo báo cáo đơn hàng
        [HttpPost("generate/orders")]
        public async Task<IActionResult> GenerateOrderReports([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                var reports = await _reportServiceClient.GenerateOrderReportsAsync(request.Token);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating order reports: {ex.Message}");
            }
        }

        // Phương thức POST để tạo báo cáo sản phẩm
        [HttpPost("generate/products")]
        public async Task<IActionResult> GenerateProductReports([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                var reports = await _reportServiceClient.GenerateProductReportsAsync(request.Token);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating product reports: {ex.Message}");
            }
        }

        // Xem danh sách báo cáo đơn hàng
        [HttpGet("orders")]
        public async Task<ActionResult<List<orders_reports>>> GetOrderReports([FromHeader] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }

            var reports = await _reportServiceClient.GetOrderReportsAsync(token);
            return Ok(reports);
        }

        // Xem một báo cáo đơn hàng bằng id
        [HttpGet("orders/{id}")]
        public async Task<ActionResult<orders_reports>> GetOrderReportById(int id, [FromHeader] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                var orderReport = await _reportServiceClient.GetOrderReportByIdAsync(id, token);
                return Ok(orderReport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving order report: {ex.Message}");
            }
        }

        // Xem danh sách báo cáo tour
        [HttpGet("tours")]
        public async Task<ActionResult<List<orders_reports>>> GetTourReports([FromHeader] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }

            var tours = await _reportServiceClient.GetTourReportsAsync(token);
            return Ok(tours);
        }

        // Xem một báo cáo tour bằng id
        [HttpGet("tours/{id}")]
        public async Task<ActionResult<orders_reports>> GetTourReportById(int id, [FromHeader] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                var tourReport = await _reportServiceClient.GetTourReportByIdAsync(id, token);
                return Ok(tourReport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving tour report: {ex.Message}");
            }
        }
    }
}
