using Diplom2.Db;
using Diplom2.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class KrossBuketController : ControllerBase
    {
        private DiplomContext _context;

        public KrossBuketController(DiplomContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KrossBuketDTO>>> GetKrossBukets()
        {
            var h = await _context.KrossBukets.Include(s => s.IdBuketNavigation).Include(s => s.IdTovarNavigation).Select(s =>
               new KrossBuketDTO
               {
                   IdKrossBuket = s.IdKrossBuket,
                   IdTovar = s.IdTovar,
                   IdBuket = s.IdBuket,

                   IdBuketNavigation = s.IdBuketNavigation != null? new BuketDTO
                   {
                       IdBuket = s.IdBuketNavigation.IdBuket,
                       ImageBuket = s.IdBuketNavigation.ImageBuket
                   } : null,
                   IdTovarNavigation = s.IdTovarNavigation != null? new TovarDTO
                   {
                       NameTovar = s.IdTovarNavigation.NameTovar,
                       PriceTovar = s.IdTovarNavigation.PriceTovar,
                       ImageTovar = s.IdTovarNavigation.ImageTovar,

                   }: null
               }
           ).ToListAsync();
            return Ok(h);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<KrossBuketDTO>> GetKrossBuket(int id)
        {
            var krossBuket = await _context.KrossBukets
                .Include(kb => kb.IdBuketNavigation)
                .Include(kb => kb.IdTovarNavigation)

                .FirstOrDefaultAsync(kb => kb.IdKrossBuket == id);

            if (krossBuket == null)
            {
                return NotFound();
            }
            var krossbuketDTO = new KrossBuketDTO
            {
                IdKrossBuket = krossBuket.IdKrossBuket,
                IdTovar = krossBuket.IdTovar,
                IdBuket = krossBuket.IdBuket,

                IdBuketNavigation = new BuketDTO
                {
                    IdBuket = krossBuket.IdBuketNavigation.IdBuket,
                    ImageBuket = krossBuket.IdBuketNavigation.ImageBuket
                },
                IdTovarNavigation = new TovarDTO
                {
                    NameTovar = krossBuket.IdTovarNavigation.NameTovar,
                    PriceTovar = krossBuket.IdTovarNavigation.PriceTovar,
                    ImageTovar = krossBuket.IdTovarNavigation.ImageTovar,

                }
            };

            return Ok(krossbuketDTO);
        }

        // POST: api/KrossBuket
        [HttpPost] // Добавление
        public async Task<IActionResult> AddKrossBuket(KrossBuketDTO krossBuketDto)
        {
            if (krossBuketDto == null)
            {
                return BadRequest("Кросс-букет не может быть null.");
            }
           
            var tovar = await _context.Tovars.FindAsync(krossBuketDto.IdTovar);
            if (tovar == null)
            {
                return NotFound("Товар не найден.");
            }
            if (tovar.StockTovar <= 0)
            {
                return BadRequest("Товар закончился.");
            }

            if (tovar.DeleteAt != null)
            {
                return BadRequest("Товар удален.");
            }


            var newKrossBuket = new KrossBuket
            {
                IdBuket = krossBuketDto.IdBuket,
                IdTovar = krossBuketDto.IdTovar,

            };

            tovar.StockTovar--;
            if (tovar.StockTovar >= 0)
            {
                tovar.DeleteAt = DateTime.Now;
            }

            await _context.KrossBukets.AddAsync(newKrossBuket); // Используйте асинхронный метод для добавления
            await _context.SaveChangesAsync(); // Используйте асинхронный метод для сохранения изменений

            return Ok("Кросс-букет успешно добавлен."); // Вернуть ответ о результате
        }

        // PUT: api/KrossBuket/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKrossBuket(int id, KrossBuket krossBuket)
        {
            if (_context.KrossBukets == null)
            {
                return NotFound();
            }
            var tovar = _context.KrossBukets.FirstOrDefault(l => l.IdKrossBuket == id);
            if (tovar == null)
            {
                return NotFound();
            }


            tovar.IdBuket = krossBuket.IdBuket;
            tovar.IdTovar = krossBuket.IdTovar;


            _context.Update(tovar);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/KrossBuket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKrossBuket(int id)
        {
            var krossBuket = await _context.KrossBukets.FindAsync(id);
            if (krossBuket == null)
            {
                return NotFound();
            }

            _context.KrossBukets.Remove(krossBuket);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}



