

using System.Diagnostics;

namespace ConsoleApp1;

public class DictionaryContentType
{
    private static Dictionary<string, string> _dictionaryType = new Dictionary<string, string>
    {
        { ".svg","image/svg+xml"},
        { ".html", "text/html" },
        { ".png", "image/png" },
    };
    public static string GetContentType(string filePath)
    {
        string fileExtension = Path.GetExtension(filePath).ToLower();

        if (_dictionaryType.TryGetValue(fileExtension, out string contentType))
        {
            return contentType;
        }
        else
        {
            return null;
        }
    }
}