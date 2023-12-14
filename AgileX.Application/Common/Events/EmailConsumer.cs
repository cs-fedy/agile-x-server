using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Events;
using MassTransit;

namespace AgileX.Application.Common.Events;

public sealed class EmailConsumer : IConsumer<NewEmail>
{
    private readonly IEmailProvider _emailProvider;

    public EmailConsumer(IEmailProvider emailProvider) => _emailProvider = emailProvider;

    public async Task Consume(ConsumeContext<NewEmail> context)
    {
        var email = context.Message.Email;
        await _emailProvider.Send(email);
    }
}
