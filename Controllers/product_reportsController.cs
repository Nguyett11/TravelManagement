using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reporting.DataConetion;
using Reporting.Models;
using Reporting.Service;
using static Reporting.Controllers.orders_reportsController;

namespace Reporting.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class product_reportsController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly TourReportService _tourService;

        public product_reportsController(DataBaseContext context, TourReportService tourService)
        {
            _context = context;
            _tourService = tourService;
        }

        // GET: api/product_reports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<product_reports>>> Getproduct_reports()
        {
            return await _context.product_reports.ToListAsync();
        }

        // GET: api/product_reports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<product_reports>> Getproduct_reports(int id)
        {
            var product_reports = await _context.product_reports.FindAsync(id);

            if (product_reports == null)
            {
                return NotFound();
            }

            return product_reports;
        }




        // POST: api/product_reports/{orderReportId}
        [HttpPost]
        public async Task<IActionResult> CreateProductReports([FromBody] TokenRequestt request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                // Gọi phương thức để tạo báo cáo tour mà không cần truyền orderReportId
                var reports = await _tourService.GenerateTourReportsAsync(request.Token);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating reports: {ex.Message}, StackTrace: {ex.StackTrace}");
            }
        }

        public class TokenRequestt
        {
            public string Token { get; set; }
        }


        // DELETE: api/product_reports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteproduct_reports(int id)
        {
            var product_reports = await _context.product_reports.FindAsync(id);
            if (product_reports == null)
            {
                return NotFound();
            }

            _context.product_reports.Remove(product_reports);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool product_reportsExists(int id)
        {
            return _context.product_reports.Any(e => e.id == id);
        }
    }
}
