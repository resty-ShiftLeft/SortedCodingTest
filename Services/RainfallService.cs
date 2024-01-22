using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SortedCodingTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace SortedCodingTest.Services
{
    public class RainfallService
    {
        private readonly HttpClient _httpClient;

        public RainfallService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JObject> GetRainfallStationData(string id)
        {
            var externalUrl = "https://environment.data.gov.uk/flood-monitoring/id/stations/"+id;
            var response = await _httpClient.GetStringAsync(externalUrl);

            var jsonObject_response = JsonConvert.DeserializeObject<JObject>(response);

            return jsonObject_response;
        }

        //public async Task<IActionResult> GetRainfallObjectData()
        //{
          
        //}
    }
}
