using Microsoft.AspNetCore.Mvc;

namespace Api2 {
    
    [Route("/api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("api2");
        }
    }
}