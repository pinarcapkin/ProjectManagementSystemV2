using ProjectManagementSystemV2.Entities;
using ProjectManagementSystemV2.Entities.DTOs;

namespace ProjectManagementSystemV2.Services;

public interface IProjectService
{
    Task<(IEnumerable<Project> Items, int TotalCount)> GetAllAsync(
        int page,
        int pageSize,
        ProjectStatus? status,
        int? currentUserId,
        bool isAdmin,
        string? sortBy,
        string? sortDirection);
    
    Task<Project?> GetByIdAsync(int id);
    Task<Project> CreateAsync(CreateProjectDto dto);
    Task<bool> UpdateAsync(int id, UpdateProjectDto dto);
    Task<bool> ArchiveAsync(int id);
    Task<bool> AddMemberAsync(int projectId, AddProjectMemberDto dto);
    Task<bool> RemoveMemberAsync(int projectId, int userId);
}