using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemV2.Entities;
using ProjectManagementSystemV2.Entities.DTOs;
using ProjectManagementSystemV2.Services;

namespace ProjectManagementSystemV2.Controllers;

[Route("api/tasks")]
[ApiController]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // Görevleri sayfalı ve filtreli listeler
    [HttpGet]
    public async Task<ActionResult> GetTasks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? projectId = null,
        [FromQuery] ProjectTaskStatus? status = null,
        [FromQuery] TaskPriority? priority = null,
        [FromQuery] int? assignedToUserId = null)
    {
        if (pageSize > 50) pageSize = 50;
        if (page < 1) page = 1;

        var (items, totalCount) = await _taskService.GetAllAsync(
            page, pageSize, projectId, status, priority, assignedToUserId);

        return Ok(new { totalCount, page, pageSize, items });
    }

    // Tek görev detayı
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetTask(int id)
    {
        var task = await _taskService.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound($"Id {id} ile eşleşen görev bulunamadı.");
        }

        return Ok(task);
    }

    // Yeni görev oluşturur
    [HttpPost]
    public async Task<ActionResult> CreateTask(CreateTaskDto dto)
    {
        try
        {
            var task = await _taskService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // Sadece görevin durumunu değiştirir (CompletedAt kuralı otomatik uygulanır)
    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult> UpdateTaskStatus(int id, [FromBody] ProjectTaskStatus newStatus)
    {
        var success = await _taskService.UpdateStatusAsync(id, newStatus);
        if (!success)
        {
            return NotFound($"Id {id} ile eşleşen görev bulunamadı.");
        }

        return Ok(new { message = "Görev durumu güncellendi." });
    }

    // Bir görevin değişiklik geçmişini listeler
    [HttpGet("{taskId:int}/histories")]
    public async Task<ActionResult> GetTaskHistories(int taskId)
    {
        var histories = await _taskService.GetHistoriesAsync(taskId);
        return Ok(histories);
    }
}