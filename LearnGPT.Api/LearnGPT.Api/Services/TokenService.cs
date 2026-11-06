
using LearnGPT.Api.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LearnGPT.Api.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            // 1. Read JWT configuration
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var secretKey = _configuration["Jwt:Key"];

            var expiryMinutes = _configuration
                .GetValue<int>("Jwt:ExpiryMinutes");

            if (string.IsNullOrWhiteSpace(issuer) ||
                string.IsNullOrWhiteSpace(audience) ||
                string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException(
                    "JWT configuration is missing.");
            }

            if (Encoding.UTF8.GetByteCount(secretKey) < 32)
            {
                throw new InvalidOperationException(
                    "JWT signing key must be at least 32 bytes.");
            }

            if (expiryMinutes <= 0)
            {
                throw new InvalidOperationException(
                    "JWT expiry must be greater than zero.");
            }

            // 2. Create user claims
            var claims = new[]
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.Id.ToString()),

                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email),

                new Claim(
                    ClaimTypes.Role,
                    user.Role),

                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            };

            // 3. Create the signing key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey));

            // 4. Define the signing algorithm
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            // 5. Define token expiry
            var expires = DateTime.UtcNow.AddMinutes(expiryMinutes);

            // 6. Create the JWT
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            // 7. Convert JWT object into a string
            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
