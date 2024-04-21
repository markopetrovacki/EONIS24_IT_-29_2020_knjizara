using Knjizara.Data;
using Knjizara.Entitets;
using KnjizaraBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KnjizaraBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IKorisnikRepository korisnikRepository; 
        private readonly IConfiguration _configuration;

        public AuthController(IKorisnikRepository korisnikRepository, IConfiguration _configuration) 
        {
            this.korisnikRepository = korisnikRepository;
            this._configuration = _configuration;
        }

        [HttpPost("login")]
        public ActionResult<Korisnik> Login (LoginDto request)
        {
            /* if(korisnik.username != request.username)
             {
                 return BadRequest("Korisnik not found.");
             }

             if(!BCrypt.Net.BCrypt.Verify(request.passwordHash, korisnik.pasword))
             {
                 return BadRequest("Wrong name");
             }*/
            var username = request.username;
            var password = request.passwordHash;
            Korisnik korisnik = korisnikRepository.GetKorisnikByUsernameAndPassword(username, password);
            if (korisnik != null)
            {
                string token = CreateToken(korisnik);
                return Ok(token);
            }
            
            return NotFound();
        }

        private string CreateToken(Korisnik korisnik)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, korisnik.username),
                new Claim(ClaimTypes.Role, korisnik.status_korisnika),
              //  new Claim(ClaimTypes.Role, "User") "Admin"
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
               );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
