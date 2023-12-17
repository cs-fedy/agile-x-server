using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Members.Queries.GetMember;

public class GetMemberQueryHandler : IRequestHandler<GetMemberQuery, Result<GetMemberResult>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMemberRepository _memberRepository;

    public GetMemberQueryHandler(
        IProjectRepository projectRepository,
        IUserRepository userRepository,
        IMemberRepository memberRepository
    )
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Result<GetMemberResult>> Handle(
        GetMemberQuery request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingProject = _projectRepository.GetById(request.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingUser = _userRepository.GetById(request.TargetUserId);
        if (existingUser is null || existingUser.IsDeleted)
            return UserErrors.UserNotFound with { Description = "Target user not found" };

        var existingLoggedMember = _memberRepository.Get(request.ProjectId, request.LoggedUserId);
        if (existingLoggedMember is null || existingLoggedMember.IsDeleted)
            return MemberErrors.UnauthorizedMember with
            {
                Description = "Logged user is not a member"
            };

        var existingTargetMember = _memberRepository.Get(request.ProjectId, request.TargetUserId);
        if (existingTargetMember is null || existingTargetMember.IsDeleted)
            return MemberErrors.UnauthorizedMember with
            {
                Description = "Target user is not a member"
            };

        return new GetMemberResult(Member: existingTargetMember, User: existingUser);
    }
}
