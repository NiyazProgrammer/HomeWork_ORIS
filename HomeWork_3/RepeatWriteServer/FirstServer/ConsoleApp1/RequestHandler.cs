using System.Net;
using System.Text.Json;
using ConsoleApp1.Configuration;

namespace ConsoleApp1;

public class RequestHandler
{
    public RequestHandler()
    { }
    private AppSettings _config = ConfigManager.GetConfig();
        
    public void GetPages( HttpListenerContext context)
    { 
        var request = context.Request;
        
        using var response = context.Response;
        var absoluteRequestUrl = request.Url!.AbsolutePath;

        var pathStaticFile = absoluteRequestUrl.Trim('/');
        
        if (absoluteRequestUrl!.Split('/')!.LastOrDefault()!.Contains('.'))
        {
            var extensionType = absoluteRequestUrl?.Split('/')?.LastOrDefault();
            
            extensionType = Path.GetExtension(extensionType)?.ToLower();

            if (File.Exists(pathStaticFile) && extensionType != null)
            {
                response.ContentType = DictionaryContentType.GetContentType(extensionType);
                using var fileStream = File.OpenRead(pathStaticFile);
                fileStream.CopyTo(response.OutputStream);   
            }
            else
            {
                using var fileStream = File.OpenRead(Path.Combine(_config.StaticFilesPath, "page404.html"));
                fileStream.CopyTo(response.OutputStream);
            }
        }
    }
}