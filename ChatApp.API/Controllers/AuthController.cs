using ChatApp.Application.Commands.Login;
using ChatApp.Application.Commands.Register;
using ChatApp.Application.DTOs.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var command = new RegisterCommand(registerDto);
        var result = await _mediator.Send(command);
        return Ok(result.Data);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var command = new LoginCommand(loginDto);
        var result = await _mediator.Send(command);
        return Ok(result.Data);
    }
}

