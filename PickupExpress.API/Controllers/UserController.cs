using Microsoft.AspNetCore.Mvc;
using PickupExpress.Core.DTOs;
using PickupExpress.Core.Interfaces;
using PickupExpress.Core.Models;

namespace PickupExpress.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
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
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.ValidateUserCredentialsAsync(dto.Email, dto.Password);
            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Email or password is incorrect"
                });
            }

            return Ok(new
            {
                userId = user.UserId,
                username = user.Username,
                role = user.Role.ToString(),
                message = "Login successful"
            });


        
        }

        //Creating a new user
        
        [HttpPost]
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