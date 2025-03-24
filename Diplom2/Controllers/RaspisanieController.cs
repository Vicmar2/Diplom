using Diplom2.Db;
using Diplom2.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diplom2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaspisanieController : ControllerBase
    {
        private DiplomContext _context;

        public RaspisanieController(DiplomContext context)
        {
            _context = context;
        }

        [HttpGet] //Вывод 
        public async Task<ActionResult<IEnumerable<RaspisanieDTO>>> GetRaspisanie()
        {
            if (_context.Raspisanies == null)
            {
                return NotFound();
            }

            var h = await _context.Raspisanies.Include(s => s.IdSmenaNavigation).Include(s => s.IdSotrudNavigation).Select(s =>
                new RaspisanieDTO
                {
                    IdRaspisanie = s.IdRaspisanie,
                    IdSotrud = s.IdSotrud,
                    IdSmena = s.IdSmena,

                    IdSmenaNavigation = new SmenaDTO
                    {
                        IdSmena = s.IdSmenaNavigation.IdSmena,
                        StartSmena = s.IdSmenaNavigation.StartSmena,
                        EndSmena = s.IdSmenaNavigation.EndSmena
                    },
                    IdSotrudNavigation = new SotrudDTO
                    {
                        IdSotrud = s.IdSotrudNavigation.IdSotrud,
                        NameSotrud = s.IdSotrudNavigation.NameSotrud,
                       
                    }


                }
            ).ToListAsync();
            return Ok(h);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditRaspisanie(int id, RaspisanieDTO RaspisanieDTO)
        {
            if (_context.Raspisanies == null)
            {
                return NotFound();
            }
            var tovar = _context.Raspisanies.FirstOrDefault(l => l.IdRaspisanie == id);
            if (tovar == null)
            {
                return NotFound();
            }

            tovar.IdRaspisanie = RaspisanieDTO.IdRaspisanie;
            tovar.IdSotrud = RaspisanieDTO.IdSotrud;
            tovar.IdSmena = RaspisanieDTO.IdSmena;
           


            _context.Update(tovar);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")] // Вывод id
        public async Task<ActionResult<RaspisanieDTO>> GetRaspisanie(int id)
        {
            if (_context.Raspisanies == null)
            {
                return NotFound();
            }

            var tovar = await _context.Raspisanies.Include(s => s.IdSmenaNavigation).Include(s => s.IdSotrudNavigation)
                // Включение навигационного свойства
                .FirstOrDefaultAsync(s => s.IdRaspisanie == id); // Поиск по ID

            if (tovar == null)
            {
                return NotFound();
            }

            var tovarDTO = new RaspisanieDTO
            {
                IdRaspisanie = tovar.IdRaspisanie,
                IdSotrud = tovar.IdSotrud,
                IdSmena = tovar.IdSmena,

                IdSmenaNavigation = new SmenaDTO
                {
                    IdSmena = tovar.IdSmenaNavigation.IdSmena,
                    StartSmena = tovar.IdSmenaNavigation.StartSmena,
                    EndSmena = tovar.IdSmenaNavigation.EndSmena
                },
                IdSotrudNavigation = new SotrudDTO
                {
                    IdSotrud = tovar.IdSotrudNavigation.IdSotrud,
                    NameSotrud = tovar.IdSotrudNavigation.NameSotrud,

                }



            };

            return Ok(tovarDTO); // Возвращаем DTO
        }


        [HttpPost] // Добавление 
        public async Task<IActionResult> AddRaspisanie(RaspisanieDTO tovar)
        {
            if (tovar == null)
            {
                return BadRequest("Пользователь не может быть null.");
            }

            _context.Add(new Raspisanie
            {

               IdRaspisanie = tovar.IdRaspisanie,
               IdSotrud = tovar.IdSotrud,
               IdSmena = tovar.IdSmena,
              

            });
            await _context.SaveChangesAsync();

            //await _context.AddAsync(newTovar);  // Используйте асинхронный метод для добавления
            /*await _context.SaveChangesAsync();*/  // Используйте асинхронный метод для сохранения изменений

            return Ok("Пользователь успешно добавлен."); // Вернуть ответ о результате
        }



        [HttpDelete("{id}")] //Удаление 
        public async Task<IActionResult> DeleteRaspisanie(int id)
        {
            if (_context.Raspisanies == null)
            {
                return NotFound();
            }
            var tovar = await _context.Raspisanies.FindAsync(id);
            if (tovar == null)
            {
                return NotFound(tovar);

            }
            _context.Raspisanies.Remove(tovar);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

