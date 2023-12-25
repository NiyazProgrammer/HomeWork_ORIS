using System.Net;
using System.Text;
using System.Text.Json;
using ConsoleApp1.Configuration;
using HttpServerBattleNet.Model;
using Newtonsoft.Json;

namespace ConsoleApp1;

class Program
{
    private static async Task Main(string[] args)
    {
        try
        {
            using (var server = new HttpServer())
            {
                await server.StartAsync();
                server.ProcessStop();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
        }
    }
}

