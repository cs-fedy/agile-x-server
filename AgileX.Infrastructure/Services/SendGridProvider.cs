using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.ObjectValues;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AgileX.Infrastructure.Services;

public class SendGridProvider : IEmailProvider
{
    private readonly ISendGridClient _sendGridClient;
    private readonly EmailAddress _emailSender;

    public SendGridProvider(ISendGridClient sendGridClient, EmailAddress emailSender)
    {
        _sendGridClient = sendGridClient;
        _emailSender = emailSender;
    }

    public async Task Send(Email emailDetails)
    {
        SendGridMessage message =
            new()
            {
                From = _emailSender,
                Subject = emailDetails.Subject,
                PlainTextContent = emailDetails.PlainTextContent
            };

        foreach (var s in emailDetails.To)
            message.AddTo(new EmailAddress(s));

        await _sendGridClient.SendEmailAsync(message);
    }
}
