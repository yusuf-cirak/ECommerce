using System.Reflection.Metadata.Ecma335;
using ECommerce.Application.Abstractions.Token;
using ECommerce.Application.DTOs;
using ECommerce.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Application.Features.Commands.AppUser.LoginUser;

public class LoginUserCommandHandler:IRequestHandler<LoginUserCommandRequest,LoginUserCommandResponse>
{
    private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
    private readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
    private readonly ITokenHandler _tokenHandler;

    public LoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, SignInManager<Domain.Entities.Identity.AppUser> signInManager, ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenHandler = tokenHandler;
    }

    public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
    {
        Domain.Entities.Identity.AppUser user=await _userManager.FindByNameAsync(request.UsernameOrEmail);
        if (user==null)
            user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);

        if (user==null)
            throw new UserLoginFailedException();

        SignInResult result=await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (result.Succeeded)
        {
            Token token = _tokenHandler.CreateAccessToken(expiration: 10);
            return new ()
            {
                Token = token
            };
        }

        throw new UserLoginFailedException(message:"Kimlik doğrulama hatası");

    }
}