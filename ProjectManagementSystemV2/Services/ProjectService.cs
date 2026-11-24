using ProjectManagementSystemV2.Entities;
using ProjectManagementSystemV2.Entities.DTOs;
using ProjectManagementSystemV2.Repositories;

namespace ProjectManagementSystemV2.Services;

public class ProjectService : IProjectService
{
    private readonly IGenericRepository<Project> _projectRepository;
    private readonly IGenericRepository<ProjectMember> _memberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProjectService(
        IGenericRepository<Project> projectRepository,
        IGenericRepository<ProjectMember> memberRepository,
        IUnitOfWork unitOfWork)
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
    }

    // Admin ise tüm projeleri, değilse kullanıcının sahibi veya aktif üyesi olduğu projeleri getirir
    public async Task<(IEnumerable<Project> Items, int TotalCount)> GetAllAsync(
        int page, int pageSize, ProjectStatus? status, int? currentUserId, bool isAdmin, string? sortBy, string? sortDirection)
    {
        var all = await _projectRepository.GetAllAsync();
        var query = all.Where(p => !p.IsDeleted && !p.IsArchived);

        if (!isAdmin && currentUserId.HasValue)
        {
            var allMembers = await _memberRepository.GetAllAsync();
            var memberProjectIds = allMembers
                .Where(m => m.UserId == currentUserId.Value && m.IsActive)
                .Select(m => m.ProjectId)
                .ToHashSet();

            query = query.Where(p => p.UserId == currentUserId.Value || memberProjectIds.Contains(p.Id));
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        var totalCount = query.Count();

        bool descending = sortDirection?.ToLower() == "desc";

        query = sortBy?.ToLower() switch
        {
            "title" => descending ? query.OrderByDescending(p => p.Title.ToLower()) : query.OrderBy(p => p.Title.ToLower()),
            "startdate" => descending ? query.OrderByDescending(p => p.StartDate) : query.OrderBy(p => p.StartDate),
            "status" => descending ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status),
            _ => descending ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt)
        };

        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return (items, totalCount);
    }

    // tek proje getirir, yoksa ya da silinmişse null döner
    public async Task<Project?> GetByIdAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        return project != null && !project.IsDeleted ? project : null;
    }

    // yeni proje oluşturur
    public async Task<Project> CreateAsync(CreateProjectDto dto)
    {
        if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate)
        {
            throw new ArgumentException("Proje bitiş tarihi, başlangıç tarihinden önce olamaz.");
        }

        var project = new Project
        {
            Title = dto.Title,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status,
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow
        };

        await _projectRepository.AddAsync(project);
        await _unitOfWork.CommitAsync();

        return project;
    }

    // var olan projeyi günceller
    public async Task<bool> UpdateAsync(int id, UpdateProjectDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null || project.IsDeleted)
        {
            return false;
        }

        if (dto.EndDate.HasValue && dto.EndDate.Value < dto.StartDate)
        {
            throw new ArgumentException("Proje bitiş tarihi, başlangıç tarihinden önce olamaz.");
        }

        project.Title = dto.Title;
        project.Description = dto.Description;
        project.StartDate = dto.StartDate;
        project.EndDate = dto.EndDate;
        project.Status = dto.Status;

        _projectRepository.Update(project);
        await _unitOfWork.CommitAsync();

        return true;
    }

    // projeyi arşivler, silmez
    public async Task<bool> ArchiveAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null || project.IsDeleted)
        {
            return false;
        }

        project.IsArchived = true;
        project.ArchivedAt = DateTime.UtcNow;

        _projectRepository.Update(project);
        await _unitOfWork.CommitAsync();

        return true;
    }

    // projeye ekip üyesi ekler
    public async Task<bool> AddMemberAsync(int projectId, AddProjectMemberDto dto)
    {
        var projectExists = await _projectRepository.GetByIdAsync(projectId);
        if (projectExists == null || projectExists.IsDeleted)
        {
            return false;
        }

        var allMembers = await _memberRepository.GetAllAsync();
        var existingMember = allMembers.FirstOrDefault(m => m.ProjectId == projectId && m.UserId == dto.UserId);

        if (existingMember != null)
        {
            if (existingMember.IsActive)
            {
                return false;
            }

            existingMember.IsActive = true;
            existingMember.Role = dto.Role;
            existingMember.JoinedAt = DateTime.UtcNow;
            _memberRepository.Update(existingMember);
            await _unitOfWork.CommitAsync();
            return true;
        }

        var member = new ProjectMember
        {
            ProjectId = projectId,
            UserId = dto.UserId,
            Role = dto.Role,
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _memberRepository.AddAsync(member);
        await _unitOfWork.CommitAsync();

        return true;
    }

    // projeden ekip üyesi çıkarır (soft delete)
    public async Task<bool> RemoveMemberAsync(int projectId, int userId)
    {
        var allMembers = await _memberRepository.GetAllAsync();
        var member = allMembers.FirstOrDefault(m => m.ProjectId == projectId && m.UserId == userId && m.IsActive);

        if (member == null)
        {
            return false;
        }

        member.IsActive = false;
        _memberRepository.Update(member);
        await _unitOfWork.CommitAsync();

        return true;
    }
}