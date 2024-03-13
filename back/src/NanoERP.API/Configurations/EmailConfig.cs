namespace NanoERP.API.Configurations;

public class EmailConfig
{
    public string Address { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; } = 0;
}