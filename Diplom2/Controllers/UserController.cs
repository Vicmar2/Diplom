using Diplom2.Db;
using Diplom2.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Diplom2.Controllers
 {
    /// <summary>
    /// /все ок удаление прописать
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        Mail mail = new Mail();
        private readonly VremUser _tempUserStorage;
        /// <summary>
        ///  работает надо будет удаление доработать, чтобы с пользователем удалялись и его заказы
        /// </summary>
        private DiplomContext _context;

        public UserController(DiplomContext context, VremUser tempUserStorage)
        {
            _context = context;
            _tempUserStorage = tempUserStorage;
        }

   
        [HttpGet] //Вывод 
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUser()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var h = await _context.Users.Where(e => e.DeleteAt == null).Select(s =>
               new UserDTO
               {
                   IdUser = s.IdUser,
                   NameUser = s.NameUser,
                   EmailUser = s.EmailUser,
                   ParolUser = s.ParolUser,
                   NumberUser = s.NumberUser,
                   DeleteAt = s.DeleteAt,

               }
           ).ToListAsync();
            return Ok(h);
        }

        [HttpGet("Delete")] //Вывод 
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUserDelete()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var h = await _context.Users.Where(e => e.DeleteAt != null).Select(s =>
               new UserDTO
               {
                   IdUser = s.IdUser,
                   NameUser = s.NameUser,
                   EmailUser = s.EmailUser,
                   ParolUser = s.ParolUser,
                   NumberUser = s.NumberUser,
                   DeleteAt = s.DeleteAt,

               }
           ).ToListAsync();
            return Ok(h);
        }




        [HttpGet("{id}")] //Вывод id
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(s => s.IdUser == id);
            if (user == null)
            {
                return NotFound(user);

            }

            var userDTO = new UserDTO
            {
                IdUser = user.IdUser,
                NameUser = user.NameUser,
                EmailUser = user.EmailUser,
                ParolUser = user.ParolUser,
                NumberUser = user.NumberUser,
                DeleteAt = user.DeleteAt,
                
            };

            return user;
        }


        [HttpPost] // Добавление 
        public async Task<IActionResult> RegisterUser([FromBody] RegistUser registuser)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }




            //var ProverkaUser = await _context.Users.
            //       FirstOrDefaultAsync(u => u.EmailUser == registuser.EmailUser);
            //if (ProverkaUser != null)
            //{
            //    return BadRequest("Пользователь с такой почтой уже существует");
            //}


            string hashedPassword = await HashPasswordAsync(registuser.ParolUser);

            var newuser = new RegistUser

            {
                IdUser = registuser.IdUser,
                NameUser = registuser.NameUser,
                EmailUser = registuser.EmailUser,
                ParolUser= hashedPassword,
                NumberUser = registuser.NumberUser,
            };

         

            await mail.Send(newuser);
            await _context.SaveChangesAsync();




            _tempUserStorage.Add(newuser);
            _context.SaveChanges();
            return Ok(newuser);



        }


        

        [HttpPost("vashkod")] 
        public async Task<IActionResult> VerifyCode(VerifyCodeRequest request)
        {

            
            var proverkaUser = _tempUserStorage.GetByEmail(request.EmailUser);
            if (proverkaUser == null)
            {
                return NotFound("Пользователь не найден");
            }

          
            if (proverkaUser.kod != request.Code)
            {
                return BadRequest("Неверный код подтверждения");
            }

           
            var newUser = new User 
            {
                NameUser = proverkaUser.NameUser,
                EmailUser = proverkaUser.EmailUser,
                ParolUser = proverkaUser.ParolUser,
                NumberUser = proverkaUser.NumberUser,
            };

           
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();


            _tempUserStorage.Remove(proverkaUser);
            await _context.SaveChangesAsync();

            return Ok(newUser);
        }

        public class VerifyCodeRequest
        {
            public string EmailUser { get; set; }
            public string Code { get; set; }
        }

        //[HttpPost("VerifyCode")]
        //public IActionResult VerifyCode([FromBody] RegistUser user)
        //{
        //    var vashkod = new RegistUser
        //    {
        //        kod = user.kod,
        //    };
        //    Ваша логика проверки кода здесь
        //    if (vashkod.kod == user.kod) // Пример проверки кода
        //    {



        //        var newuser = new User

        //        {
        //            NameUser = user.NameUser,
        //            EmailUser = user.EmailUser,
        //            ParolUser = hashedPassword,
        //            NumberUser = user.NumberUser,
        //        };
        //        return Ok(new { message = "Код подтвержден" });

        //    }
        //    else
        //    {
        //        return BadRequest(new { error = "Неверный код" });
        //    }





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
        //private string GenerateRandomPassword()
        //{
        //    string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        //    var random = new Random();
        //    var password = new string(
        //        Enumerable.Repeat(chars, 8)
        //                  .Select(s => s[random.Next(s.Length)])
        //                  .ToArray());
        //    return password;
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(int id, UserDTO userDto)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = _context.Users.FirstOrDefault(l => l.IdUser == id);
            if (user == null)
            {
                return NotFound();
            }

            user.NameUser = userDto.NameUser;
            user.EmailUser = userDto.EmailUser;
            user.ParolUser = userDto.ParolUser;
            user.NumberUser = userDto.NumberUser;
            user.DeleteAt = userDto.DeleteAt;

            _context.Update(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")] //Удаление 
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var toy = await _context.Users.FindAsync(id);
            if (toy == null)
            {
                return NotFound(toy);

            }

            toy.DeleteAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

