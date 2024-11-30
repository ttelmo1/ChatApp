using System;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.API.DTOs.User;

public class LoginDto
{
    [Required(ErrorMessage = "The username is required")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
