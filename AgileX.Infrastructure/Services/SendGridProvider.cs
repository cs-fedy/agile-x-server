using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.ObjectValues;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AgileX.Infrastructure.Services;

public class SendGridProvider : IEmailProvider
{
    private readonly ISendGridClient _sendGridClient;
    private readonly EmailAddress _email_sender;

    public SendGridProvider(ISendGridClient sendGridClient, EmailAddress emailSender)
    {
        _sendGridClient = sendGridClient;
        _email_sender = emailSender;
    }

    public async Task Send(Email EmailDetails)
    {
        SendGridMessage message =
            new()
            {
                From = _email_sender,
                Subject = EmailDetails.Subject,
                PlainTextContent = EmailDetails.PlainTextContent
            };

        foreach (var s in EmailDetails.To)
            message.AddTo(new EmailAddress());

        await _sendGridClient.SendEmailAsync(message);
    }
}
