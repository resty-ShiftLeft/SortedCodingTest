using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SortedCodingTest.Models;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;

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

        /// <summary>
        /// Gets a list of rainfall readings.
        /// </summary>
        /// <returns>A list of rainfall readings.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<RainfallStationData>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetAllRainfall(string id)
        {
            var externalUrl = "https://environments.data.gov.uk/flood-monitoring/id/stations/" + id;

            try
            {
                using (HttpClient client = new HttpClient())
                {                  
                    try
                    {
                        HttpResponseMessage message = await client.GetAsync(externalUrl);

                        if(message.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            return NotFound("No readings found for the specified stationId");
                        } else if(message.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        {
                            Console.WriteLine("Internal server error.");
                        } else if (message.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            Console.WriteLine("Invalid request.");
                        }
                        else
                        {
                            Console.WriteLine($"Error: {message.StatusCode}");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"HTTP request error: {ex.Message}");
                    }
                }
                  
                var response = await _httpClient.GetStringAsync(externalUrl);
                var jsonObject_response = JsonConvert.DeserializeObject<JObject>(response);

                // Get index items
                var items = jsonObject_response["items"];

                var mappedData = new RainfallStationData()
                {
                    id = items["@id"].ToString(),
                    RLOIid = int.Parse(items["RLOIid"].ToString()),
                    catchmentName = items["catchmentName"].ToString(),
                    dateOpened = DateOnly.Parse(items["dateOpened"].ToString()),
                    eaAreaName = items["eaAreaName"].ToString(),
                    eaRegionName = items["eaRegionName"].ToString(),
                    easting = int.Parse(items["easting"].ToString()),
                    label = items["label"].ToString(),
                    lat = items["lat"].ToString(),
                    long_ = items["long"].ToString(),
                    measures = new List<RainfallStationMeasures> { new RainfallStationMeasures()
                        {
                            id = items["measures"][0]["@id"].ToString(),
                            datumType = items["measures"][0]["datumType"]?.ToString(),
                            label = items["measures"][0]["label"].ToString(),
                            notation = items["measures"][0]["notation"].ToString(),
                            parameter = items["measures"][0]["parameter"].ToString(),
                            parameterName = items["measures"][0]["parameterName"].ToString(),
                            period = int.Parse(items["measures"][0]["period"].ToString()),
                            qualifier = items["measures"][0]["qualifier"].ToString(),
                            station = items["measures"][0]["station"].ToString(),
                            stationReference = items["measures"][0]["stationReference"].ToString(),
                            unit = items["measures"][0]["unit"].ToString(),
                            unitName = items["measures"][0]["unitName"].ToString(),
                            valueType = items["measures"][0]["valueType"].ToString()
                        }
                    },
                    northing = int.Parse(items["northing"].ToString()),
                    notation = items["notation"].ToString(),
                    riverName = items["riverName"].ToString(),
                    stationReference = items["stationReference"].ToString(),
                    status = items["status"].ToString(),
                    town = items["town"].ToString(),
                    wiskiID = items["wiskiID"].ToString(),
                };

                return new JsonResult(mappedData);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest($"Error retrieving rainfall station data: {ex.Message}");
            }
        }
    }
}
