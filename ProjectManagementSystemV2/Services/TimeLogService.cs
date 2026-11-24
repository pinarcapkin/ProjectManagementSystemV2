using ProjectManagementSystemV2.Entities;
using ProjectManagementSystemV2.Entities.DTOs;
using ProjectManagementSystemV2.Repositories;

namespace ProjectManagementSystemV2.Services;

public class TimeLogService : ITimeLogService
{
    private readonly IGenericRepository<TaskTimeLog> _timeLogRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TimeLogService(IGenericRepository<TaskTimeLog> timeLogRepository, IUnitOfWork unitOfWork)
    {
        _timeLogRepository = timeLogRepository;
        _unitOfWork = unitOfWork;
    }

    // yeni zaman kaydı ekler
    public async Task<TaskTimeLog> CreateAsync(CreateTimeLogDto dto, int userId)
    {
        if (dto.Hours <= 0)
        {
            throw new ArgumentException("Harcanan çalışma süresi 0'dan büyük olmalıdır.");
        }

        var timeLog = new TaskTimeLog
        {
            TaskId = dto.TaskId,
            UserId = userId,
            HourSpent = dto.Hours,
            Description = dto.Description,
            LogDate = dto.WorkDate,
            CreatedAt = DateTime.UtcNow
        };

        await _timeLogRepository.AddAsync(timeLog);
        await _unitOfWork.CommitAsync();

        return timeLog;
    }

    // bir görevin tüm zaman kayıtlarını getirir
    public async Task<IEnumerable<TaskTimeLog>> GetByTaskIdAsync(int taskId)
    {
        var all = await _timeLogRepository.GetAllAsync();
        return all.Where(t => t.TaskId == taskId);
    }

    // bir görevin toplam harcanan saatini, kayıtların toplamından hesaplar
    public async Task<decimal> GetTotalHoursAsync(int taskId)
    {
        var logs = await GetByTaskIdAsync(taskId);
        return logs.Sum(l => l.HourSpent);
    }
}