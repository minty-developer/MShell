using System.Diagnostics;

namespace MShell;

public static class Program
{
    private static string currentDirectory = AppContext.BaseDirectory;
    private static bool AppRunning = true;

    public static void Main()
    {
        Console.Clear();
        Console.WriteLine(value: "(c) Minty Shell. All rights reserved");
        Console.WriteLine();

        Console.CancelKeyPress += (sender, e) => { e.Cancel = true; };

        while(AppRunning)
        {
            Console.Write(value: $"{GetDisplayPath(currentDirectory)}>> ");
            string? command = Console.ReadLine();
            if(string.IsNullOrWhiteSpace(value: command)) { continue; }
            string[] commands = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            switch(commands[0])
            {
                case "quit":
                case "exit":
                    AppRunning = false;
                    break;
                case "cls":
                case "clear":
                    Console.Clear();
                    Console.WriteLine(value: "(c) Minty Shell. All rights reserved");
                    Console.WriteLine();
                    break;
                case "calc":
                case "calculator":
                    Process.Start("cmd.exe", "/c calc");
                    break;
                case "move":
                    if(commands.Length == 1)
                    {
                        WriteResult(message: "To know about this command, type \"move [/?|--help|-h]");
                        continue;
                    }
                    if(commands[1] == "/?" || commands[1] == "--help" || commands[1] == "-h")
                    {
                        WriteResult(message: "move [option]\nmove [path]\n\nMove current directory\n\noption:\n    - /?, --help, -h: Show how to use this command");
                        continue;
                    }
                    if(!MoveToDirectory(commands[1]))
                    {
                        WriteResult(message: "Moving is not allowed. Is Directory Exsist?", textColor: ConsoleColor.Yellow);
                    }
                    break;
                case "file":
                    if(commands.Length == 1)
                    {
                        WriteResult(message: "To know about this command, type \"file [/?|--help|-h]\"");
                        continue;
                    }
                    switch(commands[1])
                    {
                        case "current":
                            WriteResult(message: currentDirectory);
                            break;
                        case "/?": case "--help": case "-h":
                            WriteResult(message: "file [option]\n\nControl Files\n\noption:\n    - current: Show this shells' current directory\n    - tree: Show folder & file tree that starts from current directory\n    - create: Create new file or directory\n    - edit: Edit file\n    - move: Move file to another directory\n    - remove: Remove file or directory\n    - /?, --help, -h: Show how to use this command.");
                            break;
                        case "tree":
                            WriteResult(message: GetFolderTree(currentDirectory));
                            break;
                        case "create":
                        case "remove":
                        case "move":
                        case "edit":
                            WriteResult(message: "It has not been implemented.", textColor: ConsoleColor.Yellow);
                            break;
                        default:
                            WriteResult(message: $"Unknow Option: {commands[1]}", textColor: ConsoleColor.Red);
                            break;
                    }
                    break;
                default:
                    WriteResult(message: $"Unknown Command: {commands[0]}", textColor: ConsoleColor.Red);
                    break;
            }
        }
    }

    private static string GetDisplayPath(string path, int keepFolders = 3)
    {
        string fullPath = Path.GetFullPath(path);
        string rootPath = Path.GetPathRoot(fullPath)!;

        string relativePath = Path.GetRelativePath(rootPath, fullPath);

        string[] folders = relativePath.Split(
            [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar],
            StringSplitOptions.RemoveEmptyEntries
        );

        if (folders.Length <= keepFolders)
            return fullPath;

        string shortenedPath = Path.Combine(
            rootPath,
            "...",
            Path.Combine([.. folders.Skip(folders.Length - keepFolders)])
        );

        return shortenedPath;
    }

    private static bool MoveToDirectory(string path)
    {
        string targetPath = Path.GetFullPath(Path.Join(currentDirectory, path));

        if (!Directory.Exists(targetPath))
            return false;

        currentDirectory = targetPath;
        return true;
    }

    private static string GetFolderTree(string path)
    {
        DirectoryInfo directoryInfo = new(path);
        DirectoryInfo[] directories = directoryInfo.GetDirectories();
        string treeString = string.Empty;
        if(directories.Length == 0) { return ""; }
        treeString += $"{Path.GetDirectoryName(path)}\n";
        for(int i = 0; i < directories.Length - 1; i++)
        {
            treeString += $"  ├─ {directories[i].Name}\n";
        }
        treeString += $"  └─ {directories[^1].Name}";
        return treeString;
    }

    private static void WriteResult(string message, ConsoleColor textColor = ConsoleColor.White)
    {
        Console.ForegroundColor = textColor;
        Console.WriteLine(message);
        Console.WriteLine();
        Console.ResetColor();
    }
}