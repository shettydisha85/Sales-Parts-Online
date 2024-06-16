using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesPartsOnline.Models;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Asp.Versioning;

namespace SalesPartsOnline.Controllers
{
    [Route("api/token")]
    [ApiController]
    [Produces("application/json")]
    [ApiVersion(1)]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost("authenticate")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public IActionResult GetToken()
        {
            var username = User.Identity.Name;

            var key = Encoding.ASCII.GetBytes("YourSuperSecretKeyHere");
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token) });
        }
    }
}