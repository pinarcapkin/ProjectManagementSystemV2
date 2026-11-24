using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemV2.Data;
using ProjectManagementSystemV2.Entities;
using ProjectManagementSystemV2.Entities.DTOs;

namespace ProjectManagementSystemV2.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/users - giriş yapmış kullanıcılar ekip üyesi seçimi için kullanıcı listesini görebilir
    [HttpGet]
    public async Task<ActionResult> GetUsers()
    {
        bool isAdmin = User.IsInRole("Admin");

        var query = _context.Users
            .AsNoTracking()
            .Where(u => !u.IsDeleted && u.IsActive);

        var users = await query
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                IsActive = isAdmin ? u.IsActive : (bool?)null,
                CreatedAt = isAdmin ? u.CreatedAt : (DateTime?)null
            })
            .ToListAsync();

        return Ok(users);
    }

    // POST: api/users/{userId:int}/roles - kullanıcıya rol atar
    [HttpPost("{userId:int}/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> AssignRole(int userId, AssignRoleDto dto)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId && !u.IsDeleted);
        if (!userExists)
        {
            return NotFound($"Id {userId} ile eşleşen kullanıcı bulunamadı.");
        }

        var roleExists = await _context.Roles.AnyAsync(r => r.Id == dto.RoleId && r.IsActive);
        if (!roleExists)
        {
            return NotFound($"Id {dto.RoleId} ile eşleşen aktif rol bulunamadı.");
        }

        var alreadyAssigned = await _context.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == dto.RoleId && ur.IsActive);
        if (alreadyAssigned)
        {
            return BadRequest("Bu rol zaten kullanıcıya atanmış.");
        }

        var userRole = new UserRole
        {
            UserId = userId,
            RoleId = dto.RoleId,
            AssignedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Rol başarıyla atandı." });
    }

    // DELETE: api/users/{userId:int}/roles/{roleId:int} - kullanıcının rolünü kaldırır (soft delete)
    [HttpDelete("{userId:int}/roles/{roleId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> RemoveRole(int userId, int roleId)
    {
        var userRole = await _context.UserRoles
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId && ur.IsActive);

        if (userRole == null)
        {
            return NotFound("Bu kullanıcıya atanmış böyle bir rol bulunamadı.");
        }

        userRole.IsActive = false;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Rol başarıyla kaldırıldı." });
    }

    // PATCH: api/users/{id:int}/deactivate - kullanıcıyı pasif hale getirir
    [HttpPatch("{id:int}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeactivateUser(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        if (user == null)
        {
            return NotFound($"Id {id} ile eşleşen kullanıcı bulunamadı.");
        }

        user.IsActive = false;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Kullanıcı pasif hale getirildi." });
    }

    // PATCH: api/users/{id:int}/activate - kullanıcıyı tekrar aktif hale getirir
    [HttpPatch("{id:int}/activate")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> ActivateUser(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        if (user == null)
        {
            return NotFound($"Id {id} ile eşleşen kullanıcı bulunamadı.");
        }

        user.IsActive = true;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Kullanıcı tekrar aktif hale getirildi." });
    }
}