using BookTourProcess.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookTourProcess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        private readonly TourServiceClient _tourServiceClient;

        public TourController(TourServiceClient tourServiceClient)
        {
            _tourServiceClient = tourServiceClient;
        }

        [HttpGet]
        public async Task<ActionResult<List<TourServiceClient.Tour>>> GetTours()
        {
            // Lấy danh sách tours từ TourServiceClient
            var tours = await _tourServiceClient.GetToursAsync();
            return Ok(tours);
        }
    }
}
