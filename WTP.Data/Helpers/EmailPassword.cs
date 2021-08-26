using Microsoft.Extensions.Options;
using System;
using System.Net.Mail;
using WTP.Data.Interfaces;
using WTP.Domain.Entities.Auth;
using WTP.Domain.Entities.Settings;

namespace WTP.Data.Helpers
{
    public class EmailPassword : IEmailPassword
    {
        private readonly MailSettings _mailSettings;

        public EmailPassword(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public bool SendEmailPasswordReset(ForgotPassword model, string link)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_mailSettings.Mail);
            mailMessage.To.Add(new MailAddress(model.email));

            mailMessage.Subject = "Password Reset";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = $"<H1>Try to click the link below<H1/> <br/> <div>{link}<div/>" +
                $"<div><label for='username'>Username:</label><input type='text' id='username' name='username'></div>" +
                $"<div><label for='pass'>Password (8 characters minimum):</label><input type='password' id='pass' name='password'minlength='8' required></div>" +
                $"<input type='submit' value='Sign in'>";

            SmtpClient client = new SmtpClient()
            {
                EnableSsl = true
            };
            client.Credentials = new System.Net.NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
            client.Host = _mailSettings.Host;
            client.Port = _mailSettings.Port;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
    }
}