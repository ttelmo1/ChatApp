using System;
using ChatApp.Application.DTOs.User;
using ChatApp.Infraestructure.Models;
using MediatR;

namespace ChatApp.Application.Commands.Login;

public class LoginCommand : IRequest<ResultViewModel<string>>
{
    public string Username { get; set; }
    public string Password { get; set; }

    public LoginCommand(LoginDto dto)
    {
        Username = dto.Username;
        Password = dto.Password;
    }
}
