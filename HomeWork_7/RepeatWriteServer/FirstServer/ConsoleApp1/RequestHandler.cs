using System.Net;
using System.Text.Json;
using ConsoleApp1.Configuration;
using HttpServerBattleNet.Model;

namespace ConsoleApp1;

public class RequestHandler
{
    private readonly AppSettings _config = new AppSettings();
    public RequestHandler()
    {
    }
    
    public async Task<Account> GetFormDataAsync(HttpListenerRequest request)
    {
        if (request.HttpMethod == "POST")
        {
            using (var reader = new StreamReader(request.InputStream))
            {
                var requestBody = await reader.ReadToEndAsync();
                Console.WriteLine(requestBody);
                try
                {
                    var data = JsonSerializer.Deserialize<Account>(requestBody);
                    var email = new EmailSender();
                    email.SendEmail(data.Email, data.Password, "AAA");
                    Console.WriteLine($"Приняты данные: Email: {data.Email}, Password: {data.Password}");
                    return data;
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Ошибка десериализации JSON: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при отправке сообщения{ex}");
                }
            }
        }

        return null;
    }

    private void Server404Page(HttpListenerContext context)
    {
        var path = File.ReadAllBytes(Path.Combine(_config.StaticFilesPath, "page404.html"));
        var response = context.Response;
        
        response.ContentLength64 = path.Length;
        using Stream output = response.OutputStream;

        output.WriteAsync(path); 
        output.FlushAsync();
    }
}