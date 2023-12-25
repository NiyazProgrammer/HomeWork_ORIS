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
        // некоторая обработка запроса
        var request = context.Request;
        
        using var response = context.Response;
        var absoluteRequestUrl = request.Url!.AbsolutePath;
        if (request.HttpMethod == "POST")
        {
            var account = requestHandler.GetFormDataAsync(request);
        }
        // формирует путь убирая / в конце и в начале
        var pathStaticFile = absoluteRequestUrl.Trim('/');
        
        // проверка: разбивает на массив, берет последний элемент, проверяет на расширение - тут может быть null(!)
        if (absoluteRequestUrl!.Split('/')!.LastOrDefault()!.Contains('.'))
        {
            // берет последний элемент не null(?)
            var extensionType = absoluteRequestUrl?.Split('/')?.LastOrDefault();
            
            // здесь мы берем подстроку начиная с точки и до конца строки
            //pattern = pattern?[pattern.IndexOf('.')..];
            extensionType = Path.GetExtension(extensionType)?.ToLower();

            if (File.Exists(pathStaticFile) && extensionType != null)
            {
                response.ContentType = DictionaryContentType.GetContentType(extensionType);
                // используется using чтобы автоматически закрылся файл
                using var fileStream = File.OpenRead(pathStaticFile);
                // Копируем объект и отправляем клиенту
                fileStream.CopyTo(response.OutputStream);   
            }
            else
            {
                using var fileStream = File.OpenRead(Path.Combine(_config.StaticFilesPath, "page404.html"));
                fileStream.CopyTo(response.OutputStream);
            }
        }
        // передача запроса дальше по цепи при наличии в ней обработчиков
        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }
}