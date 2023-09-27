using System.Net;
using System.Text;
using System.Text.Json;
using ConsoleApp1.Configuration;

namespace ConsoleApp1;

public class HttpServer
{
    private readonly AppSettings _config;
    private HttpListener _server = new HttpListener();
    private RequestHandler _handler;

    public HttpServer(AppSettings config)
    {
        _config = config;
        _handler = new RequestHandler(_config);
    }

    public async Task StartAsync()
    {
        
        _server.Prefixes.Add($"http://{_config.Address}:{_config.Port}/");
        _server.Start();
        Console.WriteLine("Сервер успешно запущен");

        while (_server.IsListening)
        {
            var context = await _server.GetContextAsync();
            await _handler.HandleRequestAsync(context);
        }
        Console.WriteLine("Запрос обработан");
    }

    public void StopServer()
    {
        if (_server != null && _server.IsListening)
        {
            _server.Stop();
        }
    }
}