using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs.Facebook;
using ECommerce.Application.Exceptions;
using ECommerce.Domain.Entities.Identity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Persistance.Services
{
    public class AuthService:IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<AppUser> userManager, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager)
        {
            _configuration = configuration; // to access secrets.json
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _httpClient = httpClientFactory.CreateClient(); // for http requests
        }

        private async Task<Token> CreateUserExternalAsync(string providerName,string userId,string email,string name,int accessTokenLifeTime)
        {
            var info = new UserLoginInfo(providerName, userId, providerName);
            // AspNetLoginUsers tablosu dış kaynaktan bize gelen Login'leri kaydettiğimiz tablodur.
            // UserLoginInfo nesnesi de dış kaynaktaki kullanıcıyı AspNetLoginUsers'a kaydedebileceğimiz bir nesne verir.
            AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        FullName = name
                    };
                    var identityResult = await _userManager.CreateAsync(user); // AspNetUesrse tablosuna kullanıcı ekleniyor.
                    result = identityResult.Succeeded;
                }
            }

            if (result) // Eğer kullanıcı başarıyla AspNetUsers'a kaydedildiyse ya da veri tabanında bulunduysa
                await _userManager.AddLoginAsync(user, info);
            else
                throw new Exception("Invalid external authentication");

            return _tokenHandler.CreateAccessToken(accessTokenLifeTime);
        }

        public async Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
        {
            // 1- Access token elde edilmeli. Bize gelen response'da access_token ve token_type'ın olduğu bir string geliyor, deserialize edilmesi lazım. Sonrasında FacebookAccessTokenResponse nesnesi olarak elde etmemiz gerekiyor. 
            string accessTokenResponse = await _httpClient.GetStringAsync(
                $"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");

            FacebookAccessTokenResponse? facebookAccessToken =
                JsonSerializer.Deserialize<FacebookAccessTokenResponse>(accessTokenResponse);

            // 2- Kullanıcının uygulamaya olan erişimini doğrulamamız gerekiyor. Bize bir sürü prop geliyor ama şimdilik userId ve isValid yeterli. Bu yüzden nesne oluşturuyoruz.

            string userAccessValidation = await _httpClient.GetStringAsync(
                $"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessToken?.AccessToken}");

            FacebookUserAccessTokenValidation? validation =
                JsonSerializer.Deserialize<FacebookUserAccessTokenValidation>(userAccessValidation);

            // 3- Uygulama ile kullanıcı arasındaki bağlantıda sorun yoksa, kullanıcı bilgilerini elde etmemiz gerekiyor.

            if (validation?.Data.IsValid != null)
            {
                string userInfoResponse =
                    await _httpClient.GetStringAsync(
                        $"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");

                FacebookUserInfoResponse? userInfo =
                    JsonSerializer.Deserialize<FacebookUserInfoResponse>(userInfoResponse);

                // Kullanıcıyı doğruladık ve bilgilerini elde ettik, artık veri tabanımıza kaydedip login etmeliyiz veya sadece login etmeliyiz.
                // todo refactor

                return await CreateUserExternalAsync("FACEBOOK", validation.Data.UserId, userInfo.Email, userInfo.Name, accessTokenLifeTime);
            }
            throw new Exception("Invalid external authentication");
        }

        public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>()
                    { $"{_configuration["ExternalLoginSettings:Google:Client_ID"]}" }
            };

            GoogleJsonWebSignature.Payload? payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            // GoogleLogin'de payload.Subject => UserId'dir.
            return await CreateUserExternalAsync("GOOGLE", payload.Subject, payload.Email, payload.Name,
                accessTokenLifeTime);
        }

        public async Task<Token> LoginAsync(string usernameOrEmail, string password, int accessTokenLifeTime)
        {
            AppUser user = await _userManager.FindByNameAsync(usernameOrEmail) ?? await _userManager.FindByEmailAsync(usernameOrEmail);

            if (user == null)
                throw new UserLoginFailedException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
                return _tokenHandler.CreateAccessToken(accessTokenLifeTime);
            throw new UserLoginFailedException(message: "Kimlik doğrulama hatası");
        }



    }
}
