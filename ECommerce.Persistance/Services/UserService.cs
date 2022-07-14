using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.DTOs.User;
using ECommerce.Application.Exceptions;
using ECommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Persistance.Services
{
    public class UserService:IUserService
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
            //if (result.Succeeded)
            //{
            //    return new() { Succeeded = true, Message = "Kullanıcı başarıyla oluşturuldu" };
            //}

            //throw new UserCreateFailedException();
        }
    }
}
