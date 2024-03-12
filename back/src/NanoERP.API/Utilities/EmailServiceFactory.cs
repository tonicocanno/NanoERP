using NanoERP.API.Configurations;

namespace NanoERP.API.Utilities;

public class EmailServiceFactory(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public EmailService Create()
    {
        var emailConfig = _configuration.GetSection("EmailSettings").Get<EmailConfig>();

        if (emailConfig == null || string.IsNullOrEmpty(emailConfig.Address) || string.IsNullOrEmpty(emailConfig.Password))
        {
            throw new Exception("Email address and password must be set in environment variables");
        }

        return new EmailService(emailConfig.Address, emailConfig.Password, emailConfig.Host, emailConfig.Port);
    }
}