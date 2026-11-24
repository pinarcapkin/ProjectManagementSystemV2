namespace ProjectManagementSystemV2.Entities;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public  int TaskId  { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
   
    public TaskItem Task { get; set; } = null!;
    public User User { get; set; } = null!;
}