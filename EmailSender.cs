using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace New_Eticket.Utility
{
    public class EmailSender : IEmailSender

    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(userName: "esraa.hussein.93.eh@gmail.com", password: "lrem kbmp tfmo hvrv")
            };

            return client.SendMailAsync(
                new MailMessage(from: "esraa.hussein.93.eh@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                {
                    IsBodyHtml = true
                });
        }
    }
}
