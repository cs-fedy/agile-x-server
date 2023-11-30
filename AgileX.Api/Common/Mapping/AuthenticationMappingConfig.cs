using AgileX.Application.Authentication.Commands.Register;
using AgileX.Application.Authentication.Queries.Login;
using AgileX.Contracts.Authentication.Login;
using AgileX.Contracts.Authentication.Register;
using Mapster;

namespace AgileX.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();
        config.NewConfig<RegisterCommand, RegiseterResponse>();

        config.NewConfig<LoginRequest, LoginQuery>();
        config.NewConfig<LoginQuery, LoginResponse>();
    }
}
