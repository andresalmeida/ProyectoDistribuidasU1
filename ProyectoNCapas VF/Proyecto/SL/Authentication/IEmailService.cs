using System.Threading.Tasks;

namespace SL.Authentication
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipientEmail, string subject, string body);
    }
}