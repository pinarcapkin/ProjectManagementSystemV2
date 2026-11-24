using ProjectManagementSystemV2.Entities;
using ProjectManagementSystemV2.Entities.DTOs;
using ProjectManagementSystemV2.Repositories;

namespace ProjectManagementSystemV2.Services;

public class TaskService : ITaskService
{
    private readonly IGenericRepository<TaskItem> _taskRepository;
    private readonly IGenericRepository<TaskHistory> _historyRepository;
    private readonly IGenericRepository<Project> _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TaskService(
        IGenericRepository<TaskItem> taskRepository,
        IGenericRepository<TaskHistory> historyRepository,
        IGenericRepository<Project> projectRepository,
        IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _historyRepository = historyRepository;
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<(IEnumerable<TaskItem> Items, int TotalCount)> GetAllAsync(
        int page,
        int pageSize,
        int? projectId,
        ProjectTaskStatus? status,
        TaskPriority? priority,
        int? assignedToUserId)
    {
        var all = await _taskRepository.GetAllAsync();
        var query = all.Where(t => !t.IsDeleted);

        if (projectId.HasValue)
            query = query.Where(t => t.ProjectId == projectId.Value);

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority.Value);

        if (assignedToUserId.HasValue)
            query = query.Where(t => t.AssignedToUserId == assignedToUserId.Value);

        var totalCount = query.Count();

        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, totalCount);
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task != null && !task.IsDeleted ? task : null;
    }

    public async Task<TaskItem> CreateAsync(CreateTaskDto dto)
    {
        if (dto.DueDate.HasValue)
        {
            var project = await _projectRepository.GetByIdAsync(dto.ProjectId);
            if (project != null)
            {
                if (dto.DueDate.Value < project.StartDate)
                    throw new ArgumentException("Son teslim tarihi, projenin başlangıç tarihinden önce olamaz.");

                if (project.EndDate.HasValue && dto.DueDate.Value > project.EndDate.Value)
                    throw new ArgumentException("Son teslim tarihi, projenin bitiş tarihinden sonra olamaz.");
            }
        }

        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            ProjectId = dto.ProjectId,
            AssignedToUserId = dto.AssignedToUserId,
            CreatedByUserId = 1,
            Status = dto.Status,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            EstimatedHours = dto.EstimatedHours,
            CompletedAt = dto.Status == ProjectTaskStatus.Done ? DateTime.UtcNow : null,
            CreatedAt = DateTime.UtcNow
        };

        await _taskRepository.AddAsync(task);
        await _unitOfWork.CommitAsync();

        return task;
    }

    public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null || task.IsDeleted)
            return false;

        if (dto.DueDate.HasValue)
        {
            var project = await _projectRepository.GetByIdAsync(task.ProjectId);
            if (project != null)
            {
                if (dto.DueDate.Value < project.StartDate)
                    throw new ArgumentException("Son teslim tarihi, projenin başlangıç tarihinden önce olamaz.");

                if (project.EndDate.HasValue && dto.DueDate.Value > project.EndDate.Value)
                    throw new ArgumentException("Son teslim tarihi, projenin bitiş tarihinden sonra olamaz.");
            }
        }

        var oldStatus = task.Status;
        var oldAssignedUserId = task.AssignedToUserId;

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.AssignedToUserId = dto.AssignedToUserId;
        task.Status = dto.Status;
        task.Priority = dto.Priority;
        task.DueDate = dto.DueDate;
        task.EstimatedHours = dto.EstimatedHours;
        task.CompletedAt = dto.Status == ProjectTaskStatus.Done ? (task.CompletedAt ?? DateTime.UtcNow) : null;
        task.UpdatedAt = DateTime.UtcNow;

        _taskRepository.Update(task);

        if (oldStatus != dto.Status)
        {
            await _historyRepository.AddAsync(new TaskHistory
            {
                TaskId = task.Id,
                ChangedByUserId = 1,
                ChangeType = "StatusChanged",
                OldValue = oldStatus.ToString(),
                NewValue = dto.Status.ToString(),
                CreatedAt = DateTime.UtcNow
            });
        }

        if (oldAssignedUserId != dto.AssignedToUserId)
        {
            await _historyRepository.AddAsync(new TaskHistory
            {
                TaskId = task.Id,
                ChangedByUserId = 1,
                ChangeType = "AssignedUserChanged",
                OldValue = oldAssignedUserId?.ToString() ?? "Unassigned",
                NewValue = dto.AssignedToUserId?.ToString() ?? "Unassigned",
                CreatedAt = DateTime.UtcNow
            });
        }

        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task<bool> UpdateStatusAsync(int id, ProjectTaskStatus newStatus)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null || task.IsDeleted)
            return false;

        var oldStatus = task.Status;

        task.Status = newStatus;
        task.CompletedAt = newStatus == ProjectTaskStatus.Done ? DateTime.UtcNow : null;
        task.UpdatedAt = DateTime.UtcNow;

        _taskRepository.Update(task);

        if (oldStatus != newStatus)
        {
            var history = new TaskHistory
            {
                TaskId = task.Id,
                ChangedByUserId = 1,
                ChangeType = "StatusChanged",
                OldValue = oldStatus.ToString(),
                NewValue = newStatus.ToString(),
                CreatedAt = DateTime.UtcNow
            };
            await _historyRepository.AddAsync(history);
        }

        await _unitOfWork.CommitAsync();
        return true;
    }

    public async Task<IEnumerable<TaskHistory>> GetHistoriesAsync(int taskId)
    {
        var all = await _historyRepository.GetAllAsync();
        return all.Where(h => h.TaskId == taskId).OrderByDescending(h => h.CreatedAt);
    }
}