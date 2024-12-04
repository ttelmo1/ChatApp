using ChatApp.Application.DTOs.User;
using ChatApp.Infraestructure.Models;
using ChatApp.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Application.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ResultViewModel<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    } 

    public async Task<ResultViewModel<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = new ApplicationUser
            {
                Name = request.Username,
                UserName = request.Username,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return ResultViewModel<string>.Success("User registered successfully");
            }

            return ResultViewModel<string>.Error(result.Errors.ToString());
        }
        catch (Exception ex)
        {
            return ResultViewModel<string>.Error(ex.Message);
        }
    }
}
