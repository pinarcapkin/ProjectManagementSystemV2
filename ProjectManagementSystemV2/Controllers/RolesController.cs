using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemV2.Data;

namespace ProjectManagementSystemV2.Controllers;

[Route("api/roles")]
[ApiController]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult> GetRoles()
    {
        var roles = await _context.Roles
            .AsNoTracking()
            .Where(r => r.IsActive)
            .Select(r => new { r.Id, r.Name, r.Description })
            .ToListAsync();

        return Ok(roles);
    }
}