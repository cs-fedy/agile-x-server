using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Services;

public interface IEmailProvider
{
    Task Send(Email EmailDetails);
}
