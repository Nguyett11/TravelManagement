using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.DataConection;
using OrderManagement.Models;
using OrderManagement.Service;
using static OrderManagement.Controllers.orderDetailsController;

namespace OrderManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class orderDetailsController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly OrderDetailService _orderDetailService;

        public orderDetailsController(DataBaseContext context, OrderDetailService orderDetailService)
        {
            _context = context;
            _orderDetailService = orderDetailService;
        }

        // GET: api/orderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<orderDetail>>> GetOrderDetail()
        {
            return await _context.OrderDetail.ToListAsync();
        }

        // GET: api/orderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<orderDetail>> GetorderDetail(int id)
        {
            var orderDetail = await _context.OrderDetail.FindAsync(id);

            if (orderDetail == null)
            {
                return NotFound();
            }

            return orderDetail;
        }

        // PUT: api/orderDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutorderDetail(int id, orderDetail orderDetail)
        {
            if (id != orderDetail.id)
            {
                return BadRequest();
            }

            _context.Entry(orderDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!orderDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/product_reports/{orderReportId}
        [HttpPost]
        public async Task<IActionResult> CreateProductReports(int productId, int quantity, [FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                // Gọi phương thức để tạo báo cáo tour mà không cần truyền orderReportId
                var reports = await _orderDetailService.AddOrderDetailAsync(productId, quantity, request.Token);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating reports: {ex.Message}, StackTrace: {ex.StackTrace}");
            }
        }

        public class TokenRequest
        {
            public string Token { get; set; }
        }

        // DELETE: api/orderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteorderDetail(int id)
        {
            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            _context.OrderDetail.Remove(orderDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool orderDetailExists(int id)
        {
            return _context.OrderDetail.Any(e => e.id == id);
        }
    }
}
