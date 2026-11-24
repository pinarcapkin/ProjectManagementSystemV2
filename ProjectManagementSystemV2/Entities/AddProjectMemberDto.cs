using ProjectManagementSystemV2.Entities;

namespace ProjectManagementSystemV2.Entities.DTOs;

public class AddProjectMemberDto
{
    public int UserId { get; set; }
    public ProjectMemberRole Role { get; set; } = ProjectMemberRole.Member;
}