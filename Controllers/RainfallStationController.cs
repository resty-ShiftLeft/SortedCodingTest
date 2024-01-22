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
            var externalUrl = "https://environment.data.gov.uk/flood-monitoring/id/stations/" + id;

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

                var _measure = new List<RainfallStationMeasures>();
                foreach (var measure in items["measures"])
                {
                    _measure.Add(new RainfallStationMeasures
                    {
                        id = measure["@id"].ToString(),
                        datumType = measure["datumType"]?.ToString(),
                        label = measure["label"].ToString(),
                        notation = measure["notation"].ToString(),
                        parameter = measure["parameter"].ToString(),
                        parameterName = measure["parameterName"].ToString(),
                        period = int.Parse(measure["period"].ToString()),
                        qualifier = measure["qualifier"].ToString(),
                        station = measure["station"].ToString(),
                        stationReference = measure["stationReference"].ToString(),
                        type = new List<string>(){ measure["type"].ToString() }, 
                        unit = measure["unit"].ToString(),
                        unitName = measure["unitName"].ToString(),
                        valueType = measure["valueType"].ToString()
                    });
                }

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
                    measures = _measure,
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
