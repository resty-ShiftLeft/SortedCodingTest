using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SortedCodingTest.Services;

namespace SortedCodingTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RainfallDataController : ControllerBase
    {
        private readonly RainfallService _rainfallService;

        public RainfallDataController(RainfallService rainfallService)
        {
            _rainfallService = rainfallService;
        }
    }
}
