using Newtonsoft.Json;
using static BookTourProcess.Service.TourServiceClient;
using System.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using static BookTourProcess.Service.OrderServiceClient;
using System.Text;

namespace BookTourProcess.Service
{
    public class ReportServiceClient
    {
        private readonly HttpClient _httpClient;

        public ReportServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Phương thức tạo báo cáo orders_reports 
        public async Task<List<orders_reports>> GenerateOrderReportsAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token không được để trống.", nameof(token));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpContent = new StringContent(JsonConvert.SerializeObject(new { Token = token }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5003/api/orders_reports/generate", httpContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<orders_reports>>(content);
        }


        // Phương thức tạo báo cáo product_reports 
        public async Task<List<product_reports>> GenerateProductReportsAsync(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token không được để trống.", nameof(token));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var httpContent = new StringContent(JsonConvert.SerializeObject(new { Token = token }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:5003/api/product_reports", httpContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<product_reports>>(content);
        }

        //Xem danh sách báo cáo đơn hàng
        public async Task<List<orders_reports>> GetOrderReportsAsync(string token)
        {
       
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("http://localhost:5003/api/orders_reports");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<orders_reports>>(content);
        }

        //Xem một báo cáo đơn hàng bằng id
        public async Task<orders_reports> GetOrderReportByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"http://localhost:5003/api/orders_reports/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<orders_reports>(content);
        }

        //Xem danh sách báo cáo sản phẩm
        public async Task<List<product_reports>> GetTourReportsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("http://localhost:5003/api/product_reports");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<product_reports>>(content);
        }

        //Xem một báo cáo sản phẩm bằng id
        public async Task<product_reports> GetTourReportByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"http://localhost:5003/api/product_reports/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<product_reports>(content);
        }

        public class orders_reports
        {
            public int id { get; set; }
            public int order_id { get; set; }
            public decimal total_revenue { get; set; }
            public decimal total_cost { get; set; }
            public decimal total_profit { get; set; }
        }

        public class product_reports
        {
            [Key]
            public int id { get; set; }
            public int order_report_id { get; set; }
            public int product_id { get; set; }
            public int total_sold { get; set; }
            public decimal revenue { get; set; }
            public decimal cost { get; set; }
            public decimal profit { get; set; }
        }

    }
}
