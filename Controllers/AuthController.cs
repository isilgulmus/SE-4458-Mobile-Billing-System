using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MobileBillingSystem.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Kullanıcı doğrulama işlemi (burada basit bir kontrol yapıyoruz, gerçek uygulamada veritabanı kontrolü yapılır)
            if (request.Username != "testuser" || request.Password != "password")
                return Unauthorized("Invalid credentials");

            // JWT token oluşturuluyor
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Username),
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("UbyNbFvlonVCj-WPoZRnlXWGZRQZTxIAhuIxX7obGt4"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "MobileBillingAPI",  // Issuer
                audience: "MobileBillingClient",  // Audience
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
