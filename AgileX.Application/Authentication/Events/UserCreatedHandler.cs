using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.ObjectValues;
using MediatR;
using DomainEvents = AgileX.Domain.Events.Events;

namespace AgileX.Application.Authentication.Events;

public class UserCreatedHandler : INotificationHandler<DomainEvents.User.UserCreated>
{
    private readonly IUserRepository _userRepository;
    private readonly ICacheRepository _cacheRepository;
    private readonly IEmailProvider _emailProvider;
    private readonly ICodeProvider _codeProvider;
    private readonly IDateTimeProvider _dateTimeProvider;

    private const int EMAIL_CONFIRMATION_CODE_EXPIRY_MINUTES = 60;

    public UserCreatedHandler(
        IUserRepository userRepository,
        ICacheRepository cacheRepository,
        IEmailProvider emailProvider,
        ICodeProvider codeProvider,
        IDateTimeProvider dateTimeProvider
    )
    {
        _userRepository = userRepository;
        _cacheRepository = cacheRepository;
        _emailProvider = emailProvider;
        _codeProvider = codeProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public Task Handle(
        DomainEvents.User.UserCreated notification,
        CancellationToken cancellationToken
    )
    {
        var existingUser = _userRepository.GetUserById(notification.UserId);

        // TODO: instead of returning task completed, handle this error by putting it in an error queue
        if (existingUser is null)
            return Task.CompletedTask;

        var code = _codeProvider.Generate(
            5,
            _dateTimeProvider.UtcNow.AddMinutes(EMAIL_CONFIRMATION_CODE_EXPIRY_MINUTES)
        );

        _cacheRepository.Cache(
            $"confirmation_code_{existingUser.UserId}",
            code.Digits,
            code.ExpiresIn
        );

        // TODO: use relative time instead of plain object
        Email email = new Email(
            To: new List<string>() { existingUser.Email },
            Subject: "Account created successfully",
            PlainTextContent: $"Dear {existingUser.FullName}. \n"
                + "Your account has been created successfully, "
                + $"To get started please login and confirm your account using this code `{code.Digits}` "
                + $"before it expires in {code.ExpiresIn}"
        );

        _emailProvider.Send(email);
        return Task.CompletedTask;
    }
}
