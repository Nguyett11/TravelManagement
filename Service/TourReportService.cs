using Microsoft.EntityFrameworkCore;
using Reporting.DataConetion;
using Reporting.Models;

namespace Reporting.Service
{
    public class TourReportService
    {
        private readonly TourServiceClient _productServiceClient; // Service để lấy sản phẩm
        private readonly OrderServiceClient _orderServiceClient; // Service để lấy đơn hàng
        private readonly DataBaseContext _context;

        public TourReportService(TourServiceClient productServiceClient, OrderServiceClient orderServiceClient, DataBaseContext context)
        {
            _productServiceClient = productServiceClient;
            _orderServiceClient = orderServiceClient;
            _context = context;
        }

        public async Task<List<product_reports>> GenerateTourReportsAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is required", nameof(token));
            }

            // Lấy dữ liệu từ dịch vụ quản lý sản phẩm
            var products = await _productServiceClient.GetToursAsync(token);
            if (products == null || !products.Any())
            {
                Console.WriteLine("No products found.");
                throw new InvalidOperationException("No products found.");
            }

            // Lấy dữ liệu từ dịch vụ quản lý đơn hàng
            var orders = await _orderServiceClient.GetOrdersAsync(token);
            if (orders == null || !orders.Any())
            {
                Console.WriteLine("No orders found.");
                throw new InvalidOperationException("No orders found.");
            }

            // Lấy dữ liệu từ dịch vụ quản lý chi tiết đơn hàng
            var orderDetails = await _orderServiceClient.GetOrderDetailsAsync(token); // Kiểm tra endpoint này
            if (orderDetails == null || !orderDetails.Any())
            {
                Console.WriteLine("No order details found.");
                throw new InvalidOperationException("No order details found.");
            }

            // Lấy ID báo cáo đơn hàng gần nhất
            var latestOrderReport = await _context.orders_reports.OrderByDescending(or => or.id).FirstOrDefaultAsync();
            if (latestOrderReport == null)
            {
                Console.WriteLine("No order report found.");
                throw new InvalidOperationException("No order report found.");
            }

            var reports = new List<product_reports>();
            Console.WriteLine($"Number of products: {products.Count()}");
            Console.WriteLine($"Number of orders: {orders.Count()}");
            Console.WriteLine($"Number of order details: {orderDetails.Count()}");

            foreach (var product in products)
            {
                // Lấy các chi tiết đơn hàng tương ứng với sản phẩm hiện tại
                var productOrderDetails = orderDetails
                    .Where(od => od.product_id == product.Id);

                // Ghi log số lượng chi tiết đơn hàng cho sản phẩm
                Console.WriteLine($"Product ID: {product.Id}, Number of orderDetails: {productOrderDetails.Count()}");

                if (!productOrderDetails.Any())
                {
                    continue; // Không có chi tiết đơn hàng cho sản phẩm này, tiếp tục với sản phẩm tiếp theo
                }

                // Tính toán các giá trị báo cáo
                int totalSold = productOrderDetails.Sum(od => od.quantity);
                decimal revenue = productOrderDetails.Sum(od => od.total_price);
                decimal cost = productOrderDetails.Sum(od => od.unit_price * od.quantity * 0.7m); // Giả định chi phí là 70% doanh thu
                decimal profit = revenue - cost;

                // Thêm báo cáo cho sản phẩm này
                reports.Add(new product_reports
                {
                    order_report_id = latestOrderReport.id,
                    product_id = product.Id,
                    total_sold = totalSold,
                    revenue = revenue,
                    cost = cost,
                    profit = profit
                });
            }

            if (!reports.Any())
            {
                // Không có báo cáo nào để lưu
                Console.WriteLine("No reports to save.");
            }
            else
            {
                // Lưu báo cáo vào cơ sở dữ liệu
                _context.productReports.AddRange(reports);
                await _context.SaveChangesAsync();
            }

            return reports;
        }




    }
}
