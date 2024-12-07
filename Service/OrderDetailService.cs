using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagement.DataConection;
using OrderManagement.Models;

namespace OrderManagement.Service
{
    public class OrderDetailService
    {
        private readonly TourServiceClient _productServiceClient; // Dịch vụ để lấy thông tin sản phẩm
        private readonly DataBaseContext _context;

        public OrderDetailService(TourServiceClient productServiceClient, DataBaseContext context)
        {
            _productServiceClient = productServiceClient;
            _context = context;
        }

        public async Task<orderDetail> AddOrderDetailAsync(int productId, int quantity, string token)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
            }

            // Lấy sản phẩm từ dịch vụ sản phẩm
            var product = await _productServiceClient.GetTourByIdAsync(productId, token);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with ID {productId} not found.");
            }

            // Lấy ID của đơn hàng mới nhất
            var latestOrder = await _context.Order.OrderByDescending(o => o.id).FirstOrDefaultAsync();
            if (latestOrder == null)
            {
                throw new InvalidOperationException("No recent order found.");
            }

            // Tính toán giá trị cho orderDetail
            int unitPrice = product.Gia; // Giá sản phẩm
            decimal totalPrice = unitPrice * quantity;

            // Tạo mới đối tượng orderDetail
            var orderDetails = new orderDetail
            {
                order_id = latestOrder.id,
                product_id = productId,
                product_name = product.Ten,
                quantity = quantity,
                unit_price = unitPrice,
                total_price = totalPrice
            };

            // Thêm vào cơ sở dữ liệu
            _context.OrderDetail.Add(orderDetails);
            await _context.SaveChangesAsync();

            return orderDetails;
        }
    }
}
