using Diplom2.Db;
using Diplom2.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Diplom2.Controllers
{
    /// <summary>
    /// vse ok. tolko ydalenie
    /// </summary>
        [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SotrudController : ControllerBase
    {
        private DiplomContext _context;

        public SotrudController(DiplomContext context)
        {
            _context = context;
        }

        [HttpGet] //Вывод 
        public async Task<ActionResult<IEnumerable<SotrudDTO>>> GetSotrud()
        {
            if (_context.Sotruds == null)
            {
                return NotFound();
            }
            var h = await _context.Sotruds.Select(s =>
                new SotrudDTO
                {
                    IdSotrud = s.IdSotrud,
                    NameSotrud = s.NameSotrud,
                    LoginSotrud = s.LoginSotrud,
                    ParolSotrud = s.ParolSotrud,

                }
            ).ToListAsync();
            return Ok(h);
        
         }

        [HttpGet("{id}")] //Вывод id
        public async Task<ActionResult<SotrudDTO>> GetSotrud(int id)
        {
            if (_context.Sotruds == null)
            {
                return NotFound();
            }
            var sotrud = await _context.Sotruds.FirstOrDefaultAsync(s => s.IdSotrud == id);
            if (sotrud == null)
            {
                return NotFound();

            }
            var sotrudDTO = new SotrudDTO
            {
                IdSotrud = sotrud.IdSotrud,
                NameSotrud = sotrud.NameSotrud,
                LoginSotrud = sotrud.LoginSotrud,
                ParolSotrud = sotrud.ParolSotrud,
            };

            return Ok(sotrud);
        }


        [HttpPost] // Добавление 
        public async Task<IActionResult> AddSotrud(SotrudDTO user)
        {
            if (user == null)
            {
                return BadRequest("Пользователь не может быть null.");
            }

            string hashedPassword = await HashPasswordAsync(user.ParolSotrud);


            var newUser = new Sotrud
            {    
                NameSotrud = user.NameSotrud,
                LoginSotrud = user.LoginSotrud,
                ParolSotrud = hashedPassword,
                
            };

            await _context.AddAsync(newUser);  // Используйте асинхронный метод для добавления
            await _context.SaveChangesAsync();  // Используйте асинхронный метод для сохранения изменений

            return Ok("Пользователь успешно добавлен."); // Вернуть ответ о результате
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

        [HttpPut("{id}")]
        public async Task<IActionResult> EditSotrud(int id, SotrudDTO lentaDto)
        {
            if (_context.Sotruds == null)
            {
                return NotFound();
            }
            var lenta = _context.Sotruds.FirstOrDefault(l => l.IdSotrud == id);
            if (lenta == null)
            {
                return NotFound();
            }

            lenta.NameSotrud = lentaDto.NameSotrud;
            lenta.LoginSotrud = lentaDto.LoginSotrud;
            lenta.ParolSotrud = lentaDto.ParolSotrud;

            _context.Update(lenta);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")] //Удаление 
        public async Task<IActionResult> DeleteSotrud(int id)
        {
            if (_context.Sotruds == null)
            {
                return NotFound();
            }
            var toy = await _context.Sotruds.FindAsync(id);
            if (toy == null)
            {
                return NotFound(toy);

            }

            var hren = _context.Raspisanies.Where(s => s.IdSotrud == id).ToList();
            _context.Raspisanies.RemoveRange(hren);

            //_context.Bukets.Remove(tovar);
            //await _context.SaveChangesAsync();
            //return NoContent();

            _context.Sotruds.Remove(toy);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

