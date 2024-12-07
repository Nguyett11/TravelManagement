using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace BookTourProcess.Service
{
    public class TourServiceClient
    {
        private readonly HttpClient _httpClient;

        public TourServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        

        //Lấy danh sách Tour
        public async Task<List<Tour>> GetToursAsync()
        {     
            var response = await _httpClient.GetAsync("http://localhost:5001/api/Tours");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Tour>>(content);
        }

        // Lấy thông tin chi tiết một Tour theo ID
        public async Task<Tour?> GetTourByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"http://localhost:5001/api/Tours/{id}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error fetching tour with ID {id}. Status Code: {response.StatusCode}");
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Tour>(content);
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
