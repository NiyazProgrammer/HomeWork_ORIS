namespace Templates;

class Program
{
    private static User user = new User();

    public Program()
    {
        
    }
    public static void Main()
    {
        user.FullName = "Niyaz";
        user.Adress = "Kazan, Pushkina 20 street";
        var templatesOne = TemplatesOne(user);
        Console.WriteLine();
    }

    public static string TemplatesOne(User user)
    {
        var str = "Здравствуйте, @{Program.user.FullName}. Вы уволены";
        return str.Replace("@{Program.user.FullName}", user.FullName);
    }

    public static string TemplatesTwo(User user)
    {
        var str = "Здравствуйте, @{model.FullName} проживающий по адресу @{model.Address}." +
                  " Ваша посылка дошла до пункта выдачи.";
        return str.Replace();
    }
}

