// çalışma süresi günlüğü
namespace ProjectManagementSystemV2.Entities;

public class TaskTimeLog
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public  int UserId  { get; set; }
    public  decimal HourSpent { get; set; }// harcanan süre
    public string? Description { get; set; } // işin açıklaması
    public DateTime LogDate { get; set; } = DateTime.UtcNow; // Çalışmanın yapıldığı tarih
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   
    public TaskItem Task { get; set; } = null!;
    public User User { get; set; } = null!;
}