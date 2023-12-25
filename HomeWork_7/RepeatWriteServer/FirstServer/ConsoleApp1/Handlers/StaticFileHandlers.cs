using System.Net;
using ConsoleApp1;
using ConsoleApp1.Configuration;

namespace HttpServerBattleNet.Handler;
 
public class StaticFileHandlers : Handler
{
    private AppSettings _config = ConfigManager.GetConfig();
    private RequestHandler requestHandler = new RequestHandler();
        
    public override void HandleRequest(HttpListenerContext context)
    {
        
        var request = context.Request;
        
        using var response = context.Response;
        var absoluteRequestUrl = request.Url!.AbsolutePath;
        if (request.HttpMethod == "POST")
        {
            var account = requestHandler.GetFormDataAsync(request);
            ListAccounts.Accounts.Add(account.Result);
        }
        var pathStaticFile = Path.Combine(_config.StaticFilesPath, absoluteRequestUrl.Trim('/'));
        
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
        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }
}