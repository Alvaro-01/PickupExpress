using Microsoft.AspNetCore.Mvc;
using PickupExpress.Core.DTOs;
using PickupExpress.Core.Interfaces;
using PickupExpress.Core.Models;

namespace PickupExpress.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return Ok(dto);

            var user = await _userRepository.ValidateUserCredentialsAsync(dto.Email, dto.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid credentials");
                return Ok(dto);
            }

            // Store user info in session or claims
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role.ToString());

            // Redirect based on role
            if (user.Role == UserRole.Customer)
                return RedirectToAction("Dashboard", "Customer");
            else
                return RedirectToAction("Dashboard", "Employee");

        }

        //Creating a new user
        
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] UserAddDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUserByUsername = await _userRepository.GetUserByUsernameAsync(dto.Username!);
            if (existingUserByUsername != null)
            {
                return Conflict("Username already exists");
            }

            var existingUserByEmail = await _userRepository.GetUserByEmailAsync(dto.Email!);
            if (existingUserByEmail != null)
            {
                return Conflict("Email already registered");
            }

            var newUser = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password, // In a real application, hash the password before storing
                Role = dto.Role
            };

            var createdUser = await _userRepository.CreateUserAsync(newUser);

            return CreatedAtAction(nameof(CreateUser), new
            {
                createdUser.UserId,
                createdUser.Username,
                createdUser.Email,
                createdUser.Role
            });
        }

        //For testing purposes only
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }
    }
}