using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SortedCodingTest.Models;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using System.Collections.Generic;

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
        [Route("get-rainfall")]
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
               
                //latestReading
                var _latestReading = new List<RainfallStationMeasuresLastReading>();
                //measures
                var _measure = new List<RainfallStationMeasures>();
                //stageScale
                var _stageScale = new List<RainfallStationStageScaleData>();


                foreach (var latestReading in items["measures"])
                {
                    _latestReading.Add(new RainfallStationMeasuresLastReading
                    {
                        id = latestReading["@id"].ToString(),
                        date = DateOnly.Parse(latestReading["latestReading"]["date"].ToString()),
                        dateTime = DateTime.Parse(latestReading["latestReading"]["dateTime"].ToString()),
                        measure = latestReading["latestReading"]["measure"].ToString(),
                        value = latestReading["latestReading"]["value"].ToString(),
                    });
                }

                foreach (var measure in items["measures"])
                {
                    _measure.Add(new RainfallStationMeasures
                    {
                        id = measure["@id"].ToString(),
                        datumType = measure["datumType"]?.ToString(),
                        label = measure["label"].ToString(),
                        latestReading = _latestReading,
                        notation = measure["notation"].ToString(),
                        parameter = measure["parameter"].ToString(),
                        parameterName = measure["parameterName"].ToString(),
                        period = int.Parse(measure["period"].ToString()),
                        qualifier = measure["qualifier"].ToString(),
                        station = measure["station"].ToString(),
                        stationReference = measure["stationReference"].ToString(),
                        type = new List<string>()
                            { 
                                measure["type"].ToString() 
                            }, 
                        unit = measure["unit"].ToString(),
                        unitName = measure["unitName"].ToString(),
                        valueType = measure["valueType"].ToString()
                    });
                }

                var _highestRecent = new List<RainfallStationHighestRecent>();
                var _maxOnRecord = new List<RainfallStationMaxOnRecord>();
                var _minOnRecord = new List<RainfallStationMinOnRecord>();

                _highestRecent.Add(new RainfallStationHighestRecent
                {
                    id = items["stageScale"]["highestRecent"]["@id"].ToString(),
                    dateTime = DateTime.Parse(items["stageScale"]["highestRecent"]["dateTime"].ToString()),
                    value = double.Parse(items["stageScale"]["highestRecent"]["value"].ToString())
                });
                _maxOnRecord.Add(new RainfallStationMaxOnRecord
                {
                    id = items["stageScale"]["maxOnRecord"]["@id"].ToString(),
                    dateTime = DateTime.Parse(items["stageScale"]["maxOnRecord"]["dateTime"].ToString()),
                    value = double.Parse(items["stageScale"]["maxOnRecord"]["value"].ToString())
                });
                _minOnRecord.Add(new RainfallStationMinOnRecord
                {
                    id = items["stageScale"]["minOnRecord"]["@id"].ToString(),
                    dateTime = DateTime.Parse(items["stageScale"]["minOnRecord"]["dateTime"].ToString()),
                    value = double.Parse(items["stageScale"]["minOnRecord"]["value"].ToString())
                });

                _stageScale.Add(new RainfallStationStageScaleData
                {
                    id = items["stageScale"]["@id"].ToString(),
                    datum = double.Parse(items["stageScale"]["datum"].ToString()),
                    
                    highestRecent = _highestRecent,
                    maxOnRecord = _maxOnRecord,
                    minOnRecord = _minOnRecord,

                    scaleMax = int.Parse(items["stageScale"]["scaleMax"].ToString()),
                    typicalRangeHigh = double.Parse(items["stageScale"]["typicalRangeHigh"].ToString()),
                    typicalRangeLow = double.Parse(items["stageScale"]["typicalRangeLow"].ToString())
                });

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
                    
                    stageScaleData = _stageScale,
                    
                    stationReference = items["stationReference"].ToString(),
                    status = items["status"].ToString(),
                    town = items["town"].ToString(),
                    type = new List<string>() 
                        { 
                            items["type"].ToString() 
                        },
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
