using ECommerce.Application.Abstractions.Services.Authentications;

namespace ECommerce.Application.Abstractions.Services;

public interface IAuthService : IInternalAuthentication,IExternalAuthentication
{
}