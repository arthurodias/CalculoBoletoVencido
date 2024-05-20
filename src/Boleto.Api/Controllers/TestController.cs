using Microsoft.AspNetCore.Mvc;

namespace Boleto.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var token = Request.Headers["Authorization"].ToString();
            return Ok(new { Token = token });
        }
    }
}