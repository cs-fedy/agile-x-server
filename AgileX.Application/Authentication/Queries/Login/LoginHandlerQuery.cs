﻿using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Result;
using AgileX.Domain.Common.Errors;
using MediatR;

namespace AgileX.Application.Authentication.Queries.Login;

public class LoginHandlerQuery : IRequestHandler<LoginQuery, Result<LoginResul>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginHandlerQuery(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<Result<LoginResul>> Handle(
        LoginQuery request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingUser = _userRepository.GetUserByEmail(request.Email);
        if (existingUser == null)
            return Result<LoginResul>.From(Errors.User.UserAlreadyExist);

        var token = _jwtTokenGenerator.GenerateToken(existingUser);
        return Result<LoginResul>.From(new LoginResul(token));
    }
}
