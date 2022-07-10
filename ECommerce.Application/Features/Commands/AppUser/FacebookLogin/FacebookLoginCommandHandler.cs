using System.Text.Json;
using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.DTOs.Facebook;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Commands.AppUser.FacebookLogin;

public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
{
    private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
    private readonly ITokenHandler _tokenHandler;
    private readonly HttpClient _httpClient;

    public FacebookLoginCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler, IHttpClientFactory httpClientFactory)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _httpClient = httpClientFactory.CreateClient();
    }
    public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
    {
        // 1- Access token elde edilmeli. Bize gelen response'da access_token ve token_type'ın olduğu bir string geliyor, deserialize edilmesi lazım. Sonrasında FacebookAccessTokenResponse nesnesi olarak elde etmemiz gerekiyor. 
        string accessTokenResponse = await _httpClient.GetStringAsync("https://graph.facebook.com/oauth/access_token?client_id=374724658104694&client_secret=4fff6128b3ea68a02744562d6b39c713&grant_type=client_credentials");

        FacebookAccessTokenResponse? facebookAccessToken = JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);

        // 2- Kullanıcının uygulamaya olan erişimini doğrulamamız gerekiyor. Bize bir sürü prop geliyor ama şimdilik userId ve isValid yeterli. Bu yüzden nesne oluşturuyoruz.

        string userAccessValidation =await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={request.AuthToken}&access_token={facebookAccessToken?.AccessToken}");

        FacebookUserAccessTokenValidation? validation =
            JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessValidation);

        // 3- Uygulama ile kullanıcı arasındaki bağlantıda sorun yoksa, kullanıcı bilgilerini elde etmemiz gerekiyor.

        if (validation.Data.IsValid)
        {
            string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={request.AuthToken}");

            FacebookUserInfoResponse? userInfo= JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

            // Kullanıcıyı doğruladık ve bilgilerini elde ettik, artık veri tabanımıza kaydedip login etmeliyiz veya sadece login etmeliyiz.
            // todo refactor

            var info = new UserLoginInfo("FACEBOOK", validation.Data.UserId, "FACEBOOK");
            // AspNetLoginUsers tablosu dış kaynaktan bize gelen Login'leri kaydettiğimiz tablodur.
            // UserLoginInfo nesnesi de dış kaynaktaki kullanıcıyı AspNetLoginUsers'a kaydedebileceğimiz bir nesne verir.

            Domain.Entities.Identity.AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(userInfo?.Email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = userInfo?.Email,
                        UserName = userInfo?.Email,
                        FullName = userInfo?.Name
                    };
                    var identityResult = await _userManager.CreateAsync(user); // AspNetUesrse tablosuna kullanıcı ekleniyor.
                    result = identityResult.Succeeded;
                }
            }

            if (result) // Eğer kullanıcı başarıyla AspNetUsers'a kaydedildiyse
            {
                await _userManager.AddLoginAsync(user, info);

                return new() { Token = _tokenHandler.CreateAccessToken() };
            }

        }
        throw new Exception("Invalid external authentication");

    }

}