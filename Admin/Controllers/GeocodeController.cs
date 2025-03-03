using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeocodeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly GoogleMapsSettings _googleMapsSettings;
        private readonly string _googleapisGeocode;

        public GeocodeController(IHttpClientFactory httpClientFactory, IOptions<GoogleMapsSettings> googleMapsSettings, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _googleMapsSettings = googleMapsSettings.Value;
            _configuration = configuration;
        }

        [HttpGet("geocode")]
        public async Task<IActionResult> GeocodeAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return BadRequest("Address is required.");
            }

            try
            {
                string encodedAddress = System.Net.WebUtility.UrlEncode(address);
                string apiKey = _googleMapsSettings.ApiKey;
                //_configuration["GoogleMaps:GoogleapisGeocode"]
                string url = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodedAddress}&key={apiKey}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                JsonDocument document = JsonDocument.Parse(json);
                JsonElement root = document.RootElement;

                if (root.GetProperty("status").GetString() == "OK")
                {
                    JsonElement location = root.GetProperty("results")[0].GetProperty("geometry").GetProperty("location");
                    double latitude = location.GetProperty("lat").GetDouble();
                    double longitude = location.GetProperty("lng").GetDouble();

                    return Ok(new { Latitude = latitude, Longitude = longitude });
                }
                else
                {
                    return BadRequest($"Geocoding failed: {root.GetProperty("status").GetString()}");
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
            catch (JsonException ex)
            {
                return StatusCode(500, $"Error parsing JSON: {ex.Message}");
            }
        }
    }
}
