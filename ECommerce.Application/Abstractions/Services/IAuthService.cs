using ECommerce.Application.Abstractions.Services.Authentications;

namespace ECommerce.Application.Abstractions.Services;

public interface IAuthService : IInternalAuthentication,IExternalAuthentication
{
    Task ResetPasswordAsync(string email);
    Task<bool> VerifyResetTokenAsync(string resetToken,string userId);

}