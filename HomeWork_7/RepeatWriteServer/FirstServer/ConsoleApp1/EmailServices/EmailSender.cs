using System.Net.Mail;
using System.Net;
using ConsoleApp1.Configuration;


namespace ConsoleApp1;

public class EmailSender : IEmailSender
{
    private static AppSettings _config = ConfigManager.GetConfig();
    public void SendEmail(string email, string password, string subject)
    {
        MailAddress from = new MailAddress($"{_config.EmailSender}", $"{_config.FromName}");
        MailAddress to = new MailAddress(email);
        MailMessage m = new MailMessage(from, to);
        m.Subject = "Успешный вход в BattleNet";
        m.Body = $"<h2>Здравствуйте!</h2><br><p>Ваш логин: {email}</p><p>Ваш пароль: {password}</p>";
        m.IsBodyHtml = true;
        SmtpClient smtp = new SmtpClient($"{_config.SmtpServerHost}", _config.SmtpServerPort);
        smtp.Credentials = new NetworkCredential($"{_config.EmailSender}", $"{_config.PasswordSender}");
        smtp.EnableSsl = true;
        smtp.Send(m);
    }
}