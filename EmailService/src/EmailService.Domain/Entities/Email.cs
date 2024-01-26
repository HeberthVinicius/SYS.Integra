namespace EmailService.src.EmailService.Domain.Entities
{
    public class Email
    {
        public string ToEmail { get; set; } = string.Empty;
        public string ToName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Statement { get; set; } = string.Empty;
    }
}
