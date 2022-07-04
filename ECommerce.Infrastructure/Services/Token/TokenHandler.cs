using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Infrastructure.Services.Token
{
    public class TokenHandler:ITokenHandler
    {
        private readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token CreateAccessToken(int expiration)
        {
            Application.DTOs.Token token = new();

            // SecurityKey'in simetriğini alıyoruz.
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            // Şifrelenmiş kimliği oluşturuyoruz.

            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            // Oluşturulacak token'ın ayarlarını yapıyoruz.
            token.Expiration=DateTime.UtcNow.AddMinutes(expiration);

            JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires:token.Expiration,
                notBefore:DateTime.UtcNow, // Expire ne zaman devreye girsin? Token üretildiği anda
                signingCredentials:signingCredentials
            );

            // Token oluşturucu sınıfından bir örnek alalım

            JwtSecurityTokenHandler tokenHandler = new();

            token.AccessToken=tokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
