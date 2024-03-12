using System.Net;
using System.Net.Mail;

namespace NanoERP.API.Utilities;

public class EmailService(string emailAddress, string emailPassword, string emailHost = "smtp.gmail.com", int emailPort = 587)
{
    private readonly string _emailAddress = emailAddress;
    private readonly string _emailPassword = emailPassword;
    private readonly string _emailHost = emailHost;
    private readonly int _emailPort = emailPort;

    public void Send(string to, string subject, string body)
    {
        using var smtpClient = new SmtpClient
        {
            Host = _emailHost,
            Port = _emailPort,
            EnableSsl = true,
            Credentials = new NetworkCredential(_emailAddress, _emailPassword)
        };

        using var message = new MailMessage(_emailAddress, to)
        {
            Subject = subject,
            Body = body
        };

        smtpClient.Send(message);
    }
}