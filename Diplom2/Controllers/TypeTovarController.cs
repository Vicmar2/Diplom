using Diplom2.Db;
using Diplom2.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeTovarController : ControllerBase
    {
        private DiplomContext _context;

        public TypeTovarController(DiplomContext context)
        {
            _context = context;
        }

        [HttpGet] //Вывод 
        public async Task<ActionResult<IEnumerable<TypeTovarDTO>>> GetType()
        {
            if (_context.TypeTovars == null)
            {
                return NotFound();
            }
           var type = await _context.TypeTovars.Select(s =>
                new TypeTovarDTO
                {
                    IdTypeTovar = s.IdTypeTovar,
                    NameType = s.NameType,
                   
                }
            ).ToListAsync();
            return Ok(type);
        }

        [HttpGet("{id}")] //Вывод id
        public async Task<ActionResult<TypeTovarDTO>> GetTypeTovar(int id)
        {
            if (_context.TypeTovars == null)
            {
                return NotFound();
            }
            var type = await _context.TypeTovars.FirstOrDefaultAsync(s => s.IdTypeTovar == id);
            if (type == null)
            {
                return NotFound(type);

            }
            var typeDTO = new TypeTovarDTO
            {
                IdTypeTovar = type.IdTypeTovar,
                NameType = type.NameType,
            };
            return Ok(typeDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditTypeTovar(int id, TypeTovarDTO lentaDto)
        {
            if (_context.TypeTovars == null)
            {
                return NotFound();
            }
            var lenta = _context.TypeTovars.FirstOrDefault(l => l.IdTypeTovar == id);
            if (lenta == null)
            {
                return NotFound();
            }

            
            lenta.NameType = lentaDto.NameType;

            _context.Update(lenta);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost] // Добавление 
        public async Task<IActionResult> AddTypeTovar(TypeTovarDTO user)
        {

            if (user == null)
            {
                return BadRequest("Пользователь не может быть null.");
            }
            if (string.IsNullOrWhiteSpace(user.NameType) )
            {
                return BadRequest("Имя товара не может быть пустым");
            }
            var newUser = new TypeTovar
            {
                

                NameType = user.NameType

            };

            await _context.AddAsync(newUser);  // Используйте асинхронный метод для добавления
            await _context.SaveChangesAsync();  // Используйте асинхронный метод для сохранения изменений

            return Ok("Пользователь успешно добавлен."); // Вернуть ответ о результате
        }



        //[HttpDelete("{id}")] //Удаление 
        //public async Task<IActionResult> DeleteTypeTovar(int id)
        //{
        //    if (_context.TypeTovars == null)
        //    {
        //        return NotFound();
        //    }
        //    var toy = await _context.TypeTovars.FindAsync(id);
        //    if (toy == null)
        //    {
        //        return NotFound(toy);

        //    }
        //    _context.TypeTovars.Remove(toy);
        //    await _context.SaveChangesAsync();
        //    return NoContent();
        //}
    }
}

