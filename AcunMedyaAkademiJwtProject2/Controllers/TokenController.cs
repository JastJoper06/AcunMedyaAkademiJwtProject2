using AcunMedyaAkademiJwtProject2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AcunMedyaAkademiJwtProject2.Controllers
{
    public class TokenController : Controller
    {
        private readonly JwtSettingsModel _jwtSettings;

        public TokenController(IOptions<JwtSettingsModel> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }
        [HttpGet]   
        public IActionResult Generate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Generate(SimpleUserDto simpleUserDto)
        {
            var claims = new[]
            {
                new Claim("name",simpleUserDto.Name),
                new Claim("surname",simpleUserDto.SurName),
                new Claim("city",simpleUserDto.City),
                new Claim("username",simpleUserDto.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
                signingCredentials: credentials
                );
            simpleUserDto.Token= new JwtSecurityTokenHandler().WriteToken(token);
            return View(simpleUserDto);
        }
    }
}
