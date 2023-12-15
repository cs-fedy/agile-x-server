using AgileX.Domain.Entities;
using AgileX.Domain.ObjectValues;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface IMemberPermissionRepository
{
    void Save(MemberPermission memberPermission);
    void Delete(Guid projectId, Guid userId, Permission permission);
    List<MemberPermission> ListByMemberId(Guid projectId, Guid userId);

    MemberPermission? Get(Guid projectId, Guid userId, Permission permission);
}
