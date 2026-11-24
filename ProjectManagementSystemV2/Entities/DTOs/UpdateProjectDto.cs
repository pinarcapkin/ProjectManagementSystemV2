using ProjectManagementSystemV2.Entities;

namespace ProjectManagementSystemV2.Entities.DTOs;

public class UpdateProjectDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatus Status { get; set; }
}