using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemV2.Entities;
using ProjectManagementSystemV2.Entities.DTOs;
using ProjectManagementSystemV2.Services;

namespace ProjectManagementSystemV2.Controllers;

[Route("api/projects")]
[ApiController]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // Admin ise tüm projeleri; normal kullanıcı ise sahibi veya aktif üyesi olduğu projeleri listeler
    [HttpGet]
    public async Task<ActionResult> GetProjects(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] ProjectStatus? status = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] string? sortDirection = null)
    {
        var kullaniciIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(kullaniciIdStr, out var currentUserId))
        {
            return Unauthorized("Geçerli kullanıcı kimliği bulunamadı.");
        }

        var role = User.FindFirstValue(ClaimTypes.Role);
        bool isAdmin = User.IsInRole("Admin") ||
                       string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);

        if (pageSize > 50) pageSize = 50;
        if (page < 1) page = 1;

        var (items, totalCount) = await _projectService.GetAllAsync(
            page, pageSize, status, currentUserId, isAdmin, sortBy, sortDirection);

        return Ok(new { totalCount, page, pageSize, items });
    }

    // Tek proje detayı
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetProject(int id)
    {
        var project = await _projectService.GetByIdAsync(id);
        if (project == null)
        {
            return NotFound($"Id {id} ile eşleşen proje bulunamadı.");
        }

        return Ok(project);
    }

    // Yeni proje oluşturur
    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectDto dto)
    {
        var kullaniciIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(kullaniciIdStr, out var userId))
        {
            return Unauthorized("Geçerli kullanıcı kimliği bulunamadı.");
        }

        dto.UserId = userId;

        var project = await _projectService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }

    // Proje günceller
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProject(int id, UpdateProjectDto dto)
    {
        var success = await _projectService.UpdateAsync(id, dto);
        if (!success)
        {
            return NotFound($"Id {id} ile eşleşen proje bulunamadı.");
        }

        return Ok(new { message = "Proje güncellendi." });
    }

    // Projeyi arşivler (silmez)
    [HttpPatch("{id:int}/archive")]
    public async Task<ActionResult> ArchiveProject(int id)
    {
        var success = await _projectService.ArchiveAsync(id);
        if (!success)
        {
            return NotFound($"Id {id} ile eşleşen proje bulunamadı.");
        }

        return Ok(new { message = "Proje arşivlendi." });
    }

    // Projeye ekip üyesi ekler
    [HttpPost("{projectId:int}/members")]
    public async Task<ActionResult> AddMember(int projectId, AddProjectMemberDto dto)
    {
        var success = await _projectService.AddMemberAsync(projectId, dto);
        if (!success)
        {
            return BadRequest("Üye eklenemedi (proje bulunamadı ya da kullanıcı zaten üye).");
        }

        return Ok(new { message = "Ekip üyesi eklendi." });
    }

    // Projeden ekip üyesi çıkarır
    [HttpDelete("{projectId:int}/members/{userId:int}")]
    public async Task<ActionResult> RemoveMember(int projectId, int userId)
    {
        var success = await _projectService.RemoveMemberAsync(projectId, userId);
        if (!success)
        {
            return NotFound("Bu projede aktif böyle bir üyelik bulunamadı.");
        }

        return Ok(new { message = "Ekip üyesi çıkarıldı." });
    }
}