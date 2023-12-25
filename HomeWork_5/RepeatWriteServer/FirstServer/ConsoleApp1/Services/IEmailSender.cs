namespace ConsoleApp1;

public interface IEmailSender
{
    void SendEmail(string emailFromUser, string passwordFromUser);
}