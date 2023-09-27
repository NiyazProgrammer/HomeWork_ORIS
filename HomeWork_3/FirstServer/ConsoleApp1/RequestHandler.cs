using System.Net;
using ConsoleApp1.Configuration;

namespace ConsoleApp1;

public class RequestHandler
{
    private readonly AppSettings _config;
    public RequestHandler(AppSettings config)
    {
        _config = config;
    }
    
    public async Task HandleRequestAsync(HttpListenerContext context)
    {
        var uri = context.Request.Url;
        var filePath = GetFilePath(uri);

        try
        {
            var response = context.Response;
            var path = File.ReadAllBytes(filePath);
            
            response.ContentType = DictionaryContentType.GetContentType(filePath) ;
   
            response.ContentLength64 = path.Length;
            
            using Stream output = response.OutputStream;

            await output.WriteAsync(path);
            await output.FlushAsync();
        }
        catch (FileNotFoundException)
        {
            Server404Page(context);
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