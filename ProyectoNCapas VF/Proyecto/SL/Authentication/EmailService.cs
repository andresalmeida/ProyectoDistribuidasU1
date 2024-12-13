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

        try
        {
            // Aquí agregamos el envío real del correo
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // En caso de error, lo logueas o manejas adecuadamente
            Console.WriteLine($"Error al enviar correo: {ex.Message}");
            throw;
        }
    }
}
