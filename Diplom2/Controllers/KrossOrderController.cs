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
    public class KrossOrderController : ControllerBase
    {
        private DiplomContext _context;

        public KrossOrderController(DiplomContext context)
        {
            _context = context;
        }

        [HttpGet] //Вывод 
        public async Task<ActionResult<IEnumerable<KrossOrderDTO>>> GetKrossOrder()
        {
            if (_context.KrossOrders == null)
            {
                return NotFound();
            }

            var h = await _context.KrossOrders.Include(s => s.IdTovarNavigation).ThenInclude(s => s.IdTypeTovarNavigation).
                Include(s => s.IdOrderNavigation).Include( s =>s.IdBuketNavigation).Select(s =>
                new KrossOrderDTO
                {
                    IdKrossOrder = s.IdKrossOrder,
                    IdOrder = s.IdOrder,
                    IdBuket = s.IdBuket,
                    IdTovar = s.IdTovar,
                    IdOrderNavigation = s.IdOrderNavigation !=null? new OrderDTO
                    {
                        IdUser = s.IdOrderNavigation.IdUser,
                        CreatedAt = s.IdOrderNavigation.CreatedAt,
                        StatusOrder = s.IdOrderNavigation.StatusOrder,
                        PriceOrder = s.IdOrderNavigation.PriceOrder,
                        IdSkidka = s.IdOrderNavigation.IdSkidka
                    } : null,
                    IdBuketNavigation = s.IdBuketNavigation != null? new BuketDTO
                    {
                        IdBuket = s.IdBuketNavigation.IdBuket,
                        ImageBuket = s.IdBuketNavigation.ImageBuket,
                        PriceBuket = s.IdBuketNavigation.PriceBuket

                    } : null,
                    IdTovarNavigation = s.IdTovarNavigation != null? new TovarDTO
                    {
                        IdTovar = s.IdTovarNavigation.IdTovar,
                        NameTovar = s.IdTovarNavigation.NameTovar,
                        IdTypeTovar = s.IdTovarNavigation.IdTypeTovar,
                        IdTypeTovarNavigation = new TypeTovarDTO
                        {
                            IdTypeTovar = s.IdTovarNavigation.IdTypeTovarNavigation.IdTypeTovar,
                            NameType = s.IdTovarNavigation.IdTypeTovarNavigation.NameType
                        }
                    } : null

                }
            ).ToListAsync();
            return Ok(h);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditKrossOrder(int id, KrossOrderDTO KrossOrderDTO)
        {
            if (_context.KrossOrders == null)
            {
                return NotFound();
            }
            var tovar = _context.KrossOrders.FirstOrDefault(l => l.IdKrossOrder == id);
            if (tovar == null)
            {
                return NotFound();
            }

            tovar.IdOrder = KrossOrderDTO.IdOrder;
            tovar.IdBuket = KrossOrderDTO.IdBuket;
            tovar.IdTovar = KrossOrderDTO.IdTovar;
           

            _context.Update(tovar);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")] // Вывод id
        public async Task<ActionResult<KrossOrderDTO>> GetKrossOrder(int id)
        {
            if (_context.KrossOrders == null)
            {
                return NotFound();
            }

            var tovar = await _context.KrossOrders
                .Include(s => s.IdTovarNavigation).ThenInclude(s => s.IdTypeTovarNavigation).
                Include(s => s.IdOrderNavigation).Include(s => s.IdBuketNavigation) // Включение навигационного свойства
                .FirstOrDefaultAsync(s => s.IdKrossOrder == id); // Поиск по ID

            if (tovar == null)
            {
                return NotFound();
            }

            var KrossOrderDTO = new KrossOrderDTO
            {
                IdKrossOrder = tovar.IdKrossOrder,
                IdOrder = tovar.IdOrder,
                IdBuket = tovar.IdBuket,
                IdTovar = tovar.IdTovar,
                IdOrderNavigation =  new OrderDTO
                {
                    IdUser = tovar.IdOrderNavigation.IdUser,
                    CreatedAt = tovar.IdOrderNavigation.CreatedAt,
                    StatusOrder = tovar.IdOrderNavigation.StatusOrder,
                    PriceOrder = tovar.IdOrderNavigation.PriceOrder,
                    IdSkidka = tovar.IdOrderNavigation.IdSkidka
                } ,
                IdBuketNavigation =  new BuketDTO
                {
                    IdBuket = tovar.IdBuketNavigation.IdBuket,
                    ImageBuket = tovar.IdBuketNavigation.ImageBuket,
                    PriceBuket = tovar.IdBuketNavigation.PriceBuket

                },
                IdTovarNavigation =    new TovarDTO
                {
                    IdTovar = tovar.IdTovarNavigation.IdTovar,
                    NameTovar = tovar.IdTovarNavigation.NameTovar,
                    IdTypeTovar = tovar.IdTovarNavigation.IdTypeTovar,
                    IdTypeTovarNavigation = new TypeTovarDTO
                    {
                        IdTypeTovar = tovar.IdTovarNavigation.IdTypeTovarNavigation.IdTypeTovar,
                        NameType = tovar.IdTovarNavigation.IdTypeTovarNavigation.NameType
                    }
                }
            };

            return Ok(KrossOrderDTO); // Возвращаем DTO
        }


        [HttpPost] // Добавление 
        public async Task<IActionResult> AddTovar(KrossOrderDTO tovar)
        {
            if (tovar == null)
            {
                return BadRequest("Пользователь не может быть null.");
            }
            
            _context.Add(new KrossOrder
            {
                IdKrossOrder = tovar.IdKrossOrder,
                IdOrder = tovar.IdOrder,
                IdBuket = tovar.IdBuket,
                IdTovar = tovar.IdTovar,
                
            });

            
            await _context.SaveChangesAsync();

            //await _context.AddAsync(newTovar);  // Используйте асинхронный метод для добавления
            /*await _context.SaveChangesAsync();*/  // Используйте асинхронный метод для сохранения изменений

            return Ok("Пользователь успешно добавлен."); // Вернуть ответ о результате
        }



        [HttpDelete("{id}")] //Удаление 
        public async Task<IActionResult> DeleteKrossOrder(int id)
        {
            if (_context.KrossOrders == null)
            {
                return NotFound();
            }
            var tovar = await _context.KrossOrders.FindAsync(id);
            if (tovar == null)
            {
                return NotFound(tovar);

            }
            _context.KrossOrders.Remove(tovar);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
