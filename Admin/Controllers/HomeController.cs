using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Services;

namespace RealEstate.Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITestServices _testServices;

        public HomeController(ITestServices testServices)
        {
            _testServices = testServices;
        }

        [HttpGet]
        [Route("api/home/get")]
        public IActionResult Get()
        {
            _testServices.TestData();
            var json = "{\"registration_ids\":[\"id1\",\"id2\"],\"data\":{\"message\":\"Your message\",\"tickerText\":\"Your ticket\",\"contentTitle\":\"Your content\"}}";
            return Ok(json);
        }
    }
}
