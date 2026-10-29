using System.Diagnostics;
using Minty.Shell.UIManager;

namespace Minty.Shell.ProcessManager;

public static class PM
{
    public static async Task<int> ExecuteProcessAsync(string fileName, IEnumerable<string> arguments)
    {
        using Process process = new();

        process.StartInfo = new ProcessStartInfo
        {
            FileName = fileName,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = false,
            CreateNoWindow = true
        };

        foreach (string argument in arguments)
        {
            process.StartInfo.ArgumentList.Add(item: argument);
        }

        try
        {
            process.Start();

            Task outputTask = RedirectOutputAsync(reader: process.StandardOutput, writer: Console.Out);

            Task errorTask = RedirectOutputAsync(reader: process.StandardError, writer: Console.Error);

            await Task.WhenAll(outputTask, errorTask, process.WaitForExitAsync());

            return process.ExitCode;
        }
        catch (System.ComponentModel.Win32Exception ex)
        {
            if (ex.NativeErrorCode == 2)
            {
                UIM.WriteLine(message: $"Unknown command: {fileName}", textColor: ConsoleColor.Red);
            }
            else if (ex.NativeErrorCode == 5)
            {
                UIM.WriteLine(message: $"Access denied: {fileName}", textColor: ConsoleColor.Red);
            }
            else
            {
                Console.Error.WriteLine($"Unknown error in {fileName}: {ex.Message}");
            }

            return -1;
        }
    }

    public static async Task RedirectOutputAsync(StreamReader reader, TextWriter writer)
    {
        while (await reader.ReadLineAsync() is { } line)
        {
            await writer.WriteLineAsync(value: line);
        }
    }
}