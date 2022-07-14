using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ECommerce.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandRequest : DTOs.User.CreateUser, IRequest<CreateUserCommandResponse>
    {
        //public string FullName { get; set; }
        //public string UserName { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; }
        //public string PasswordConfirm { get; set; }
    }
}
