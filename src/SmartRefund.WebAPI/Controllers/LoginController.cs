using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartRefund.Domain.Models;
using SmartRefund.WebAPI.Request;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartRefund.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly List<User> _users;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
            _users = new List<User>()
            {
                new User(1, "employee1", "employee123", "employee"),
                new User(2, "employee2", "employee123", "employee"),
                new User(3, "finance", "finance123", "finance"),
            };
        }


        [HttpPost]
        public IActionResult Login(LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.UserName) || string.IsNullOrEmpty(loginRequest.Password))
                return BadRequest("Username and/or Password not specified");

            var user = ValidateUser(loginRequest.UserName, loginRequest.Password);

            if (user != null)
            {
                var token = GenerateJwtToken(user.Id, user.UserType);
                return Ok(new { token, userType = user.UserType });
            }

            return Unauthorized(new { errorMessage = "Invalid username or password" });
        }

        private User ValidateUser(string userName, string password)
        {
            var user = _users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
            return user;
        }

        private string GenerateJwtToken(uint userId, string userType)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisisasecretkey@12345678901234567890"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("userType", userType),
                new Claim("userId", userId.ToString())
            };

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: "ABCXYZ",
                claims: claims,
                audience: "http://localhost:7088",
                expires: DateTime.Now.AddDays(15),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
