namespace ProjectManagementSystemV2.Entities.DTOs;

public class CreateTimeLogDto
{
    public int TaskId { get; set; }              // hangi göreve ait
    public decimal Hours { get; set; }             // harcanan süre
    public string? Description { get; set; }        // yapılan işin kısa açıklaması
    public DateTime WorkDate { get; set; }           // çalışmanın yapıldığı tarih
}