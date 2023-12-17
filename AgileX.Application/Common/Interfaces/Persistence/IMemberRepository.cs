using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface IMemberRepository
{
    void Save(Member member);
    void Delete(Guid projectId, Guid userId);
    Member? Get(Guid projectId, Guid userId);
    List<Member> ListByProjectId(Guid projectId);
    List<Member> ListByUserId(Guid userId);
}
