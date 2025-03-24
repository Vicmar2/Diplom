using Diplom2.Db;
using Diplom2.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Diplom2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenHandler _jwtTokenHandler;
        private DiplomContext _context;
        public AuthController(JwtTokenHandler jwtTokenHandler, DiplomContext context)
        {
            _jwtTokenHandler = jwtTokenHandler;
            _context = context;
        }

        [HttpPost("login")] 
        public async Task<IActionResult> Login([FromBody] AuthData sotru)
        {
            string hashedPassword = await HashPasswordAsync(sotru.Password);
            var user = _context.Sotruds.FirstOrDefault(u => u.LoginSotrud == sotru.Login
       && u.ParolSotrud == hashedPassword);
            //var user = _context.Sotruds.FirstOrDefault(l => l.LoginSotrud == sotru.Login && l.ParolSotrud == sotru.Password);
            if (user !=  null)
            {
                var userId = Guid.NewGuid(); 
                var role = "sotrud"; 
                var (token, _) = _jwtTokenHandler.GenerateJwtToken(role, userId);
                return Ok(new { Token = token });
            }
            


            return Unauthorized(); 
        }

        private static async Task<string> HashPasswordAsync(string password)
        {
            var bytes = Encoding.ASCII.GetBytes(password);
            StringBuilder result = new StringBuilder();
            using (var md5 = MD5.Create())
            using (var ms = new MemoryStream(bytes))
            {
                var hash = await md5.ComputeHashAsync(ms);
                foreach (var b in hash)
                    result.Append(b.ToString("x2"));
            }
            return result.ToString();
        }

    }

 
}

