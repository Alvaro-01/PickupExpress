using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PickupExpress.Core.DTOs;
using PickupExpress.Core.Interfaces;
using PickupExpress.Core.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace PickupExpress.API.Controllers
{

[Route("api/[controller]")]
[ApiController]
[Authorize]   // applies to all actions by default
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    [HttpPost("login")]
    [AllowAnonymous]   
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userRepository.ValidateUserCredentialsAsync(dto.Email, dto.Password);
        if (user == null)
            return Unauthorized(new { message = "Email or password is incorrect" });

        // Generate JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        var claims = new List<Claim>
        {
            new Claim("name", user.Username),
            new Claim("email", user.Email),
            new Claim("role", user.Role.ToString()),
            new Claim("userId", user.UserId.ToString())
        };


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"])),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new
        {
            userId = user.UserId,
            username = user.Username,
            role = user.Role.ToString(),
            token = tokenString,
            message = "Login successful"
        });
    }



        //Creating a new user
        
        [HttpPost]
        [AllowAnonymous]
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