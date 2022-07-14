using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ECommerce.Application.Features.Commands.AppUser.UserLogin
{
    public class UserLoginCommandRequest:IRequest<UserLoginCommandResponse>
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }

    }
}
