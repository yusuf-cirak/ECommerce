using ECommerce.Application.Abstractions.Services;
using MediatR;

namespace ECommerce.Application.Features.Commands.AppUser.VerifyResetToken;

public sealed class VerifyResetTokenCommandRequest:IRequest<bool>
{
    public string ResetToken { get; set; }
    public string UserId { get; set; }
}

public sealed class VerifyResetTokenCommandHandler:IRequestHandler<VerifyResetTokenCommandRequest,bool>
{
    private readonly IAuthService _authService;

    public VerifyResetTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Handle(VerifyResetTokenCommandRequest request, CancellationToken cancellationToken)
    {
        bool state = await _authService.VerifyResetTokenAsync(request.ResetToken, request.UserId);

        return state;
    }
}