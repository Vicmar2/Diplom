using Diplom2.Db;
using Diplom2.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private DiplomContext _context;

        public OrderController(DiplomContext context)
        {
            _context = context;
        }

        [HttpGet] //Вывод 
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrder()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            var h = await _context.Orders.Where(e => e.DeleteAt == null).Include(s => s.IdSkidkaNavigation).Include(s => s.StatusOrderNavigation).Include( s => s.IdUserNavigation).Select(s =>
                new OrderDTO
                {
                    IdOrder = s.IdOrder,
                    IdUser = s.IdUser,
                    CreatedAt = s.CreatedAt,
                    StatusOrder = s.StatusOrder,
                    PriceOrder = s.PriceOrder,
                    IdSkidka = s.IdSkidka,
                    DeleteAt = s.DeleteAt,
                    IdSkidkaNavigation = s.IdSkidkaNavigation != null ? new  SkidkaDTO
                    {
                        IdSkidka = s.IdSkidkaNavigation.IdSkidka,
                        NameSkidka = s.IdSkidkaNavigation.NameSkidka
                    } : null,
                    IdUserNavigation = s.IdUserNavigation != null ? new UserDTO
                    {
                        IdUser = s.IdUserNavigation.IdUser,
                        NameUser = s.IdUserNavigation.NameUser,
                        NumberUser = s.IdUserNavigation.NumberUser
                    } : null,
                    StatusOrderNavigation = s.StatusOrderNavigation != null ? new StatusDTO
                    {
                        IdStatusOrder = s.StatusOrderNavigation.IdStatusOrder,
                        NameStatus = s.StatusOrderNavigation.NameStatus
                    } : null


                }
            ).ToListAsync();
            return Ok(h);
        }

        [HttpGet] //Вывод 
        public async Task<ActionResult<IEnumerable<StatusDTO>>> GetStatus()
        {
            if (_context.Statuses == null)
            {
                return NotFound();
            }
            var type = await _context.Statuses.Select(s =>
                 new StatusDTO
                 {
                     IdStatusOrder = s.IdStatusOrder,
                     NameStatus = s.NameStatus,

                 }
             ).ToListAsync();
            return Ok(type);
        }

        [HttpGet("Delete")] //Вывод 
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrderDelete()
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            var h = await _context.Orders.Where(e => e.DeleteAt != null).Include(s => s.IdSkidkaNavigation).Include(s => s.IdUserNavigation).Select(s =>
                new OrderDTO
                {
                    IdOrder = s.IdOrder,
                    IdUser = s.IdUser,
                    CreatedAt = s.CreatedAt,
                    StatusOrder = s.StatusOrder,
                    PriceOrder = s.PriceOrder,
                    IdSkidka = s.IdSkidka,
                    DeleteAt = s.DeleteAt,
                    IdSkidkaNavigation = s.IdSkidkaNavigation != null ? new SkidkaDTO
                    {
                        IdSkidka = s.IdSkidkaNavigation.IdSkidka,
                        NameSkidka = s.IdSkidkaNavigation.NameSkidka
                    } : null,
                    IdUserNavigation = s.IdUserNavigation != null ? new UserDTO
                    {
                        IdUser = s.IdUserNavigation.IdUser,
                        NameUser = s.IdUserNavigation.NameUser,
                        NumberUser = s.IdUserNavigation.NumberUser
                    } : null


                }
            ).ToListAsync();
            return Ok(h);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditOrder(int id, OrderDTO OrderDTO)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var tovar = _context.Orders.FirstOrDefault(l => l.IdOrder == id);
            if (tovar == null)
            {
                return NotFound();
            }

            tovar.IdOrder = OrderDTO.IdOrder;
            tovar.IdUser = OrderDTO.IdUser;
            tovar.CreatedAt = OrderDTO.CreatedAt;
            tovar.StatusOrder = OrderDTO.StatusOrder;
            tovar.PriceOrder = OrderDTO.PriceOrder;
            tovar.IdSkidka = OrderDTO.IdSkidka;
            tovar.DeleteAt = OrderDTO.DeleteAt;


            _context.Update(tovar);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")] // Вывод id
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }

            var tovar = await _context.Orders.Include(s => s.IdSkidkaNavigation).Include(s => s.IdUserNavigation)
                // Включение навигационного свойства
                .FirstOrDefaultAsync(s => s.IdOrder == id); // Поиск по ID

            if (tovar == null)
            {
                return NotFound();
            }

            var tovarDTO = new OrderDTO
            {
                IdOrder = tovar.IdOrder,
                IdUser = tovar.IdUser,
                CreatedAt = tovar.CreatedAt,
                StatusOrder = tovar.StatusOrder,
                PriceOrder = tovar.PriceOrder,
                DeleteAt = tovar.DeleteAt,
                IdSkidka = tovar.IdSkidka,
                IdSkidkaNavigation = new SkidkaDTO
                {
                    IdSkidka = tovar.IdSkidkaNavigation.IdSkidka,
                    NameSkidka = tovar.IdSkidkaNavigation.NameSkidka
                },
                IdUserNavigation = new UserDTO
                {
                    IdUser = tovar.IdUserNavigation.IdUser,
                    NameUser = tovar.IdUserNavigation.NameUser,
                    NumberUser = tovar.IdUserNavigation.NumberUser
                }



            };

            return Ok(tovarDTO); // Возвращаем DTO
        }


        [HttpPost] // Добавление 
        public async Task<IActionResult> AddOrder(OrderDTO tovar)
        {
            if (tovar == null)
            {
                return BadRequest("Пользователь не может быть null.");
            }

            _context.Add(new Order
            {

                IdOrder = tovar.IdOrder,
                IdUser = tovar.IdUser,
                CreatedAt = tovar.CreatedAt,
                StatusOrder = tovar.StatusOrder,
                PriceOrder = tovar.PriceOrder,
                IdSkidka = tovar.IdSkidka,

            });
            await _context.SaveChangesAsync();

            //await _context.AddAsync(newTovar);  // Используйте асинхронный метод для добавления
            /*await _context.SaveChangesAsync();*/  // Используйте асинхронный метод для сохранения изменений

            return Ok("Пользователь успешно добавлен."); // Вернуть ответ о результате
        }



        [HttpDelete("{id}")] //Удаление 
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var tovar = await _context.Orders.FindAsync(id);
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

