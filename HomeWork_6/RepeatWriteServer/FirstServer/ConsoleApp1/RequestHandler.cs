using System.Net;
using System.Text.Json;
using ConsoleApp1.Configuration;
using HttpServerBattleNet.Model;

namespace ConsoleApp1;

public class RequestHandler
{
    public RequestHandler()
    {
    }
    
    public async Task GetFormDataAsync(HttpListenerRequest request)
    {
        if (request.HttpMethod == "POST")
        {
            Console.WriteLine(request.HttpMethod);
            using (var reader = new StreamReader(request.InputStream))
            {
                var requestBody = await reader.ReadToEndAsync();
                try
                {
                    var data = JsonSerializer.Deserialize<Account>(requestBody);
                    var email = new EmailSender();
                    Console.WriteLine(requestBody);
                    email.SendEmail(data.Email, data.Password);
                    Console.WriteLine($"Приняты данные: Email: {data.Email}, Password: {data.Password}");
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Ошибка десериализации JSON: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при отправки сообщения на почту {ex}");
                }
            }
        }
    }
}