using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.DataConection;
using OrderManagement.Models;

namespace OrderManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class orderDetailsController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public orderDetailsController(DataBaseContext context)
        {
            _context = context;
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

        // POST: api/orderDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<orderDetail>> PostorderDetail(orderDetail orderDetail)
        {
            _context.OrderDetail.Add(orderDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetorderDetail", new { id = orderDetail.id }, orderDetail);
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
