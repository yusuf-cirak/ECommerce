using ECommerce.Application.Abstractions.Token;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Commands.AppUser.GoogleLogin;

public class GoogleLoginCommandHandler:IRequestHandler<GoogleLoginCommandRequest,GoogleLoginCommandResponse>
{
    private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
    private readonly ITokenHandler _tokenHandler;

    public GoogleLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
    }

    public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>()
                { "274926534424-jiib3jp655ctpnhoelvch216n40scatf.apps.googleusercontent.com" }
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

        var info =new UserLoginInfo(request.Provider, payload.Subject, request.Provider);
        // AspNetLoginUsers tablosu dış kaynaktan bize gelen Login'leri kaydettiğimiz tablodur.
        // UserLoginInfo nesnesi de dış kaynaktaki kullanıcıyı AspNetLoginUsers'a kaydedebileceğimiz bir nesne verir.

        Domain.Entities.Identity.AppUser user=await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        bool result = user != null;
        if (user==null)
        {
            user = await _userManager.FindByEmailAsync(payload.Email);
            if (user==null)
            {
                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = payload.Email,
                    UserName = payload.Email,
                    FullName = payload.Name
                };
                var identityResult = await _userManager.CreateAsync(user); // AspNetUesrse tablosuna kullanıcı ekleniyor.
                result = identityResult.Succeeded;
            }
        }

        if (result) // Eğer kullanıcı başarıyla AspNetUsers'a kaydedildiyse
            await _userManager.AddLoginAsync(user, info);
        else
            throw new Exception("Invalid external authentication");

        return new()
        {
            Token = _tokenHandler.CreateAccessToken(10)
        };
    }
}