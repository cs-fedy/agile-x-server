using AgileX.Domain.Result;

namespace AgileX.Domain.Events;

public static class Events
{
    public static class User
    {
        public record UserCreated(Guid UserId) : IEvent;
    }
}