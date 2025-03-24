using Diplom2.Db;
using Diplom2.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmenaController : ControllerBase
    {
        private DiplomContext _context;

        public SmenaController(DiplomContext context)
        {
            _context = context;
        }

        [HttpGet] //Вывод 
        public async Task<ActionResult<IEnumerable<SmenaDTO>>> GetSmena()
        {
            if (_context.Smenas == null)
            {
                return NotFound();
            }
            var type = await _context.Smenas.Select(s =>
                 new SmenaDTO
                 {
                     IdSmena = s.IdSmena,
                     StartSmena = s.StartSmena,
                     EndSmena = s.EndSmena,
                 }
             ).ToListAsync();
            return Ok(type);
        }

        [HttpGet("{id}")] //Вывод id
        public async Task<ActionResult<SmenaDTO>> GetSmena(int id)
        {
            if (_context.Smenas == null)
            {
                return NotFound();
            }
            var type = await _context.Smenas.FirstOrDefaultAsync(s => s.IdSmena == id);
            if (type == null)
            {
                return NotFound(type);

            }
            var typeDTO = new SmenaDTO
            {
                IdSmena = type.IdSmena,
                StartSmena = type.StartSmena,
                EndSmena = type.EndSmena,
            };
            return Ok(typeDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditSmena(int id, SmenaDTO lentaDto)
        {
            if (_context.Smenas == null)
            {
                return NotFound();
            }
            var lenta = _context.Smenas.FirstOrDefault(l => l.IdSmena == id);
            if (lenta == null)
            {
                return NotFound();
            }


            lenta.StartSmena = lentaDto.StartSmena;
            lenta.EndSmena = lentaDto.EndSmena;


            _context.Update(lenta);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost] // Добавление 
        public async Task<IActionResult> AddSmena(SmenaDTO user)
        {

            if (user == null)
            {
                return BadRequest("Пользователь не может быть null.");
            }

            var newUser = new Smena
            {


                StartSmena = user.StartSmena,
                EndSmena = user.EndSmena,


            };

            await _context.AddAsync(newUser);  // Используйте асинхронный метод для добавления
            await _context.SaveChangesAsync();  // Используйте асинхронный метод для сохранения изменений

            return Ok("Пользователь успешно добавлен."); // Вернуть ответ о результате
        }



        [HttpDelete("{id}")] //Удаление 
        public async Task<IActionResult> DeleteSmena(int id)
        {
            if (_context.Smenas == null)
            {
                return NotFound();
            }
            var toy = await _context.Smenas.FindAsync(id);
            if (toy == null)
            {
                return NotFound(toy);

            }

            var hren = _context.Raspisanies.Where(s => s.IdSmena == id).ToList();
            _context.Raspisanies.RemoveRange(hren);
            _context.Smenas.Remove(toy);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
