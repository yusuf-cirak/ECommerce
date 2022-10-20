using ECommerce.Application.Abstractions.Services;
using MediatR;

namespace ECommerce.Application.Features.Commands.AppUser.ResetUserPassword;

public sealed class ResetUserPasswordCommandRequest:IRequest<bool>
{
    public string Email { get; set; }
}

public sealed class ResetUserPasswordCommandHandler:IRequestHandler<ResetUserPasswordCommandRequest,bool>
{
    private readonly IAuthService _authService;

    public ResetUserPasswordCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Handle(ResetUserPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        await _authService.ResetPasswordAsync(request.Email);

        return true;
    }
}