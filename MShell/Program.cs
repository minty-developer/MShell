using System.Diagnostics;

using Minty.Shell.FileManager;
using Minty.Shell.ProcessManager;
using Minty.Shell.UIManager;

namespace Minty.Shell;

public static class Program
{
    private static bool AppRunning = true;

    public static async Task Main()
    {
        Console.Clear();
        UIM.WriteLine(message: "(c) Minty Shell. All rights reserved");
        UIM.WriteLine();

        Console.CancelKeyPress += (_, _) => { AppRunning = false; };

        while(AppRunning)
        {
            UIM.Write(message: $"{FM.GetDisplayPath(FM.CurrentDirectory)}>> ");
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
                    UIM.WriteLine(message: "(c) Minty Shell. All rights reserved");
                    UIM.WriteLine();
                    break;
                case "calc":
                case "calculator":
                    Process.Start("cmd.exe", "/c calc");
                    break;
                case "move":
                    if(commands.Length == 1)
                    {
                        UIM.WriteResult(message: "To know about this command, type \"move [/?|--help|-h]");
                        continue;
                    }
                    if(commands[1] == "/?" || commands[1] == "--help" || commands[1] == "-h")
                    {
                        UIM.WriteResult(message: "move [option]\nmove [path]\n\nMove current directory\n\noption:\n    - /?, --help, -h: Show how to use this command");
                        continue;
                    }
                    if(!FM.MoveToDirectory(commands[1]))
                    {
                        UIM.WriteResult(message: "Moving is not allowed. Is Directory Exsist?", textColor: ConsoleColor.Yellow);
                    }
                    break;
                case "file":
                    if(commands.Length == 1)
                    {
                        UIM.WriteResult(message: "To know about this command, type \"file [/?|--help|-h]\"");
                        continue;
                    }
                    switch(commands[1])
                    {
                        case "current":
                            UIM.WriteResult(message: FM.CurrentDirectory);
                            break;
                        case "/?": case "--help": case "-h":
                            UIM.WriteResult(message: "file [option]\n\nControl Files\n\noption:\n    - current: Show this shells' current directory\n    - tree: Show folder & file tree that starts from current directory\n    - list: Do file action like GUI\n    - create: Create new file or directory\n    - edit: Edit file\n    - move: Move file to another directory\n    - remove: Remove file or directory\n    - /?, --help, -h: Show how to use this command.");
                            break;
                        case "tree":
                            UIM.WriteResult(message: FM.GetFolderTree(FM.CurrentDirectory));
                            break;
                        case "create":
                        case "remove":
                        case "move":
                        case "list":
                        case "edit":
                            UIM.WriteResult(message: "It has not been implemented.", textColor: ConsoleColor.Yellow);
                            break;
                        default:
                            UIM.WriteResult(message: $"Unknown Option: {commands[1]}", textColor: ConsoleColor.Red);
                            break;
                    }
                    break;
                default:
                    await PM.ExecuteProcessAsync(commands[0], commands[1..]);
                    break;
            }
        }
    }
}