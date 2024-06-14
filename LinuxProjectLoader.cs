using System;
using System.IO;
using System.Diagnostics;

class LinuxProjectLoader: IProjectLoader, IPythonProjectCheck
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

        string command = $"-- bash -c 'nvim .;exec bash'";
        if(IsPythonProject(projectPath))
        {
            string activateScript = GetPythonActivateScript(projectPath);
            command = $"";
            //command += $"; {activateScript}";
        }

        //ubuntu 
        //gnome-terminal --working-directory={projectPath} -- bash -c "nvim .; exec bash"
        //gnome-terminal --working-directory={projectPath} -- bash -c "source {env script};nvim .; exec bash"

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "gnome-terminal",
            Arguments = $"--working-directory={projectPath}\"{command}\"",
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
                if(innerDir == directory + "/bin")
                {
                    string virtualEnvDir = Path.GetFileName(directory);
                    activateScript = virtualEnvDir + "/bin/activate";         
                    break;
                }
            }
        }
        return activateScript;
    }
}
