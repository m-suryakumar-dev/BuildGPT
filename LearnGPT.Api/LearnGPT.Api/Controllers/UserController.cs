using LearnGPT.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace LearnGPT.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("my-profile")]
        public IActionResult GetMyProfile()
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            var email =
                User.FindFirst(ClaimTypes.Email)?.Value
                ?? User.FindFirst("email")?.Value;

            return Ok(new
            {
                message = "User identified successfully!",
                userId = userId,
                email = email,
                isAuthenticated = User.Identity?.IsAuthenticated
            });
        }

        [Authorize(Roles = "User")]
        [HttpGet("dashboard")]
        public IActionResult GetUserDashboard()
        {
            var userId =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            var email =
                User.FindFirst(ClaimTypes.Email)?.Value
                ?? User.FindFirst("email")?.Value;

            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                message = "Welcome to your LearnGPT dashboard!",
                userId = userId,
                email = email,
                role = role,
                isAuthenticated = User.Identity?.IsAuthenticated
            });
        }

    }

}