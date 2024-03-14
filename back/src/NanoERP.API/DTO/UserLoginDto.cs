using NanoERP.API.Domain.Entities;

namespace NanoERP.API.DTO;

public class UserLoginDto : IUser
{    
    public string Email { get; set; } = "";
    public string Username { get; set; }  = "";
    public string Password { get; set; } = "";
}