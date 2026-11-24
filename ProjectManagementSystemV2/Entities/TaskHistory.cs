namespace ProjectManagementSystemV2.Entities;

public class TaskHistory
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public TaskItem Task { get; set; } = null!;

    public int ChangedByUserId { get; set; }
    public User ChangedByUser { get; set; } = null!;

    public string ChangeType { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}