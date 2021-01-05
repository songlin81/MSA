using Microsoft.AspNetCore.Mvc;

namespace SalesAPI.Controllers.API
{
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        [HttpGet("status")]        
        public IActionResult Status() => Ok();
    }
}
