using Microsoft.AspNetCore.Mvc;

namespace Api1 {
    
    [Route("/api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("api1");
        }
    }
}