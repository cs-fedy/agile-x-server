using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class MemberRepository : IMemberRepository
{
    public void Save(Member member)
    {
        throw new NotImplementedException();
    }

    public Member? Get(Guid projectId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public List<Member> ListByProjectId(Guid projectId)
    {
        throw new NotImplementedException();
    }
}
