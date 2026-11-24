using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemV2.Entities.DTOs;
using ProjectManagementSystemV2.Services;

namespace ProjectManagementSystemV2.Controllers;

[Route("api/tasks/{taskId:int}/time-logs")]
[ApiController]
[Authorize]
public class TimeLogsController : ControllerBase
{
    private readonly ITimeLogService _timeLogService;

    public TimeLogsController(ITimeLogService timeLogService)
    {
        _timeLogService = timeLogService;
    }

    private bool TryGetCurrentUserId(out int userId)
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idStr, out userId);
    }

    // Yeni zaman kaydı ekler
    [HttpPost]
    public async Task<ActionResult> CreateTimeLog(int taskId, CreateTimeLogDto dto)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized("Geçerli kullanıcı kimliği bulunamadı.");
        }

        dto.TaskId = taskId;
        var timeLog = await _timeLogService.CreateAsync(dto, userId);
        return Ok(timeLog);
    }

    // Bir görevin tüm zaman kayıtlarını listeler
    [HttpGet]
    public async Task<ActionResult> GetTimeLogs(int taskId)
    {
        var logs = await _timeLogService.GetByTaskIdAsync(taskId);
        return Ok(logs);
    }

    // Bir görevin toplam harcanan saatini döner (ActualHours)
    [HttpGet("total")]
    public async Task<ActionResult> GetTotalHours(int taskId)
    {
        var total = await _timeLogService.GetTotalHoursAsync(taskId);
        return Ok(new { taskId, totalHours = total });
    }
}