using BookTourProcess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static BookTourProcess.Service.OrderServiceClient;

namespace BookTourProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderServiceClient _orderServiceClient;

        public OrderController(OrderServiceClient orderService)
        {
            _orderServiceClient = orderService;
        }

        public class CreateOrderRequest
        {
            public Order NewOrder { get; set; }
            public string Token { get; set; }
        }

        // Phương thức tạo đơn hàng
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (request.NewOrder == null)
            {
                return BadRequest("Đơn hàng không hợp lệ.");
            }

            // Sử dụng token từ đối tượng yêu cầu
            var token = request.Token;

            try
            {
                var createdOrder = await _orderServiceClient.AddOrderAsync(request.NewOrder, token);
                return CreatedAtAction(nameof(CreateOrder), new { id = createdOrder.id }, createdOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi tạo đơn hàng: {ex.Message}");
            }
        }

        public class TokenRequest
        {
            public string Token { get; set; }
        }

        //Phương thức tạo chi tiết đơn hàng
        [HttpPost("create-product-report")]
        public async Task<IActionResult> CreateProductReport(int productId, int quantity, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                var orderDetail = await _orderServiceClient.CreateProductReportAsync(productId, quantity, token);
                return Ok(orderDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating product report: {ex.Message}");
            }
        }


        //Phương thức cập nhật status bằng PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] CreateOrderRequest request)
        {
            if (id <= 0)
            {
                return BadRequest("ID đơn hàng không hợp lệ.");
            }

            if (request.NewOrder == null)
            {
                return BadRequest("Thông tin đơn hàng không hợp lệ.");
            }

            if (request.NewOrder.id != id)
            {
                return BadRequest("ID đơn hàng trong URL không khớp với ID trong dữ liệu.");
            }

            var token = request.Token;

            try
            {
                var updatedOrder = await _orderServiceClient.UpdateOrderAsync(request.NewOrder, token);
                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi cập nhật đơn hàng: {ex.Message}");
            }
        }



    }
}