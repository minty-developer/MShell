using System.Text;

namespace Minty.Shell.Process;

public static class Program
{
    public static void Main(string[] args)
    {
        if(args.Length < 1) {
            Console.WriteLine("To know about this command, type \"process [/?|--help|-h]\"");
            return;
        }

        System.Diagnostics.Process[] processes = [];
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        var mode = args[0].ToLower();

        switch (mode)
        {
            case "/?":
            case "--help":
            case "-h":
                Console.WriteLine("process [option...]");
                Console.WriteLine();
                Console.WriteLine("Control Processes");
                Console.WriteLine();
                Console.WriteLine("option:");
                Console.WriteLine("    - list: Show Process List");
                Console.WriteLine("    - kill: Kill Process");
                Console.WriteLine("    - /?, --help, -h: Show how to use this command.");
                break;
            case "list":
                UpdateProcess(ref processes);
                try
                {
                    Console.WriteLine("╔════════════════════════════════════════════════════╤════════════╗");
                    Console.WriteLine("║ Process Name                                       │ Process Id ║");
                    Console.WriteLine("╠════════════════════════════════════════════════════╪════════════╣");
                    string beforeProcess = string.Empty;
                    for(int i = 0; i < processes.Length; i++)
                    {
                        System.Diagnostics.Process process = processes[i];
                        if(process.ProcessName == beforeProcess)
                        {
                            Console.WriteLine($"║ {"",-50} │ {process.Id,-10} ║");
                        }
                        else
                        {
                            if(i != 0)
                                Console.WriteLine("╟────────────────────────────────────────────────────┼────────────╢");
                            Console.WriteLine($"║ {process.ProcessName,-50} │ {process.Id,-10} ║");
                        }
                        beforeProcess = process.ProcessName;
                        process.Dispose();
                    }
                    Console.WriteLine("╚════════════════════════════════════════════════════╧════════════╝");
                } catch(InvalidOperationException) { }
                break;
            case "kill":
                if(args.Length < 2) return;
                UpdateProcess(ref processes);
                if(int.TryParse(args[1], out int PID))
                {
                    var process = System.Diagnostics.Process.GetProcessById(PID);
                    process.Kill();
                    process.WaitForExit();
                    Console.WriteLine($"Killing Success. id: {PID}, name: {process.ProcessName}");
                    process.Dispose();
                } else
                {
                    var process = System.Diagnostics.Process.GetProcessesByName(args[1]);
                    foreach(var p in process)
                    {
                        p.Kill();
                        p.WaitForExit();
                        Console.WriteLine($"Killing Success. id: {p.Id}, name: {args[1]}");
                        p.Dispose();
                    }
                }
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Unknown Option: {mode}");
                Console.ResetColor();
                break;
        }
    }

    private static void UpdateProcess(ref System.Diagnostics.Process[] processes)
    {
        processes = [.. System.Diagnostics.Process.GetProcesses()
        .OrderBy(process => process.ProcessName)
        .ThenBy(process => process.Id)];
    }
}