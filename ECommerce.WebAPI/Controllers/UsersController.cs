using ECommerce.Application.Features.Commands.AppUser;
using ECommerce.Application.Features.Commands.AppUser.CreateUser;
using ECommerce.Application.Features.Commands.AppUser.FacebookLogin;
using ECommerce.Application.Features.Commands.AppUser.GoogleLogin;
using ECommerce.Application.Features.Commands.AppUser.LoginUser;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {

        [HttpPost, Route("[action]")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response = await Mediator.Send(createUserCommandRequest);
            return Ok(response);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommandRequest loginUserCommandRequest)
        {
            LoginUserCommandResponse response = await Mediator.Send(loginUserCommandRequest);
            return Ok(response);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginCommandRequest googleLoginCommandRequest)
        {
            GoogleLoginCommandResponse response = await Mediator.Send(googleLoginCommandRequest);
            return Ok(response);
        }

        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginCommandRequest
            facebookLoginCommandRequest)
        {
            FacebookLoginCommandResponse response = await Mediator.Send(facebookLoginCommandRequest);
            return Ok(response);
        }
    }
}
