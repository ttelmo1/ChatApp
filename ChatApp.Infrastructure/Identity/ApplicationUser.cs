using System;
using ChatApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
