using NanoERP.API.Domain.interfaces;

namespace NanoERP.API.DTO;

public class UserRegistrationDto : IUser
{
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public string Email { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}