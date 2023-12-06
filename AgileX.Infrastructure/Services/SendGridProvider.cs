using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AgileX.Infrastructure.Services;

public class SendGridProvider : IEmailProvider
{
    private readonly ISendGridClient _sendGridClient;

    public SendGridProvider(ISendGridClient sendGridClient) => _sendGridClient = sendGridClient;

    public async Task Send(Email EmailDetails)
    {
        SendGridMessage message =
            new()
            {
                From = new EmailAddress(EmailDetails.From),
                Subject = EmailDetails.Subject,
                PlainTextContent = EmailDetails.PlainTextConetnt
            };

        message.AddTo(new EmailAddress(EmailDetails.To));
        await _sendGridClient.SendEmailAsync(message);
    }
}
