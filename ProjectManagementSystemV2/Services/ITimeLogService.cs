using ProjectManagementSystemV2.Entities;
using ProjectManagementSystemV2.Entities.DTOs;

namespace ProjectManagementSystemV2.Services;

public interface ITimeLogService
{
    Task<TaskTimeLog> CreateAsync(CreateTimeLogDto dto, int userId);   // yeni zaman kaydı ekler
    Task<IEnumerable<TaskTimeLog>> GetByTaskIdAsync(int taskId);       // bir görevin tüm zaman kayıtlarını getirir
    Task<decimal> GetTotalHoursAsync(int taskId);                     // bir görevin toplam harcanan saatini hesaplar (ActualHours)
}