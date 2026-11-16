using System.Diagnostics;
using System.Text;
using Minty.Shell.FileManager;
using Minty.Shell.ProcessManager;
using Minty.Shell.UIManager;

namespace Minty.Shell;

public static class Program
{
    private static readonly string VERSION = "v1.1.0";
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        try
        {
            // [Ctrl] + [C] 대처용
            CancellationTokenSource ctx = new();

            Console.Clear();
            DrawMainContent();

            Console.CancelKeyPress += (_, _) => { ctx.Cancel(); };

            while(true)
            {
                UIM.WriteLine(message: $"{FM.GetDisplayPath(FM.CurrentDirectory)}");
                UIM.Write(message: ">> ", textColor: ConsoleColor.Blue);
                string? command = Console.ReadLine();
                Console.WriteLine();

                if(string.IsNullOrWhiteSpace(value: command)) { continue; }

                string[] commands = command.Split(separator: ' ', options: StringSplitOptions.RemoveEmptyEntries);
                switch(commands[0].ToLower())
                {
                    case "quit":
                    case "exit":
                        return;
                    case "help":
                        DrawHelpContent();
                        break;
                    case "echo":
                        UIM.WriteLine(commands[1]);
                        break;
                    case "cls":
                    case "clear":
                        Console.Clear();
                        DrawMainContent();
                        break;
                    case "calc":
                    case "calculator":
                        Process.Start(fileName: "cmd.exe", arguments: "/c calc");
                        break;
                    case "note":
                    case "notepad":
                        Process.Start(fileName: "cmd.exe", arguments: "/c notepad");
                        break;
                    case "file":
                        await PM.ExecuteProcessAsync(fileName: Path.Join(AppContext.BaseDirectory, "../../../../File/bin/Debug/net10.0/File.exe"), arguments: [FM.CurrentDirectory, .. commands[1..]]);
                        Console.WriteLine();
                        break;
                    case "process":
                        await PM.ExecuteProcessAsync(fileName: Path.Join(AppContext.BaseDirectory, "../../../../Process/bin/Debug/net10.0/Process.exe"), arguments: [.. commands[1..]]);
                        Console.WriteLine();
                        break;
                    case "environment":
                    case "env":
                        ProcessEnv(commands);
                        break;
                    case "dev":
                        await PM.ExecuteProcessAsync(fileName: "../Dev/bin/Debug/net10.0/Dev.exe", arguments: [FM.CurrentDirectory, .. commands[1..]]);
                        break;

                    // Windows 기본 명령어 차단 (하드코딩은 신이다)
                    case "assoc":
                    case "attrib":
                    case "break":
                    case "bcdedit":
                    case "cacls":
                    case "call":
                    case "cd":
                    case "chcp":
                    case "chdir":
                    case "chkdisk":
                    case "cmd":
                    case "color":
                    case "comp":
                    case "compact":
                    case "convert":
                    case "copy":
                    case "date":
                    case "del":
                    case "dir":
                    case "diskpart":
                    case "doskey":
                    case "driverquery":
                    case "endlocal":
                    case "erase":
                    case "fc":
                    case "find":
                    case "findstr":
                    case "for":
                    case "format":
                    case "fsutil":
                    case "ftype":
                    case "goto":
                    case "gpresult":
                    case "icacls":
                    case "if":
                    case "label":
                    case "md":
                    case "mkdir":
                    case "mklink":
                    case "mode":
                    case "more":
                    case "move":
                    case "openfiles":
                    case "path":
                    case "pause":
                    case "pord":
                    case "print":
                    case "prompt":
                    case "rd":
                    case "recover":
                    case "rem":
                    case "ren":
                    case "rename":
                    case "rmdir":
                    case "robocopy":
                    case "set":
                    case "setlocal":
                    case "sc":
                    case "schtasks":
                    case "shift":
                    case "shutdown":
                    case "sort":
                    case "start":
                    case "subst":
                    case "systeminfo":
                    case "tasklist":
                    case "taskkill":
                    case "time":
                    case "title":
                    case "tree":
                    case "type":
                    case "ver":
                    case "verify":
                    case "vol":
                    case "xcopy":
                    case "wmic":
                        break;

                    // 외부 프로세스 실행 시 (임시, 삭제 예정)
                    default:
                        await PM.ExecuteProcessAsync(fileName: commands[0], arguments: commands[1..]);
                        break;
                }
            }
        } catch(OperationCanceledException)
        {
            UIM.WriteLine(message: "Shell Exited.", textColor: ConsoleColor.Yellow);
            return;
        } catch (Exception ex)
        {
            UIM.WriteLine(exception: ex, textColor: ConsoleColor.Red);
        } finally
        {
            Console.ResetColor();
        }
    }

    private static void DrawMainContent()
    {
        UIM.WriteLine(message: $">_ Minty Shell {VERSION}", textColor: ConsoleColor.Blue);
        UIM.WriteLine(message: $"     (c) Minty Shell. All rights reserved", textColor: ConsoleColor.DarkGray);
        UIM.WriteLine();
    }

    private static void DrawHelpContent()
    {
        UIM.WriteLine(message: $">_ Minty Shell {VERSION}");
        UIM.WriteLine(message: "    (c) Minty Shell. All rights reserved");
        UIM.WriteLine();
        UIM.WriteLine(message: "Usage:");
        UIM.WriteLine(message: "    [command] [arguments...]");
        UIM.WriteLine();
        UIM.WriteLine(message: "Commands:");
        UIM.WriteLine();
        UIM.WriteLine(message: "    help                 Show this help message.");
        UIM.WriteLine(message: "    quit, exit           Exit Minty Shell.");
        UIM.WriteLine(message: "    echo                 Print a message.");
        UIM.WriteLine(message: "    cls, clear            Clear the screen.");
        UIM.WriteLine(message: "    calc, calculator      Open Calculator.");
        UIM.WriteLine(message: "    note, notepad         Open Notepad.");
        UIM.WriteLine();
        UIM.WriteLine(message: "    file                  Manage files.");
        UIM.WriteLine(message: "    process               Manage processes.");
        UIM.WriteLine(message: "    environment, env      Manage Shell environment.");
        UIM.WriteLine();
        UIM.WriteLine(message: "For more information about a command:");
        UIM.WriteLine(message: "    [command] /?");
    }

    private static void ProcessEnv(string[] args)
    {
        if(args.Length == 1)
        {
            UIM.WriteLine(message: "To know about this command, type \"env [/?|--help|-h]\"");
            return;
        }
        switch (args[1])
        {
            case "/?":
            case "--help":
            case "-h":
                Console.WriteLine("env [mode] [options...]");
                Console.WriteLine();
                Console.WriteLine("Control Files");
                Console.WriteLine();
                Console.WriteLine("mode:");
                Console.WriteLine("    - move, currentDirectory-move: Move this shells' current directory");
                Console.WriteLine("    - show, currentDirectory-show: Move this shells' current directory");
                Console.WriteLine("    - /?, --help, -h: Show how to use this command.");
                break;
            case "currentDirectory-move":
            case "move":
                if(args.Length == 2)
                {
                    UIM.WriteLine(message: "To know about this command, type \"move [/?|--help|-h]");
                    break;
                }
                if(args[2] == "/?" || args[2] == "--help" || args[2] == "-h")
                {
                    UIM.WriteLine(message: "move [option]\nmove [path]\n\nMove current directory\n\noption:\n    - /?, --help, -h: Show how to use this command");
                    break;
                }
                if(!FM.MoveToDirectory(args[2]))
                {
                    UIM.WriteLine(message: "Moving is not allowed. Is Directory Exsist?", textColor: ConsoleColor.Yellow);
                }
                break;
            case "currentDirectory-show":
            case "current":
                UIM.WriteLine(message: FM.CurrentDirectory);
                break;
            case "version":
            case "ver":
                UIM.WriteLine(message: VERSION);
                break;
            default:
                UIM.WriteLine(message: $"Unknown Option: {args[1]}", textColor: ConsoleColor.Red);
                break;
        }
    }
}