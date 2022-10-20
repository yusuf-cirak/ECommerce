using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs.User;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Helpers;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Persistance.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateAsync(CreateUser model)
        {
            IdentityResult result = await _userManager.CreateAsync(new() //AutoMapper eklenecek
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            }, model.Password);
            return result.Succeeded;
        }

        public async Task UpdateRefreshToken(string refreshToken, AppUser? user, DateTime accessTokenExpiration, int addOnAccessTokenDate)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenExpiration.AddMinutes(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else throw new UserNotFoundException();

        }

        public async Task UpdatePassword(string resetToken, string userId, string newPassword)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user!=null)
            {
                resetToken = resetToken.DecodeUrl();
                IdentityResult result = await _userManager.ResetPasswordAsync(user,resetToken,newPassword);

                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                }
                else
                {
                    throw new UserPasswordChangeFailedException();
                }
            }
        }
    }
}
