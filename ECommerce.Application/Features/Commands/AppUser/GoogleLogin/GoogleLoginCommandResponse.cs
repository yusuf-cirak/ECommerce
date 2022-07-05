using ECommerce.Application.DTOs;

namespace ECommerce.Application.Features.Commands.AppUser.GoogleLogin;

public class GoogleLoginCommandResponse
{
    public Token Token { get; set; }
}