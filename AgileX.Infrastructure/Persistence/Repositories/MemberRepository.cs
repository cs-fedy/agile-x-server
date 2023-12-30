using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly CustomDbContext _dbContext;

    public MemberRepository(CustomDbContext dbContext) => _dbContext = dbContext;

    public void Save(Member member)
    {
        var existingMember = Get(member.ProjectId, member.UserId);
        if (existingMember == null)
            _dbContext.Members.Add(member);
        else
            existingMember = member with { };
        _dbContext.SaveChanges();
    }

    public void Delete(Guid projectId, Guid userId)
    {
        var existingMember = Get(projectId, userId);
        if (existingMember == null)
            return;

        existingMember = existingMember with { IsDeleted = true, DeletedAt = DateTime.Now };
        _dbContext.SaveChanges();
    }

    public Member? Get(Guid projectId, Guid userId) =>
        _dbContext
            .Members
            .SingleOrDefault(member => member.ProjectId == projectId && member.UserId == userId);

    public List<Member> ListByProjectId(Guid projectId) =>
        _dbContext.Members.Where(x => x.ProjectId == projectId).ToList();

    public List<Member> ListByUserId(Guid userId) =>
        _dbContext.Members.Where(x => x.UserId == userId).ToList();
}
