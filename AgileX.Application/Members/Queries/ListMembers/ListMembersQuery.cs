using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Members.Queries.ListMembers;

public record ListMembersQuery(Guid ProjectId, Guid LoggedUserId) : IRequest<Result<List<Member>>>;
