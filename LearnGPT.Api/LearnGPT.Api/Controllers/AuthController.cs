using LearnGPT.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearnGPT.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            return Ok();
        }
    }
}
