using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using static BookTourProcess.Service.OrderServiceClient;

namespace BookTourProcess.Service
{
    public class OrderServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly TourServiceClient _tourService;

        public OrderServiceClient(HttpClient httpClient, TourServiceClient tourService)
        {
            _httpClient = httpClient;
            _tourService = tourService;
        }

        // Thêm đơn hàng
        public async Task<Order> AddOrderAsync(Order newOrder, string token)
        {
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

           
            var jsonContent = JsonConvert.SerializeObject(newOrder);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

          
            var response = await _httpClient.PostAsync("http://localhost:5002/api/Orders", httpContent);
            response.EnsureSuccessStatusCode();

          
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(content);
        }

        //Tạo OrderDetails
        public async Task<orderDetail> CreateProductReportAsync(int productId, int quantity, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is required.", nameof(token));
            }

            
            var request = new TokenRequest { Token = token };
            var jsonRequest = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync($"http://localhost:5002/api/orderDetails", httpContent); // Cập nhật URL với đường dẫn chính xác

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error calling API: {errorMessage}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<orderDetail>(content);
        }

        public class TokenRequest
        {
            public string Token { get; set; }
        }


        // Cập nhật trạng thái đơn hàng
        public async Task<Order> UpdateOrderAsync(Order updatedOrder, string token)
        {
           
            if (updatedOrder == null) throw new ArgumentNullException(nameof(updatedOrder));
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentException("Token không được để trống", nameof(token));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = JsonConvert.SerializeObject(updatedOrder);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            if (updatedOrder.id <= 0) throw new ArgumentException("ID đơn hàng không hợp lệ", nameof(updatedOrder.id));

            string url = $"http://localhost:5002/api/Orders/{updatedOrder.id}";

            var response = await _httpClient.PutAsync(url, httpContent);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(content);
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
            public string Token { get; set; }

        }
    }
}
