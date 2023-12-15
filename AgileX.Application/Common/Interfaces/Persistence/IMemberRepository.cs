using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface IMemberRepository
{
    void Save(Member member);
    Member? Get(Guid projectId, Guid userId);
}
