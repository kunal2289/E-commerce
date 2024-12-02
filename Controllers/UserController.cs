using E_commerce.Data;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            // Use LINQ to include filtering or manipulation as needed
            return await _context.Users
                .OrderBy(u => u.name)  // Example: Sort users by name
                .ToListAsync();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserById(string userId)  // Change to string for userId
        {
            // Use LINQ to find a user by their ID (userId is now string)
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.userId == userId);  // Compare userId as a string

            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            return user;
        }

        // GET: api/User/Search?name=John
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<User>>> SearchUsers(string name)
        {
            // Use LINQ to search users by name
            var users = await _context.Users
                .Where(u => u.name.Contains(name))
                .ToListAsync();

            if (!users.Any())
            {
                return NotFound($"No users found matching the name '{name}'.");
            }

            return users;
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            // Add a new user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Return the created user
            return CreatedAtAction(nameof(GetUserById), new { userId = user.userId }, user);
        }
    }
}
