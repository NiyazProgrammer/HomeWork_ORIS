using System.Net;
using System.Text;
using System.Text.Json;
using ConsoleApp1.Configuration;

namespace ConsoleApp1;

class Program
{
    private static AppSettings  _config = ConfigManager.GetConfig();
    
    private static async Task Main(string[] args)
    {
        try
        {
            var config = _config;

            var server = new HttpServer(config);
            
            server.StartAsync();

            Console.WriteLine("Нажми на любую клавишу");
            Console.ReadKey();
            Console.WriteLine();
            server.StopServer();
            
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File not found: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Сервер завершил свою работу");
        }
    }
}

