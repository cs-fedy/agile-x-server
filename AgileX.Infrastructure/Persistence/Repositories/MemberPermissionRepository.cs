using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.ObjectValues;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class MemberPermissionRepository : IMemberPermissionRepository
{
    private readonly CustomDbContext _dbContext;

    public MemberPermissionRepository(CustomDbContext dbContext) => _dbContext = dbContext;

    public void Save(MemberPermission memberPermission)
    {
        var existingMemberPermission = Get(
            memberPermission.ProjectId,
            memberPermission.UserId,
            memberPermission.Permission
        );

        if (existingMemberPermission == null)
            _dbContext.MemberPermissions.Add(memberPermission);
        else
            existingMemberPermission = memberPermission with { };
        _dbContext.SaveChanges();
    }

    public void Delete(Guid projectId, Guid userId, Permission permission)
    {
        var existingMemberPermission = Get(projectId, userId, permission);
        if (existingMemberPermission == null)
            return;

        existingMemberPermission = existingMemberPermission with
        {
            IsDeleted = true,
            DeletedAt = DateTime.Now,
        };

        _dbContext.SaveChanges();
    }

    public List<MemberPermission> ListByMemberId(Guid projectId, Guid userId) =>
        _dbContext
            .MemberPermissions
            .Where(x => x.ProjectId == projectId && x.UserId == userId)
            .ToList();

    public MemberPermission? Get(Guid projectId, Guid userId, Permission permission) =>
        _dbContext
            .MemberPermissions
            .SingleOrDefault(
                memberPermission =>
                    memberPermission.ProjectId == projectId
                    && memberPermission.UserId == userId
                    && memberPermission.Permission == permission
            );
}
