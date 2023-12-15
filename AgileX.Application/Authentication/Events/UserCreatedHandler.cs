using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using MediatR;

namespace AgileX.Application.Authentication.Events;

public sealed class UserCreatedHandler : INotificationHandler<UserCreated>
{
    private readonly IUserRepository _userRepository;
    private readonly ICacheRepository _cacheRepository;
    private readonly ICodeProvider _codeProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEventBus _eventBus;

    private const int EmailConfirmationCodeExpiryMinutes = 60;

    public UserCreatedHandler(
        IUserRepository userRepository,
        ICacheRepository cacheRepository,
        ICodeProvider codeProvider,
        IDateTimeProvider dateTimeProvider,
        IEventBus eventBus
    )
    {
        _userRepository = userRepository;
        _cacheRepository = cacheRepository;
        _codeProvider = codeProvider;
        _dateTimeProvider = dateTimeProvider;
        _eventBus = eventBus;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        var existingUser = _userRepository.GetById(notification.UserId);

        // TODO: instead of returning task completed, handle this error by putting it in an error queue
        if (existingUser is null || existingUser.IsDeleted)
            return;

        var code = _codeProvider.Generate(
            5,
            _dateTimeProvider.UtcNow.AddMinutes(EmailConfirmationCodeExpiryMinutes)
        );

        await _cacheRepository.Cache(
            $"confirmation_code_{existingUser.UserId}",
            code.Digits,
            code.ExpiresIn
        );

        // TODO: use relative time instead of plain object
        var email = new Email(
            To: new List<string>() { existingUser.Email },
            Subject: "Account created successfully",
            PlainTextContent: $"Dear {existingUser.FullName}. \n"
                + "Your account has been created successfully, "
                + $"To get started please login and confirm your account using this code `{code.Digits}` "
                + $"before it expires in {code.ExpiresIn}"
        );

        await _eventBus.Publish(new NewEmail(email), cancellationToken);
    }
}
