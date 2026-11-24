using ProjectManagementSystemV2.Entities;
using ProjectManagementSystemV2.Entities.DTOs;
using ProjectManagementSystemV2.Repositories;

namespace ProjectManagementSystemV2.Services;

public class CommentService : ICommentService
{
    private readonly IGenericRepository<Comment> _commentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(IGenericRepository<Comment> commentRepository, IUnitOfWork unitOfWork)
    {
        _commentRepository = commentRepository;
        _unitOfWork = unitOfWork;
    }

    // bir görevin silinmemiş tüm yorumlarını getirir
    public async Task<IEnumerable<Comment>> GetByTaskIdAsync(int taskId)
    {
        var all = await _commentRepository.GetAllAsync();
        return all.Where(c => c.TaskId == taskId && !c.IsDeleted);
    }

    // yeni yorum ekler
    public async Task<Comment> CreateAsync(CreateCommentDto dto, int userId)
    {
        var comment = new Comment
        {
            Content = dto.Content,
            TaskId = dto.TaskId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _commentRepository.AddAsync(comment);
        await _unitOfWork.CommitAsync();

        return comment;
    }

    // yorumu günceller - sadece yazan kişi ya da Admin yapabilir (kontrolü Controller'da yapacağız)
    public async Task<bool> UpdateAsync(int id, UpdateCommentDto dto, int userId)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null || comment.IsDeleted)
            return false;

        comment.Content = dto.Content;
        comment.UpdatedAt = DateTime.UtcNow;

        _commentRepository.Update(comment);
        await _unitOfWork.CommitAsync();

        return true;
    }

    // yorumu siler (soft delete)
    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var comment = await _commentRepository.GetByIdAsync(id);
        if (comment == null || comment.IsDeleted)
            return false;

        comment.IsDeleted = true;
        _commentRepository.Update(comment);
        await _unitOfWork.CommitAsync();

        return true;
    }
}