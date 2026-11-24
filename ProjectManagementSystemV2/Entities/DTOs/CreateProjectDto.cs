using ProjectManagementSystemV2.Entities;

namespace ProjectManagementSystemV2.Entities.DTOs;

public class CreateProjectDto
{
    public string Title { get; set; } = string.Empty;//proje isimi
    public string? Description { get; set; }//o projenin ne ile ilgili olduğuna dair açıklama
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Planning;
    public int UserId { get; set; }
}