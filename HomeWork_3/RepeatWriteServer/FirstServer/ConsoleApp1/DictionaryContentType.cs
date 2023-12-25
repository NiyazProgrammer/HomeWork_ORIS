

using System.Diagnostics;

namespace ConsoleApp1;

public class DictionaryContentType
{
    private static Dictionary<string, string> _dictionaryType = new Dictionary<string, string>
    {
        [".png"] = "image/png",
        [".css"] = "text/css",
        [".svg"] = "image/svg+xml",
        [".html"] = "text/html",
        [".jpg"] = "image/jpeg",
        [".js"] = "script/js"
    };
    
    public static string GetContentType(string extension)
    {
        return _dictionaryType[extension];
    }
}