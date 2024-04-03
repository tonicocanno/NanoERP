namespace NanoERP.API.Domain.interfaces;

public interface IUser
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}