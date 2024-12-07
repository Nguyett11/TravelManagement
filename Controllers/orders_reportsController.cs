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

namespace Reporting.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class orders_reportsController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly OrderReportService _reportService;

        public orders_reportsController(DataBaseContext context, OrderReportService reportService)
        {
            _context = context;
            _reportService = reportService;
        }

        // GET: api/orders_reports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<orders_reports>>> Getorders_reports()
        {
            return await _context.orders_reports.ToListAsync();
        }

        // GET: api/orders_reports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<orders_reports>> Getorders_reports(int id)
        {
            var orders_reports = await _context.orders_reports.FindAsync(id);

            if (orders_reports == null)
            {
                return NotFound();
            }

            return orders_reports;
        }


        [HttpPost("generate")]
        public async Task<IActionResult> GenerateOrderReports([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                var reports = await _reportService.GenerateOrderReportsAsync(request.Token);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating reports: {ex.Message}");
            }
        }

        public class TokenRequest
        {
            public string Token { get; set; }
        }

    // DELETE: api/orders_reports/5
    [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteorders_reports(int id)
        {
            var orders_reports = await _context.orders_reports.FindAsync(id);
            if (orders_reports == null)
            {
                return NotFound();
            }

            _context.orders_reports.Remove(orders_reports);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool orders_reportsExists(int id)
        {
            return _context.orders_reports.Any(e => e.id == id);
        }
    }
}
