using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("api/home/get")]
        public IActionResult Get()
        {
            var json = "{\"registration_ids\":[\"id1\",\"id2\"],\"data\":{\"message\":\"Your message\",\"tickerText\":\"Your ticket\",\"contentTitle\":\"Your content\"}}";
            return Ok(json);
        }
    }
}
