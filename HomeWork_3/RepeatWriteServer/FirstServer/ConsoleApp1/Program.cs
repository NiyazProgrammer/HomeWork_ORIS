
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

