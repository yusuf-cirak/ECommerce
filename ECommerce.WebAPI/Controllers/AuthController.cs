using ECommerce.Application.Features.Commands.AppUser.FacebookLogin;
using ECommerce.Application.Features.Commands.AppUser.GoogleLogin;
using ECommerce.Application.Features.Commands.AppUser.RefreshTokenLogin;
using ECommerce.Application.Features.Commands.AppUser.UserLogin;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserLoginCommandRequest userLoginCommandRequest)
        {
            UserLoginCommandResponse response = await Mediator.Send(userLoginCommandRequest);
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
        
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromBody] RefreshTokenLoginCommandRequest
            refreshTokenLoginCommandRequest)
        {
            RefreshTokenLoginCommandResponse response = await Mediator.Send(refreshTokenLoginCommandRequest);
            return Ok(response);
        }
    }
}
