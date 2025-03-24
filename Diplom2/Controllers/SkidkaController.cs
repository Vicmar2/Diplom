using Diplom2.Db;
using Diplom2.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom2.Controllers
{ /// <summary>
/// ydalenie tolko
/// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SkidkaController : ControllerBase
    {
        private DiplomContext _context;

        public SkidkaController(DiplomContext context)
        {
            _context = context;
        }

        [HttpGet] //Вывод 
        public async Task<ActionResult<IEnumerable<SkidkaDTO>>> GetSkidka()
        {
            if (_context.Skidkas == null)
            {
                return NotFound();
            }
            var h = await _context.Skidkas.Where(e => e.DeleteAt == null).Select(s =>
                new SkidkaDTO
                {
                    IdSkidka= s.IdSkidka,
                    PriceOrder = s.PriceOrder,
                    DeleteAt = s.DeleteAt,
                    NameSkidka = s.NameSkidka
                    
                }
            ).ToListAsync();
            return Ok(h);
        }

        [HttpGet("Delete")] //Вывод 
        public async Task<ActionResult<IEnumerable<SkidkaDTO>>> GetSkidkaDelete()
        {
            if (_context.Skidkas == null)
            {
                return NotFound();
            }
            var h = await _context.Skidkas.Where(e => e.DeleteAt != null).Select(s =>
                new SkidkaDTO
                {
                    IdSkidka = s.IdSkidka,
                    PriceOrder = s.PriceOrder,
                    DeleteAt = s.DeleteAt,
                    NameSkidka = s.NameSkidka

                }
            ).ToListAsync();
            return Ok(h);
        }

        [HttpGet("{id}")] //Вывод id
        public async Task<ActionResult<SkidkaDTO>> GetSkidka(int id)
        {
            if (_context.Skidkas == null)
            {
                return NotFound();
            }
            var skidka = await _context.Skidkas.FirstOrDefaultAsync(s => s.IdSkidka == id);
            if (skidka == null)
            {
                return NotFound(skidka);

            }
            var skidkaDTO = new SkidkaDTO
            {
                IdSkidka = skidka.IdSkidka,
                PriceOrder = skidka.PriceOrder,
                DeleteAt = skidka.DeleteAt,
                NameSkidka = skidka.NameSkidka
            };

            return Ok(skidkaDTO); // Возвращаем DTO
        
          }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditSkidka(int id, SkidkaDTO lentaDto)
        {
            if (_context.Skidkas == null)
            {
                return NotFound();
            }
            var lenta = _context.Skidkas.FirstOrDefault(l => l.IdSkidka == id);
            if (lenta == null)
            {
                return NotFound();
            }

            lenta.PriceOrder = lentaDto.PriceOrder;
            lenta.NameSkidka = lentaDto.NameSkidka;
            lenta.DeleteAt = lentaDto.DeleteAt;


            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost] // Добавление 
        public async Task<IActionResult> AddSkidka(SkidkaDTO user)
        {

            if (user == null)
            {
                return BadRequest("Пользователь не может быть null.");
            }

            var newUser = new Skidka
            {
                PriceOrder = user.PriceOrder,

                NameSkidka = user.NameSkidka

            };

            _context.Update(newUser);
            await _context.AddAsync(newUser);  // Используйте асинхронный метод для добавления
            await _context.SaveChangesAsync();  // Используйте асинхронный метод для сохранения изменений

            return Ok("Пользователь успешно добавлен."); // Вернуть ответ о результате
        }



        [HttpDelete("{id}")] //Удаление 
        public async Task<IActionResult> DeleteSkidka(int id)
        {
            if (_context.Skidkas == null)
            {
                return NotFound();
            }
            var toy = await _context.Skidkas.FindAsync(id);
            if (toy == null)
            {
                return NotFound(toy);

            }
            toy.DeleteAt = toy.DeleteAt;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

