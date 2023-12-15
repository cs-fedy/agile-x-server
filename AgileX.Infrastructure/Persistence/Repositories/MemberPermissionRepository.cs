using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.ObjectValues;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class MemberPermissionRepository : IMemberPermissionRepository
{
    public void Save(MemberPermission memberPermission)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid projectId, Guid userId, Permission permission)
    {
        throw new NotImplementedException();
    }

    public List<MemberPermission> ListByMemberId(Guid projectId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public MemberPermission? Get(Guid projectId, Guid userId, Permission permission)
    {
        throw new NotImplementedException();
    }

    public MemberPermission? GetMemberPermission(
        Guid projectId,
        Guid userId,
        Permission permission
    )
    {
        throw new NotImplementedException();
    }
}
