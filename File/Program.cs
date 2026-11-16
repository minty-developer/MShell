using System.Text;

namespace Minty.Shell.File;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        if(args.Length < 2) {
            Console.WriteLine("To know about this command, type \"file [/?|--help|-h]\"");
            return;
        }
        var currentDirectory = Path.GetFullPath(args[0]);
        if(!Directory.Exists(currentDirectory)) return;

        var mode = args[1].ToLower();

        switch (mode)
        {
            case "/?":
            case "--help":
            case "-h":
                Console.WriteLine("Usage:");
                Console.WriteLine("    file [option]");
                Console.WriteLine();
                Console.WriteLine("Manage Files");
                Console.WriteLine();
                Console.WriteLine("option:");
                Console.WriteLine("    - current: Show this shells' current directory");
                Console.WriteLine("    - tree: Show folder & file tree that starts from current directory");
                Console.WriteLine("    - list: Do file action like GUI");
                Console.WriteLine("    - create: Create new file or directory");
                Console.WriteLine("    - edit: Edit file");
                Console.WriteLine("    - move: Move file to another directory");
                Console.WriteLine("    - remove: Remove file or directory");
                Console.WriteLine("    - /?, --help, -h: Show how to use this command.");
                break;
            case "tree":
                Console.WriteLine(GetFolderTree(currentDirectory));
                break;
            case "create":
            case "remove":
            case "move":
            case "list":
            case "edit":
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("It has not been implemented.");
                Console.ResetColor();
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Unknown Option: {mode}");
                Console.ResetColor();
                break;
        }
    }

    private static string GetFolderTree(
        string path,
        string indent = "",
        bool isLast = true
    )
    {
        DirectoryInfo directoryInfo = new(path);
        DirectoryInfo[] directories = directoryInfo.GetDirectories();

        string treeString = string.Empty;

        treeString += $"{indent}{(isLast ? "└─ " : "├─ ")}{directoryInfo.Name}\n";

        string nextIndent = indent + (isLast ? "   " : "│  ");

        for (int i = 0; i < directories.Length; i++)
        {
            treeString += GetFolderTree(
                directories[i].FullName,
                nextIndent,
                i == directories.Length - 1
            );
        }

        return treeString;
    }
}