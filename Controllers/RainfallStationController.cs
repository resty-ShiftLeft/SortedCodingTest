using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SortedCodingTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RainfallStationController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public RainfallStationController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

    }
}
