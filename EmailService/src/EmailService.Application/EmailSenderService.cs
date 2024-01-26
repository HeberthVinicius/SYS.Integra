using EmailService.src.EmailService.Domain.Entities;
using EmailService.src.EmailService.Domain.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace EmailService.src.EmailService.Application
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;

        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(Email email)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_configuration["SmtpConfig:smtpNomeEmail"], _configuration["SmtpConfig:smtpEmail"]));

                if (string.IsNullOrWhiteSpace(email.ToEmail))
                {
                    throw new ArgumentException("O endereço de e-mail não pode ser nulo ou vazio."//, nameof(email.ToEmail)
                        );
                }

                var toAddress = !string.IsNullOrWhiteSpace(email.ToName)
                    ? new MailboxAddress(email.ToName, email.ToEmail)
                    : new MailboxAddress("", email.ToEmail);

                message.To.Add(toAddress);
                message.Subject = email.Subject;

                var htmlContent = await LoadHtmlBodyAsync(email);

                message.Body = new TextPart("html")
                {
                    Text = htmlContent
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(_configuration["SmtpConfig:smtpServer"], Convert.ToInt32(_configuration["SmtpConfig:smtpPort"]), false);
                await client.AuthenticateAsync(_configuration["SmtpConfig:smtpUser"], _configuration["SmtpConfig:smtpPassword"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no envio de e-mail: {ex.Message}");
                return false;//throw;
            }
        }

        private async Task<string> LoadHtmlBodyAsync(Email email)
        {
            // Carrega o conteúdo do arquivo HTML usando a instância de IConfiguration
            var htmlFilePath = _configuration["HtmlFilePath"] + "\\senhaProvisoria.html";
            var htmlContent = await File.ReadAllTextAsync(htmlFilePath);

            // Substitui os marcadores de posição pelos valores correspondentes
            htmlContent = htmlContent.Replace("{0}", email.ToName);
            htmlContent = htmlContent.Replace("{1}", email.Body);
            htmlContent = htmlContent.Replace("{2}", _configuration["linkRecuperarSenha"]);
            htmlContent = htmlContent.Replace("{3}", email.Statement);

            return htmlContent;
        }
    }
}
