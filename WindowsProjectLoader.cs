using System;
using System.IO;
using System.Diagnostics;

public class WindowsProjectLoader:IProjectLoader
{
    public void LoadProject(string projectPath)
    {
        if(!Directory.Exists(projectPath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Could not find the project at path: ");
            Console.WriteLine($"{projectPath}");
            Console.ResetColor();
            return;
        }

        string command = $"cd {projectPath}; nvim .; .\\env\\Scripts\\activate";

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-NoExit -Command \"{command}\"",
            UseShellExecute = false,
            CreateNoWindow = false
        };

        using (Process process = new Process())
        {
            process.StartInfo = startInfo;
            process.Start();
            Environment.Exit(0);
        }
    }
}
