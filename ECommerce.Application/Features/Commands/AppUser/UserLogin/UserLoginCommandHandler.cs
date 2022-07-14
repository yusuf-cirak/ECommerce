using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.DTOs;
using ECommerce.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Commands.AppUser.UserLogin;

public class UserLoginCommandHandler:IRequestHandler<UserLoginCommandRequest,UserLoginCommandResponse>
{
    private readonly IAuthService _authService;

    public UserLoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<UserLoginCommandResponse> Handle(UserLoginCommandRequest request, CancellationToken cancellationToken)
    {
        Token token=await _authService.LoginAsync(request.UsernameOrEmail, request.Password, 10);
        return new() { Token = token };

    }
}