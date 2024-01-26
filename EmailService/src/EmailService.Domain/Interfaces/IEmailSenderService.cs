using EmailService.src.EmailService.Domain.Entities;

namespace EmailService.src.EmailService.Domain.Interfaces
{
    public interface IEmailSenderService
    {
        Task<bool> SendEmailAsync(Email email);
    }
}
