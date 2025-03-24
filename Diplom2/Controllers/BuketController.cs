using Diplom2.Db;
using Diplom2.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom2.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BuketController : ControllerBase
    {
        private DiplomContext _context;

        public BuketController(DiplomContext context)
        {
            _context = context;
        }

        [HttpGet] //Вывод 
        public async Task<ActionResult<IEnumerable<BuketDTO>>> GetBuket()
        {
            if (_context.Bukets == null)
            {
                return NotFound();
            }
            
            var h = await _context.Bukets.Where(e => e.DeleteAt == null).Select(s =>
                new BuketDTO
                {
                    IdBuket = s.IdBuket,
                    DeleteAt = s.DeleteAt,
                    PriceBuket = s.PriceBuket,
                    ImageBuket = s.ImageBuket
                  
                }
            ).ToListAsync();
            return Ok(h);
        }
        [Authorize]
        [HttpGet("Delete")]
        public async Task<ActionResult<IEnumerable<BuketDTO>>> GetBuketDelete()
        {
            if (_context.Bukets == null)
            {
                return NotFound();
            }

            var h = await _context.Bukets.Where(e => e.DeleteAt != null).Select(s =>
                new BuketDTO
                {
                    IdBuket = s.IdBuket,
                    DeleteAt = s.DeleteAt,
                    PriceBuket = s.PriceBuket,
                    ImageBuket = s.ImageBuket

                }
            ).ToListAsync();
            return Ok(h);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditBuket(int id, BuketDTO buketDto)
        {
            if (_context.Bukets == null)
            {
                return NotFound();
            }
            if (buketDto.PriceBuket < 0)
            {
                return BadRequest("Цена букета не может быть отрицательной.");
            }
            var tovar = _context.Bukets.FirstOrDefault(l => l.IdBuket == id);
            if (tovar == null)
            {
                return NotFound();
            }

            tovar.DeleteAt = buketDto.DeleteAt;
            tovar.PriceBuket = buketDto.PriceBuket;
            tovar.ImageBuket = buketDto.ImageBuket;
            

            _context.Update(tovar);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")] // Вывод id
        public async Task<ActionResult<BuketDTO>> GetBuket(int id)
        {
            if (_context.Bukets == null)
            {
                return NotFound();
            }

            var tovar = await _context.Bukets
 // Включение навигационного свойства
                .FirstOrDefaultAsync(s => s.IdBuket == id); // Поиск по ID

            if (tovar == null)
            {
                return NotFound();
            }

            var tovarDTO = new BuketDTO
            {
                IdBuket = tovar.IdBuket,
                DeleteAt = tovar.DeleteAt,
                PriceBuket = tovar.PriceBuket,
                ImageBuket = tovar.ImageBuket
               
            };

            return Ok(tovarDTO); // Возвращаем DTO
        }

        [Authorize]
        [HttpPost] // Добавление 
        public async Task<IActionResult> AddBuket(BuketDTO tovar)
        {
            if (tovar == null)
            {
                return BadRequest("Пользователь не может быть null.");
            }
            if (tovar.PriceBuket < 0)
            {
                return BadRequest("Цена букета не может быть отрицательной.");
            }

            _context.Add(new Buket
            {
                
                IdBuket = tovar.IdBuket,
               
                PriceBuket = tovar.PriceBuket,
                ImageBuket = tovar.ImageBuket,
                
            });

           
            await _context.SaveChangesAsync();

         

            return Ok("Пользователь успешно добавлен."); 
        }


        [Authorize]
        [HttpDelete("{id}")] //Удаление 
        public async Task<IActionResult> DeleteBuket(int id)
        {
            if (_context.Bukets == null)
            {
                return NotFound();
            }
            var tovar = await _context.Bukets.FindAsync(id);
            if (tovar == null)
            {
                return NotFound(tovar);

            }

            tovar.DeleteAt = DateTime.UtcNow;



            //var hren  = _context.Bukets.Where(s => s.IdBuket == id).ToList();


            //var hren = _context.KrossOrders.Where(s => s.IdBuket == id).ToList();
            //_context.KrossOrders.RemoveRange(hren);
            //_context.Bukets.Remove(tovar);
            //await _context.SaveChangesAsync();
            //return NoContent();
            //_context.Bukets.Remove(tovar);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

}
