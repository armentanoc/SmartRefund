using SmartRefund.WebAPI.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartRefund.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login(LoginRequest loginRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(loginRequest.UserName) ||
                string.IsNullOrEmpty(loginRequest.Password))
                    return BadRequest("Username and/or Password not specified");
                if (loginRequest.UserName.Equals("employee") &&
                loginRequest.Password.Equals("employee123"))
                {
                    var secretKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes("thisisasecretkey@12345678901234567890"));
                    var signinCredentials = new SigningCredentials
                    (secretKey, SecurityAlgorithms.HmacSha256);

                    //var claims = new List<Claim>()
                    //{
                    //    new Claim("userType", "employee"), // Tipo de usuário
                    //    new Claim("userId", "1")           // ID do usuário
                    //};

                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: "ABCXYZ",
                        claims: new List<Claim>(),
                        audience: "http://localhost:51398",
                        expires: DateTime.Now.AddDays(15),
                        signingCredentials: signinCredentials
                    );

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), userType = "employee" });
                }

                if (loginRequest.UserName.Equals("finance") &&
                loginRequest.Password.Equals("finance123"))
                {
                    var secretKey = new SymmetricSecurityKey
                   (Encoding.UTF8.GetBytes("thisisasecretkey@12345678901234567890"));
                    var signinCredentials = new SigningCredentials
                    (secretKey, SecurityAlgorithms.HmacSha256);

                    var claims = new List<Claim>()
                    {
                        new Claim("userType", "finance"), // Tipo de usuário
                        new Claim("userId", "2")      // ID do usuário
                    };

                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: "ABCXYZ",
                        claims: claims,
                        audience: "http://localhost:51398",
                        expires: DateTime.Now.AddDays(15),
                        signingCredentials: signinCredentials
                    );

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), userType = "finance" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred in generating the token: {ex}");
            }
            return Unauthorized();
        }
    }
}
