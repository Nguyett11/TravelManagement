using Reporting.DataConetion;
using Reporting.Migrations;
using Reporting.Models;

namespace Reporting.Service
{
    public class OrderReportService
    {
        private readonly OrderServiceClient _orderServiceClient;
        private readonly DataBaseContext _context;

        public OrderReportService(OrderServiceClient orderServiceClient, DataBaseContext context)
        {
            _orderServiceClient = orderServiceClient;
            _context = context;
        }

        public async Task<List<orders_reports>> GenerateOrderReportsAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is required", nameof(token));
            }

            // Gửi token tới OrderServiceClient để lấy danh sách đơn hàng
            var orders = await _orderServiceClient.GetOrdersAsync(token);

            // Chuyển đổi dữ liệu thành báo cáo
            var reports = orders.Select(order =>
            {
                decimal totalCost = order.total_amount * 0.7m; // Chi phí giả định là 70% doanh thu
                decimal totalProfit = order.total_amount - totalCost;

                return new orders_reports
                {
                    order_id = order.id,
                    total_revenue = order.total_amount,
                    total_cost = totalCost,
                    total_profit = totalProfit
                };
            }).ToList();

            // Lưu báo cáo vào cơ sở dữ liệu
            _context.ordersReports.AddRange(reports);
            await _context.SaveChangesAsync();

            int orderReportId = reports.FirstOrDefault()?.id ?? 0; // Lưu lại ID của báo cáo đơn hàng

            return reports;
        }
    }
}
