using Diplom2.Db;
using Diplom2.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom2.Controllers
{ /*все ок работает только удаление написать и с картинками разобраться*/
    [Route("api/[controller]")]
    [ApiController]
    public class TovarController : ControllerBase
    {
        private DiplomContext _context;

        public TovarController(DiplomContext context)
        {
            _context = context;
        }

        [HttpGet] //Вывод 
        public async Task<ActionResult<IEnumerable<TovarDTO>>> GetTovar()
        {
            if (_context.Tovars == null)
            {
                return NotFound();
            }
            
            var h = await _context.Tovars.Where(e => e.DeleteAt == null).Include(s => s.IdTypeTovarNavigation).Select(s => 
                new TovarDTO
                {
                    IdTovar = s.IdTovar,
                    NameTovar = s.NameTovar,
                    PriceTovar = s.PriceTovar,
                    ImageTovar = s.ImageTovar,
                    StockTovar = s.StockTovar,
                    DeleteAt = s.DeleteAt,
                    IdTypeTovar = s.IdTypeTovar,
                    IdTypeTovarNavigation = new TypeTovarDTO
                    {
                        IdTypeTovar = s.IdTypeTovarNavigation.IdTypeTovar,
                        NameType = s.IdTypeTovarNavigation.NameType
                    }
                }    
            ).ToListAsync();
            return Ok(h);
        }

        [HttpGet("Delete")] //Вывод 
        public async Task<ActionResult<IEnumerable<TovarDTO>>> GetTovarDelete()
        {
            if (_context.Tovars == null)
            {
                return NotFound();
            }

            var h = await _context.Tovars.Where(e => e.DeleteAt != null).Include(s => s.IdTypeTovarNavigation).Select(s =>
                new TovarDTO
                {
                    IdTovar = s.IdTovar,
                    NameTovar = s.NameTovar,
                    PriceTovar = s.PriceTovar,
                    ImageTovar = s.ImageTovar,
                    StockTovar = s.StockTovar,
                    DeleteAt = s.DeleteAt,
                    IdTypeTovar = s.IdTypeTovar,
                    IdTypeTovarNavigation = new TypeTovarDTO
                    {
                        IdTypeTovar = s.IdTypeTovarNavigation.IdTypeTovar,
                        NameType = s.IdTypeTovarNavigation.NameType
                    }
                }
            ).ToListAsync();
            return Ok(h);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditTovar(int id, TovarDTO tovarDto)
        {
            if (_context.Tovars == null)
            {
                return NotFound();
            }
            var tovar = _context.Tovars.FirstOrDefault(l => l.IdTovar == id);
            if (tovar == null)
            {
                return NotFound();
            }

            tovar.IdTypeTovar = tovarDto.IdTypeTovar;
            tovar.StockTovar = tovarDto.StockTovar;
            tovar.ImageTovar = tovarDto.ImageTovar;
            tovar.PriceTovar = tovarDto.PriceTovar;
            tovar.DeleteAt = tovarDto.DeleteAt;


            tovar.NameTovar = tovarDto.NameTovar;

            if (tovarDto.PriceTovar < 0 || tovarDto.StockTovar < 0)
            {
                return BadRequest("Цена или количество не может быть отрицательным.");
            }

            _context.Update(tovar);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")] // Вывод id
        public async Task<ActionResult<TovarDTO>> GetTovar(int id)
        {
            if (_context.Tovars == null)
            {
                return NotFound();
            }

            var tovar = await _context.Tovars
                .Include(s => s.IdTypeTovarNavigation) // Включение навигационного свойства
                .FirstOrDefaultAsync(s => s.IdTovar == id); // Поиск по ID

            if (tovar == null)
            {
                return NotFound();
            }

            var tovarDTO = new TovarDTO
            {
                IdTovar = tovar.IdTovar,
                NameTovar = tovar.NameTovar,
                PriceTovar = tovar.PriceTovar,
                ImageTovar = tovar.ImageTovar,
                StockTovar = tovar.StockTovar,
                DeleteAt = tovar.DeleteAt,
                IdTypeTovar = tovar.IdTypeTovar,
                IdTypeTovarNavigation = new TypeTovarDTO
                {
                    IdTypeTovar = tovar.IdTypeTovarNavigation.IdTypeTovar,
                    NameType = tovar.IdTypeTovarNavigation.NameType
                }
            };

            return Ok(tovarDTO); // Возвращаем DTO
        }


        [HttpPost] // Добавление 
        public async Task<IActionResult> AddTovar(TovarDTO tovar)
        {
            if (tovar == null)
            {
                return BadRequest("Пользователь не может быть null.");
            }
            if (string.IsNullOrWhiteSpace(tovar.NameTovar) || tovar.PriceTovar <= 0)
            {
                return BadRequest("Имя товара не может быть пустым, а цена должна быть положительной.");
            }

            var existingTovar = await _context.Tovars
       .FirstOrDefaultAsync(t => t.NameTovar == tovar.NameTovar);

            if (existingTovar != null)
            {
                return BadRequest("Товар с таким именем уже существует.");
            }

            _context.Add(new Tovar
            {
                IdTypeTovar = tovar.IdTypeTovar,
                StockTovar = tovar.StockTovar,
                ImageTovar = tovar.ImageTovar,
                PriceTovar = tovar.PriceTovar,
                NameTovar = tovar.NameTovar,
            });
            await _context.SaveChangesAsync();

            //await _context.AddAsync(newTovar);  // Используйте асинхронный метод для добавления
            /*await _context.SaveChangesAsync();*/  // Используйте асинхронный метод для сохранения изменений

            return Ok("Пользователь успешно добавлен."); // Вернуть ответ о результате
        }



        [HttpDelete("{id}")] //Удаление 
        public async Task<IActionResult> DeleteTovar(int id)
        {
            if (_context.Tovars == null)
            {
                return NotFound();
            }
            var tovar = await _context.Tovars.FindAsync(id);
            if (tovar == null)
            {
                return NotFound(tovar);

            }

            tovar.DeleteAt = DateTime.Now;
            
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

