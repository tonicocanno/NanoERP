using NanoERP.API.Domain.interfaces;

namespace NanoERP.API.DTO;

public class UserChangePasswordDto : IUser
{
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}