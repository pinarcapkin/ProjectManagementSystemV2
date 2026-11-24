using ProjectManagementSystemV2.Entities;

namespace ProjectManagementSystemV2.Entities.DTOs;

public class UpdateTaskDto
{
    public string Title { get; set; } = string.Empty;              // güncellenmiş başlık
    public string? Description { get; set; }                        // güncellenmiş açıklama
    public int? AssignedToUserId { get; set; }                       // görev başka birine atanabilir
    public ProjectTaskStatus Status { get; set; }                    // durum değişebilir (Todo, InProgress, InReview, Done)
    public TaskPriority Priority { get; set; }                       // öncelik değişebilir
    public DateTime? DueDate { get; set; }                           // teslim tarihi değişebilir
    public decimal? EstimatedHours { get; set; }                     // tahmini süre güncellenebilir

    // Not: ProjectId burada YOK - çünkü bir görevin hangi projeye ait olduğu
    // güncelleme sırasında değiştirilmiyor, sadece oluşturulurken belirleniyor
}