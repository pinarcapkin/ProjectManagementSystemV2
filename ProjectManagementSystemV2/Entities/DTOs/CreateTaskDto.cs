using ProjectManagementSystemV2.Entities;

namespace ProjectManagementSystemV2.Entities.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;              // görevin başlığı, zorunlu
    public string? Description { get; set; }                        // görevin detaylı açıklaması, opsiyonel
    public int ProjectId { get; set; }                               // görevin hangi projeye ait olduğu
    public int? AssignedToUserId { get; set; }                       // görevin kime atandığı, opsiyonel (sonradan da atanabilir)
    public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Todo;  // başlangıç durumu, varsayılan Todo
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;       // önceliği, varsayılan Medium
    public DateTime? DueDate { get; set; }                           // son teslim tarihi, opsiyonel
    public decimal? EstimatedHours { get; set; }                     // tahmini tamamlanma süresi, opsiyonel
}