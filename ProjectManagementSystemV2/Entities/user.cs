namespace ProjectManagementSystemV2.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? Department { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // yeni kullanıcı girildiğinde güncel tarih ve saat girilmesi
        public bool IsActive { get; set; } = true; // IsActive = aktif mi? bool sadece 2 değer alır
        public bool IsDeleted { get; set; } = false; // silinmiş mi?
    }
}