namespace ProjectManagementSystemV2.Entities;

public class UserRole
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RoleId  { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow; // rolün kullanıcıya hangi zaman da atandığı
    public int? AssignedByUserId { get; set; }//kullanıcıya kimin atadığını tutan
    public bool IsActive { get; set; } = true;

    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!;
    public User? AssignedByUser { get; set; }

}