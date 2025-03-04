using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        [HttpPost("generate")]
        public IActionResult GenerateToken()
        {
            // Define the secret key and credentials
            // JUST for TEST mode, not for using in real life (keys should not be stored in code in public reposirories)
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKey12345!@#$%67890"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create the claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "user"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Generate the token
            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            // Return the generated token
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}
