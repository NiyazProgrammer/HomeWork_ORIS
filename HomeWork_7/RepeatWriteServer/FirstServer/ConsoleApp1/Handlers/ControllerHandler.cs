using System.Net;
using System.Reflection;
using System.Text;
using ConsoleApp1;
using HttpServerBattleNet.Attribuets;
using HttpServerBattleNet.Model;
using Newtonsoft.Json;

namespace HttpServerBattleNet.Handler;

public class ControllerHandler : Handler
{
    public override void HandleRequest(HttpListenerContext context)
    {
        try
        {
            // /authorize/getemaillist - ["authorize","getemaillist"]
            var strParams = context?.Request.Url!
                .Segments
                .Skip(1)
                .Select(s => s.Replace("/", ""))
                .ToArray();
            
            if (strParams!.Length < 2)
                throw new ArgumentNullException("the number of lines in the query string is less than two!");
        
            var controllerName = strParams[0]; 
            var methodName = strParams[1];  
            
            var assembly = Assembly.GetExecutingAssembly();
            
            var controller = assembly.GetTypes()
                .Where(t => Attribute.IsDefined(t, typeof(ControllerAttribute)))
                .FirstOrDefault(c => ((ControllerAttribute)Attribute.GetCustomAttribute(c, typeof(ControllerAttribute))!)
                    .ControllerName.Equals(controllerName, StringComparison.OrdinalIgnoreCase));
            
            var list = controller?.GetMethods()
                .Select(method => new { Name = method.Name, Attributes = method.GetCustomAttributes()});
            
            var method = (controller?.GetMethods())
                .FirstOrDefault(x => x.GetCustomAttributes(true)
                    .Any(attr => attr.GetType().Name.Equals($"{context.Request.HttpMethod}Attribute",
                                     StringComparison.OrdinalIgnoreCase) 
                                 && ((HttpMethodAttribute)attr).ActionName.Equals(methodName, StringComparison.OrdinalIgnoreCase)));
            
            switch (methodName.ToLower())
            {
                case "getemaillist":
                {
                    var result = method?.Invoke(Activator.CreateInstance(controller), null);
                    byte[] buffer = Encoding.UTF8.GetBytes((string)result);
                    PullAnswer(context,buffer);
                    break;
                }
                case "getaccountslist":
                {
                    
                    var result = method?.Invoke(Activator.CreateInstance(controller), null);
                    var accounts = result;
                   
                    string json2 = JsonConvert.SerializeObject(accounts, Formatting.Indented);
                    
                    result = (object)$"<html><head> <meta charset=\"utf-8\"></head><body>{json2}<h1></h1></body></html>";
                    byte[] buffer = Encoding.UTF8.GetBytes((string)result);
                    PullAnswer(context,buffer);
                    break;
                }
                case "sendemail":
                {
                    var request = new RequestHandler();
                    var data = request.GetFormDataAsync(context.Request);
                    var account = ListAccounts.Accounts[ListAccounts.Accounts.Count - 1];
                    var result = method?.Invoke(Activator.CreateInstance(controller), new object[] { $"{account.Email}", $"{account.Password}"});
                    byte[] buffer = Encoding.UTF8.GetBytes((string)"<html><head> <meta charset=\"utf-8\"></head><body>Сообщение отправлено<h1></h1></body></html>");
                    PullAnswer(context,buffer);
                    break;
                }
            }
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine(e);
        }
    }

    private void PullAnswer(HttpListenerContext context, byte[] buffer)
    {
        context.Response.ContentLength64 = buffer.Length;
        using Stream output = context.Response.OutputStream;
        output.Write(buffer, 0,buffer.Length);
        output.Flush();
    }
}