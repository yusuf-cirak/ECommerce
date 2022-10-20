using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.DTOs.User;
using ECommerce.Domain.Entities.Identity;

namespace ECommerce.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<bool> CreateAsync(CreateUser model);
        Task UpdateRefreshToken(string refreshToken, AppUser? user, DateTime accessTokenExpiration, int addOnAccessTokenDate);

        Task UpdatePassword(string userId, string resetToken, string newPassword);
    }
}
