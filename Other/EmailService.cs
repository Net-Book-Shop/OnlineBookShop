using System.Net.Mail;
using System.Net;

namespace OnlineBookShop.Other
{
    public class EmailService
    {
        private readonly string smtpServer = "sandbox.smtp.mailtrap.io";
        private readonly int smtpPort = 587;
        private readonly string smtpUsername = "b78e467bca1f56"; 
        private readonly string smtpPassword = "83523c1497d20b";

        public async Task SendEmailAsync(string toEmail, string subject, string body, string fromEmail = "from@example.com")
        {
            try
            {
                // Validate email addresses
                if (!IsValidEmail(toEmail))
                {
                    throw new Exception("Invalid recipient email address.");
                }

                if (!IsValidEmail(fromEmail))
                {
                    throw new Exception("Invalid sender email address.");
                }

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);
                    await client.SendMailAsync(mailMessage);

                    Console.WriteLine("Email sent successfully!");
                }
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.Message}");
                throw new Exception("SMTP Error: Unable to send email.", smtpEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                throw new Exception("Error occurred while sending email.", ex);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}