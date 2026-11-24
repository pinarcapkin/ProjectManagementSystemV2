using ProjectManagementSystemV2.Entities;
using ProjectManagementSystemV2.Entities.DTOs;

namespace ProjectManagementSystemV2.Services;

public interface ITaskService
{
    Task<(IEnumerable<TaskItem> Items, int TotalCount)> GetAllAsync(int page, int pageSize, int? projectId, ProjectTaskStatus? status, TaskPriority? priority, int? assignedToUserId);  // filtreli, sayfalı görev listesi
    Task<TaskItem?> GetByIdAsync(int id);                          // tek görev getir
    Task<TaskItem> CreateAsync(CreateTaskDto dto);                  // yeni görev oluştur
    Task<bool> UpdateAsync(int id, UpdateTaskDto dto);              // görev güncelle
    Task<bool> UpdateStatusAsync(int id, ProjectTaskStatus newStatus);  // sadece durumu değiştir (CompletedAt kuralı burada uygulanacak)
    Task<IEnumerable<TaskHistory>> GetHistoriesAsync(int taskId);  // bir görevin değişiklik geçmişini getirir
}