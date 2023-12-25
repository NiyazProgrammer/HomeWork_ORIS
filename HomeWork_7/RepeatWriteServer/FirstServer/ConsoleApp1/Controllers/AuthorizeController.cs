using ConsoleApp1;
using HttpServerBattleNet.Attribuets;
using HttpServerBattleNet.Model;


namespace HttpServerBattleNet.Controllers;

[Controller("Authorize")]
public class AuthorizeController
{
    [Post("SendToEmail")]
    public void SendToEmail(string emailFromUser, string passwordFromUser)
    {
        new EmailSender().SendEmail(emailFromUser,passwordFromUser,"");
        Console.WriteLine("Email has been sent.");
    }
    
    [Get("GetEmailList")]
    public string GetEmailList()
    {
        var htmlCode = "<html><head> <meta charset=\"utf-8\"></head><body><h1>Вы вызвали GetEmailList2</h1></body></html>";
        return htmlCode;
    }
    
    [Get("GetAccountsList")]
    public Account[] GetAccountsList()
    {
        var accounts = new[]
        {
            new Account(){Email = "email-1", Password = "password-1"},
            new Account(){Email = "email-2", Password = "password-2"},
        };
        
        return accounts;
    }
}