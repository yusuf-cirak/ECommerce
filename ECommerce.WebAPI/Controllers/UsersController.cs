﻿using ECommerce.Application.Features.Commands.AppUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {

        [HttpPost,Route("[action]")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommandRequest createUserCommandRequest)
        {
            CreateUserCommandResponse response = await Mediator.Send(createUserCommandRequest);
            return Ok(response);
        }
    }
}
