using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemV2.Data;
using ProjectManagementSystemV2.Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemV2.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ProjectManagementSystemV2.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var emailVarMi = await _context.Users.AnyAsync(u => u.Email == dto.Email);

        if (emailVarMi)
        {
            return BadRequest("Bu email adresi zaten kayıtlı.");
        }

        var hashliSifre = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var yeniKullanici = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = hashliSifre
        };

        _context.Users.Add(yeniKullanici);
        await _context.SaveChangesAsync();

        var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "TeamMember");
        if (defaultRole != null)
        {
            _context.UserRoles.Add(new UserRole
            {
                UserId = yeniKullanici.Id,
                RoleId = defaultRole.Id,
                IsActive = true
            });
            await _context.SaveChangesAsync();
        }

        return Ok(new { message = "Kayıt işlemi başarıyla gerçekleştirildi!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var kullanici = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (kullanici == null)
        {
            return BadRequest(new { message = "E-posta veya şifre hatalı!" });
        }

        bool sifreDogruMu = BCrypt.Net.BCrypt.Verify(dto.Password, kullanici.PasswordHash);

        if (!sifreDogruMu)
        {
            return BadRequest(new { message = "E-posta veya şifre hatalı!" });
        }

        var userRoles = await _context.UserRoles
            .Where(ur => ur.UserId == kullanici.Id && ur.IsActive)
            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
            .ToListAsync();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
            new Claim(ClaimTypes.Email, kullanici.Email)
        };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}