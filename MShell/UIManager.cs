namespace Minty.Shell.UIManager;

public static class UIM
{
    public static void WriteLine(string message = "", ConsoleColor textColor = ConsoleColor.White)
    {
        Console.ForegroundColor = textColor;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void WriteLine(Exception exception, ConsoleColor textColor = ConsoleColor.White)
    {
        Console.ForegroundColor = textColor;
        Console.WriteLine(exception);
        Console.ResetColor();
    }

    public static void Write(string message = "", ConsoleColor textColor = ConsoleColor.White)
    {
        Console.ForegroundColor = textColor;
        Console.Write(message);
        Console.ResetColor();
    }
}