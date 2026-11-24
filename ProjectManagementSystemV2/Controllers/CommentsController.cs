using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystemV2.Entities.DTOs;
using ProjectManagementSystemV2.Services;

namespace ProjectManagementSystemV2.Controllers;

[Route("api/tasks/{taskId:int}/comments")]
[ApiController]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    private bool TryGetCurrentUserId(out int userId)
    {
        var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(idStr, out userId);
    }

    [HttpGet]
    public async Task<ActionResult> GetComments(int taskId)
    {
        var comments = await _commentService.GetByTaskIdAsync(taskId);
        return Ok(comments);
    }

    [HttpPost]
    public async Task<ActionResult> CreateComment(int taskId, CreateCommentDto dto)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized("Geçerli kullanıcı kimliği bulunamadı.");
        }

        dto.TaskId = taskId;
        var comment = await _commentService.CreateAsync(dto, userId);
        return Ok(comment);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateComment(int taskId, int id, UpdateCommentDto dto)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized("Geçerli kullanıcı kimliği bulunamadı.");
        }

        var success = await _commentService.UpdateAsync(id, dto, userId);
        if (!success)
        {
            return NotFound("Yorum bulunamadı.");
        }

        return Ok(new { message = "Yorum güncellendi." });
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteComment(int taskId, int id)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized("Geçerli kullanıcı kimliği bulunamadı.");
        }

        var success = await _commentService.DeleteAsync(id, userId);
        if (!success)
        {
            return NotFound("Yorum bulunamadı.");
        }

        return Ok(new { message = "Yorum silindi." });
    }
}