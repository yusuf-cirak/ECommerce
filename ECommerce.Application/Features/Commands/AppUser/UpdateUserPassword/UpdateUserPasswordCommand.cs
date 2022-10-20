using ECommerce.Application.Abstractions.Services;
using MediatR;

namespace ECommerce.Application.Features.Commands.AppUser.UpdateUserPassword;

public sealed class UpdateUserPasswordCommandRequest:IRequest<bool>
{
    public string UserId { get; set; }
    public string ResetToken { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordConfirm { get; set; }
}

public sealed class UpdateUserPasswordCommandHandler:IRequestHandler<UpdateUserPasswordCommandRequest,bool>
{
    private readonly IUserService _userService;

    public UpdateUserPasswordCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> Handle(UpdateUserPasswordCommandRequest request, CancellationToken cancellationToken)
    {
        if (request.NewPassword != request.NewPasswordConfirm)
            return await Task.FromResult(false);

        await _userService.UpdatePassword(request.UserId, request.ResetToken, request.NewPassword);
        
        return true;
    }
}