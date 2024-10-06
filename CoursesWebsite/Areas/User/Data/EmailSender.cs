using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var fmail = "am5679456@gmail.com";
        var fpassword = "jela jvju ihoh dzst";

        var msg = new MailMessage();
        msg.From = new MailAddress(fmail);
        msg.Subject = subject;
        msg.To.Add(email);
        msg.Body = $"<html><body>{htmlMessage}</body></html>";
        msg.IsBodyHtml = true;

        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(fmail, fpassword),
            Port= 587 // 465 //25
        };

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        smtpClient.Send(msg);

        return Task.CompletedTask;
    }
}
