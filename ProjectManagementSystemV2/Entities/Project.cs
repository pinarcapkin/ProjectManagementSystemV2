namespace ProjectManagementSystemV2.Entities;

public enum ProjectStatus//enum, sadece belirli, önceden tanımlı değerlerin girilebilmesini garanti ediyor
{
    Planning,//Planlama aşamasında=0
    Active,//Aktif, devam ediyor=1
    OnHold,//Beklemede=2
    Completed,//Tamamlanmış=3
    Cancelled//İptal edilmiş=4
}

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Planning;
    public bool IsActive { get; set; } = true;
    public bool IsArchived { get; set; } = false;
    public DateTime? ArchivedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}