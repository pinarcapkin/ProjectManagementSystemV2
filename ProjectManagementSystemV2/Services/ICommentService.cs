using ProjectManagementSystemV2.Entities;
using ProjectManagementSystemV2.Entities.DTOs;

namespace ProjectManagementSystemV2.Services;

public interface ICommentService
{
    Task<IEnumerable<Comment>> GetByTaskIdAsync(int taskId);           // bir görevin tüm yorumlarını getirir
    Task<Comment> CreateAsync(CreateCommentDto dto, int userId);       // yeni yorum ekler
    Task<bool> UpdateAsync(int id, UpdateCommentDto dto, int userId);  // yorumu günceller (sadece yazan kişi ya da Admin)
    Task<bool> DeleteAsync(int id, int userId);                       // yorumu siler (soft delete, sadece yazan kişi ya da Admin)
}