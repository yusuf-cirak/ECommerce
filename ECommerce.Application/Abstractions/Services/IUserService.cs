using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Application.DTOs.User;

namespace ECommerce.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<bool> CreateAsync(CreateUser model);
    }
}
