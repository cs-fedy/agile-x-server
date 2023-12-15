using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler
    : IRequestHandler<CreateProjectCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateProjectCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        IUserRepository userRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        CreateProjectCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingUser = _userRepository.GetById(request.UserId);
        if (existingUser is null || existingUser.IsDeleted)
            return UserErrors.UserNotFound;

        var createdAt = _dateTimeProvider.UtcNow;
        var projectId = Guid.NewGuid();

        _projectRepository.Save(
            new Project(
                ProjectId: projectId,
                Name: request.Name,
                Description: request.Description,
                CompletionStatus: 0,
                Progress: 0,
                Priority: request.Priority,
                Deadline: request.Deadline,
                IsDeleted: false,
                CreatedAt: createdAt,
                UpdatedAt: createdAt,
                DeletedAt: null
            )
        );

        _memberRepository.Save(
            new Member(
                ProjectId: projectId,
                UserId: request.UserId,
                Membership: Membership.PROJECT_OWNER,
                CreatedAt: createdAt,
                IsDeleted: false,
                DeletedAt: null
            )
        );

        return new SuccessMessage("Project created successfully");
    }
}
