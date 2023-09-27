using System.Net;
using System.Text;
using System.Text.Json;
using ConsoleApp1.Configuration;

namespace ConsoleApp1;

public class HttpServer
{
    private readonly AppSettings _config;
    private HttpListener _server = new HttpListener();
    public HttpServer(AppSettings config)
    {
        _config = config;
    }

    public async Task StartAsync()
    {
        
        _server.Prefixes.Add($"http://{_config.Address}:{_config.Port}/");

        try
        {
            _server.Start();
            Console.WriteLine("Сервер успешно запущен");

            while (_server.IsListening)
            {
                var context = await _server.GetContextAsync();
                await HandleRequestAsync(context);
                
                Console.WriteLine("Запрос обработан");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Server error: {ex.Message}");
        }
        finally
        {
            _server.Stop();
        }
    }

    public void StopServer()
    {
            _server.Stop();
            Console.WriteLine("Сервер остановлен");
    }

    private async Task HandleRequestAsync(HttpListenerContext context)
    {
        var uri = context.Request.Url;
        var filePath = GetFilePath(uri);

        try
        {
            var response = context.Response;
            var path = File.ReadAllBytes(filePath);
            
            if (filePath.Contains("svg")) response.Headers.Add("Content-Type", "image/svg+xml");
           
             
            response.ContentLength64 = path.Length;
            
            using Stream output = response.OutputStream;

            await output.WriteAsync(path);
            await output.FlushAsync();
            
        }
        catch (FileNotFoundException)
        {
            Serve404Page(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private string GetFilePath(Uri uri)
    {
        var filePath = uri.AbsolutePath;
        
        if (filePath.StartsWith("/static/buttleNet") && filePath.Length == 17)
        {
            filePath = uri.AbsolutePath.Substring(1) + "/index.html";
        }
        else if (filePath.StartsWith("/static/buttleNet"))
        {
            filePath = uri.AbsolutePath.Substring(1);
        }
        else
        {
            filePath = _config.StaticFilesPath + "/buttleNet" + uri.AbsolutePath.Substring(7);
        }
        
        Console.WriteLine(filePath);
        
        return filePath;
    }

    private void Serve404Page(HttpListenerContext context)
    {
        var path = File.ReadAllBytes(Path.Combine(_config.StaticFilesPath, "page404.html"));
        var response = context.Response;
        
        response.ContentLength64 = path.Length;
        using Stream output = response.OutputStream;

        output.WriteAsync(path); 
        output.FlushAsync();
    }
}