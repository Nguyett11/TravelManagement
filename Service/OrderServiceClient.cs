using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using static Reporting.Service.OrderServiceClient;

namespace Reporting.Service
{
    public class OrderServiceClient
    {
        private readonly HttpClient _httpClient;

        public OrderServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Order>> GetOrdersAsync(string token)
        {
            // Thêm token do người dùng nhập vào header Authorization
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("http://localhost:5002/api/orders");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Order>>(content);
        }

        public async Task<List<orderDetail>> GetOrderDetailsAsync(string token)
        {
            // Thêm token do người dùng nhập vào header Authorization
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("http://localhost:5002/api/orderDetails");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<orderDetail>>(content);
        }

        public class Order
        {
            public int id { get; set; }
            public string customer_name { get; set; }
            public string customer_email { get; set; }
            public decimal total_amount { get; set; }
            public string status { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }

           

        }

        public class orderDetail
        {
            public int id { get; set; }
            public int order_id { get; set; }
            public int product_id { get; set; }
            public string product_name { get; set; }
            public int quantity { get; set; }
            public decimal unit_price { get; set; }
            public decimal total_price { get; set; }

        }
    }
}
