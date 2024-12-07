using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace Reporting.Service
{
    public class TourServiceClient
    {
        private readonly HttpClient _httpClient;

        public TourServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Tour>> GetToursAsync(string token)
        {
            // Thêm token do người dùng nhập vào header Authorization
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("http://localhost:5001/api/Tours");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Tour>>(content);
        }

        public class Tour
        {
            public int Id { get; set; }
            public string? Ten { get; set; }
            public int category_id { get; set; }
            public string? NoiXuatPhat { get; set; }
            public string? NoiDen { get; set; }
            public string? ThoiGian { get; set; }
            public int? Gia { get; set; }
            public string? Anh { get; set; }
        }
    }
}
