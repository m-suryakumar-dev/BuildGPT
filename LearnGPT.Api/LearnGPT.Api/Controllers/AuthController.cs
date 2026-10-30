using LearnGPT.Api.Models;
using LearnGPT.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearnGPT.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        private readonly TokenService _tokenService;
        public AuthController(AuthenticationService authenticationService, TokenService tokenService)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _authenticationService.ValidateUser(
                request.Email,
                request.Password);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }
            var token =  _tokenService.GenerateToken(user);

            return Ok(new
            {
                message = "Login successful.",
                userId = user.Id,
                email = user.Email,
                jwt = token
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            bool registered = await _authenticationService.RegisterUser(
                request.Name,
                request.Email,
                request.Password);

            if (!registered)
            {
                return Conflict(new
                {
                    message = "An account with this email already exists."
                });
            }

            return Ok(new
            {
                message = "User registered successfully."
            });
        }
    }
}
