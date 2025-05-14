using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Register method
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync(dto.Role))
                return BadRequest($"Role {dto.Role} does not exist.");

            await _userManager.AddToRoleAsync(user, dto.Role);

            return Ok("User registered successfully.");
        }

        // Login method
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Invalid login.");

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("supersecretkey_supersecure_jwt_32chars!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "UserService",
                audience: "UserService",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                role = roles.FirstOrDefault()
            });
        }

        // Register Farmer method - Employee Role only
        //[Authorize(Roles = "Employee")]
        [HttpPost("register/farmer")]
        public async Task<IActionResult> RegisterFarmer(RegisterFarmerDto dto)
        {
            var user = new IdentityUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "Farmer");

            return Ok($"Farmer account created for {dto.Email}");
        }

        //[Authorize(Roles = "Employee")]
        [HttpGet("farmers")]
        public async Task<IActionResult> GetFarmers()
        {
            var farmers = await _userManager.GetUsersInRoleAsync("Farmer");
            var farmerList = farmers.Select(f => new
            {
                Id = f.Id,
                Email = f.Email
                
            }).ToList();

            return Ok(farmerList);
        }

    }
}