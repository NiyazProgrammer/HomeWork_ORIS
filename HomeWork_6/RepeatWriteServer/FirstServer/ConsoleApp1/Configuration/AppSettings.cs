namespace ConsoleApp1.Configuration;

public class AppSettings
{
    public int Port { get; set; }
    
    public string Address { get; set; }
    
    public string StaticFilesPath { get; set; }
    
    public string EmailSender { get; set; }
    
    public string PasswordSender { get; set; }
    
    public string FromName { get; set; }
    
    public string SmtpServerHost { get; set; }
    
    public int SmtpServerPort { get; set; }
}