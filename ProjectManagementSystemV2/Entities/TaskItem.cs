namespace ProjectManagementSystemV2.Entities;

public enum ProjectTaskStatus// önem dersecesini belirten
{// class'tan önce tanımlıyoruz - çünkü class içinde bu enum'u kullanacağız,
    Todo, //başlamdı
    InProgress,//çalışıyor
    InReview,// incelmede
    Done// tamamlandı
    
}
public enum TaskPriority// görevin durumunu bildiren sabit seçenek listesi
{
    Low,//Düşük
    Medium,//orta
    High,//Yüksek
    Critical//Kritik
}  
public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } =  string.Empty;// görev başlığı zorunlu
    public string? Description { get; set; } = string.Empty;// ngörev açıklaması zorunlu değil ? var 
    
    public int ProjectId { get; set; }// hangi projeye ait
    public Project Project { get; set; } = null!;//o projenin kendisine erişebileceğimiz navigation property'yi 
    
    public int? AssignedToUserId{ get; set; }// hangi proje 
    public User? AssignedToUser{ get; set; }// o projeye kim atandı
    
    public int CreatedByUserId { get; set; }// bu görevi kim oluşturdu
    public User CreatedByUser { get; set; } = null!;
    public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public DateTime? DueDate { get; set; }           // son teslim tarihi, opsiyonel
    public decimal? EstimatedHours { get; set; }      // tahmini süre, opsiyonel, ondalıklı sayı olabilir (2.5 saat gibi)

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }          // son güncelleme, opsiyonel (henüz güncellenmemiş olabilir)
    public DateTime? CompletedAt { get; set; }        // tamamlanma tarihi, opsiyonel (henüz bitmemiş olabilir)

    public bool IsDeleted { get; set; } = false;      // soft delete
}


