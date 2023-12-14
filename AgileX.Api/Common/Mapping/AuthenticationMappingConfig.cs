using AgileX.Application.Authentication.Commands.Login;
using AgileX.Application.Authentication.Commands.Register;
using AgileX.Contracts.Authentication.Login;
using AgileX.Contracts.Authentication.Register;
using Mapster;

namespace AgileX.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();
        config.NewConfig<RegisterCommand, RegisterResponse>();

        config.NewConfig<LoginRequest, LoginCommand>();
        config.NewConfig<LoginCommand, LoginResponse>();
    }
}
