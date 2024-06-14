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

        string command = $"cd {projectPath}; nvim .";
        if(IsPythonProject(projectPath))
        {
            string activateScript = GetPythonActivateScript(projectPath);
            command += $"; {activateScript}";
        }

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

    public bool IsPythonProject(string projPath)
    {
        bool pyProject = false;
        string[] virtualEnvDirs = {"venv", ".venv", "env"};
        foreach(string dir in virtualEnvDirs)
        {
            if(Directory.Exists(projPath + dir))
            {
                pyProject = true;
            }
        }

        foreach(string fileName in Directory.GetFiles(projPath))
        {
            if(fileName.Split(".")[1] == "py")
            {
                pyProject = true;
            }
        }

        return pyProject;
    }

    public string GetPythonActivateScript(string projPath)
    {
        string activateScript = "";        
        foreach(string directory in Directory.GetDirectories(projPath))
        {
            foreach(string innerDir in Directory.GetDirectories(directory))
            {
                if(innerDir == directory + "\\Scripts")
                {
                    Console.WriteLine(innerDir);
                    Console.WriteLine("Found scripts directory");
                    string virtualEnvDir = Path.GetFileName(directory);

                    activateScript = ".\\"+ virtualEnvDir + "\\Scripts\\activate";         
                    break;
                }
            }
        }
        return activateScript;
    }
}
