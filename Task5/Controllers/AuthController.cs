using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext db;
    private readonly IConfiguration config;

    public AuthController(AppDbContext db, IConfiguration config)
    {
        this.db = db;
        this.config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest(new { error = "Username and password are required." });
        }

        if (dto.Role != "admin" && dto.Role != "staff")
        {
            return BadRequest(new { error = "Role must be 'admin' or 'staff'." });
        }

        bool exists = await db.Admins.AnyAsync(a => a.Username == dto.Username);
        if (exists)
        {
            return Conflict(new { error = "That username is already taken." });
        }

        Admin admin = new Admin
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role
        };

        db.Admins.Add(admin);
        await db.SaveChangesAsync();

        return StatusCode(201, new { admin.Id, admin.Username, admin.Role });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        Admin? admin = await db.Admins.FirstOrDefaultAsync(a => a.Username == dto.Username);

        if (admin == null || !BCrypt.Net.BCrypt.Verify(dto.Password, admin.PasswordHash))
        {
            return Unauthorized(new { error = "Invalid username or password." });
        }

        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("sub", admin.Id),
                new Claim("unique_name", admin.Username),
                new Claim("role", admin.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = config["Jwt:Issuer"],
            Audience = config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)),
                SecurityAlgorithms.HmacSha256)
        };

        string token = new JsonWebTokenHandler().CreateToken(descriptor);
        return Ok(new { token });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        string? id = User.FindFirst("sub")?.Value;
        if (id == null)
        {
            return Unauthorized();
        }

        Admin? admin = await db.Admins.FindAsync(id);
        if (admin == null)
        {
            return NotFound(new { error = "User not found." });
        }

        return Ok(new { admin.Id, admin.Username, admin.Role, admin.CreatedAt });
    }
}