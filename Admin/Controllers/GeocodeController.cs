using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using NetTopologySuite.Geometries;
using NetTopologySuite;


namespace RealEstate.Admin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeocodeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly GoogleMapsSettings _googleMapsSettings;
        private readonly string _googleapisGeocode;
        private readonly GeometryFactory _geometryFactory;
        // In a real application, you would replace this with a database or other persistent storage.
        private static readonly List<GeoLocation> _locations = new()
        {
            new GeoLocation { Id = 1, Name = "Point A", Latitude = 51.5074, Longitude = 0.1278 }, // London
            new GeoLocation { Id = 2, Name = "Point B", Latitude = 51.5110, Longitude = 0.1298 }, // Near london
            new GeoLocation { Id = 3, Name = "Point C", Latitude = 48.8566, Longitude = 2.3522 }, // Paris
            new GeoLocation { Id = 4, Name = "Point D", Latitude = 40.7128, Longitude = -74.0060 }, // New York
            new GeoLocation { Id = 5, Name = "Point D", Latitude = 13.0448212, Longitude = 77.7485411 },
             new GeoLocation { Id = 6, Name ="Kondasapura", Latitude = 13.045401, Longitude = 77.750944 },
            new GeoLocation { Id = 7, Name = "Kondasapura2", Latitude =13.044638, Longitude =  77.748756 },
            new GeoLocation { Id = 8, Name = "Kondasapura3", Latitude =13.043447, Longitude =  77.747050 },
             new GeoLocation { Id = 9, Name = "virgonagar", Latitude =13.042861, Longitude =  77.744968 },
              new GeoLocation { Id =10, Name = "virgonagar2", Latitude =13.039120, Longitude =  77.739829},
               new GeoLocation { Id = 11, Name = "Avalahalli, 1st Cross Rd, panchite, Aavalahalli, Bengaluru, Karnataka", Latitude =13.035670, Longitude =  77.736096 },
               new GeoLocation { Id = 12, Name = "103, Old Madras Rd, Avalahalli, Aavalahalli, Bengaluru, Karnataka", Latitude =13.033068, Longitude =   77.732598 },
               new GeoLocation { Id =13, Name = "Aavalahalli, Bengaluru, Karnataka 560049", Latitude =13.028019, Longitude =  77.724380 },
               new GeoLocation { Id = 14, Name = "156, Medahalli Kadugodi Rd, Virgonagar Industrial Estate, Aavalahalli, Bengaluru, Karnataka 560049", Latitude =13.026127, Longitude =  77.720989 },
               new GeoLocation { Id = 15, Name = "RMS Colony, TC Palya, Krishnarajapuram, Bengaluru, Karnataka ", Latitude =13.019295, Longitude =  77.708669 },

        };

        public GeocodeController(IHttpClientFactory httpClientFactory, IOptions<GoogleMapsSettings> googleMapsSettings, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _googleMapsSettings = googleMapsSettings.Value;
            _configuration = configuration;
            _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326); // WGS 84

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

        [HttpGet("search-within-distance")]
        public IActionResult SearchWithinDistance(double latitude, double longitude, double distanceInMeters)
        {
            if (distanceInMeters <= 0)
            {
                distanceInMeters = 20000;
            }

            var centerPoint = _geometryFactory.CreatePoint(new Coordinate(longitude, latitude));

            var results = _locations
                .Where(location =>
                {
                    var locationPoint = _geometryFactory.CreatePoint(new Coordinate(location.Longitude, location.Latitude));
                    return centerPoint.Distance(locationPoint) * 111320 <= distanceInMeters; // Approximate conversion to meters
                })
                .ToList();

            return Ok(results);
        }

    }
}