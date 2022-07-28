using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Features.Commands.AppUser.RefreshTokenLogin;

public class RefreshTokenLoginCommandHandler:IRequestHandler<RefreshTokenLoginCommandRequest,RefreshTokenLoginCommandResponse>
{
    private readonly IAuthService _authService;

    public RefreshTokenLoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<RefreshTokenLoginCommandResponse> Handle(RefreshTokenLoginCommandRequest request, CancellationToken cancellationToken)
    {
        Token token= await _authService.RefreshTokenLoginAsync(request.RefreshToken,15);
        return new() { Token = token };
    }
}