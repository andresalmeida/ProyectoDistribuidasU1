using SL.Authentication;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpClient = new SmtpClient("smtp.sendgrid.net")
        {
            Port = 587,
            Credentials = new NetworkCredential("apikey", "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"),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress("almeidaandres@icloud.com"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(toEmail);
    }
}
