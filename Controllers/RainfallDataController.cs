using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SortedCodingTest.Models;
using SortedCodingTest.Services;
using System.Net.Http;
using System.Xml.Linq;

namespace SortedCodingTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RainfallDataController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public RainfallDataController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRainfallFloodData()
        {
            var externalUrl = "https://environment.data.gov.uk/flood-monitoring/id/floods";

            try
            {
                var response = await _httpClient.GetStringAsync(externalUrl);

                var jsonObject_response = JsonConvert.DeserializeObject<JObject>(response);

                // Get index items
                var items = jsonObject_response["items"];

                // Convert the item back to a JSON string
                var item_json = items.ToString();

                var rainfallData = JsonConvert.DeserializeObject<List<RainfallData>>(item_json);

                return new JsonResult(rainfallData);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error retrieving rainfall floods data: {ex.Message}");
            }
        }
    }
}
