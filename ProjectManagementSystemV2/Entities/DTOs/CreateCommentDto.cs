namespace ProjectManagementSystemV2.Entities.DTOs;

public class CreateCommentDto
{
    public string Content { get; set; } = string.Empty;  // yorum içeriği
    public int TaskId { get; set; }                        // hangi göreve yazılıyor
}