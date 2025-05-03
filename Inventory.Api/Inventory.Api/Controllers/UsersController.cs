using Inventory.BL.Entities;
using Inventory.DAL.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using global::Inventory.BL.DTOs.UserDTOS;
using Inventory.BL.DTOs;

namespace Inventory.Api.Controllers
{
    

    namespace Inventory.Api.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class UsersController : ControllerBase
        {
            private readonly AppDbContext _context;

            public UsersController(AppDbContext context)
            {
                _context = context;
            }

            [HttpPost]
            public async Task<IActionResult> AddUser([FromBody] CreateUserDto createUserDto)
            {
                if (createUserDto == null)
                    return BadRequest("User data is required.");

                if (await _context.Users.AnyAsync(u => u.Email == createUserDto.Email || u.Username == createUserDto.Username))
                    return Conflict("Username or email is already in use.");

                var user = new User
                {
                    Username = createUserDto.Username,
                    Role = createUserDto.Role,
                    Email = createUserDto.Email,
                    Password = createUserDto.Password
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                var userDto = new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Role = user.Role,
                    Email = user.Email
                };

                return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, userDto);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetUser(int id)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                if (user == null)
                    return NotFound();

                var userDto = new UserDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Role = user.Role,
                    Email = user.Email
                };

                return Ok(userDto);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto updateUserDto)
            {
                if (id != updateUserDto.UserId)
                    return BadRequest();

                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound();

                user.Username = updateUserDto.Username;
                user.Role = updateUserDto.Role;
                user.Email = updateUserDto.Email;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteUser(int id)
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound();

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }


}
