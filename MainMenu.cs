using InquirerCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Runtime.InteropServices;


class MainMenu
{
    private Dictionary<string,Action> options;
    private string projectDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\projects";
    private string projectJsonFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\projects\\projects.json";
    public bool quitFlag
    {get;set;}

    public MainMenu()
    {
       options = new Dictionary<string, Action>
       {
           {"setup", this.Setup},
           {"add project", this.AddProject},
           {"remove project", this.RemoveProject},
           {"list projects", this.ListProjects},
           {"open project", this.OpenProject},
           {"quit", this.setQuit}
       }; 
    }

    public void ShowOptions()
    {
        List<string> optionNames = new List<string>(options.Keys);
        string[] optionsArray = optionNames.ToArray();
        var optionsList = new InquirerCore.Prompts.ListInput("option", 
                                                         "", 
                                                         optionsArray);
        var inquiries = new Inquirer(optionsList);
        Console.ForegroundColor = ConsoleColor.Cyan;
        inquiries.Ask();
        int selection = int.Parse(optionsList.Answer()) - 1;
        Console.ResetColor();
        Console.WriteLine(optionsArray[selection]);
        options[optionsArray[selection]].Invoke();
    }

    public void Setup()
  {
       Console.WriteLine("running setup...");
       bool dirExists = Directory.Exists(this.projectDirectory);
       if(!dirExists)
       {
           Console.WriteLine("no project directory exists, one will be created...");
           Directory.CreateDirectory(this.projectDirectory);
       }
       else
       {
           Console.WriteLine("Found a projects directory");
           string[] overWriteOptions = {"yes", "no"};
           var recreateProjectDir = new InquirerCore.Prompts.ListInput("option","Do you want to overwrite it? This will remove any projects being tracked.", overWriteOptions);
           var overWrite = new Inquirer(recreateProjectDir);
           overWrite.Ask();
           int selection = int.Parse(recreateProjectDir.Answer()) - 1;
           Console.WriteLine("You selected: {0}", overWriteOptions[selection]);
           if(overWriteOptions[selection] == "yes")
           {
               Console.WriteLine("Removing the old project directory...");
               Directory.Delete(this.projectDirectory, true);
               Directory.CreateDirectory(this.projectDirectory);
           }
           else
           {
               Console.WriteLine("Keeping project list.");
               return;
           }
       }
    }

    public void AddProject()
    {   
        Console.WriteLine("adding new project");
        string projName;
        string projPath;
        var projectNameInput = new InquirerCore.Prompts.Input("projectName", "Enter the name of the Project: ");
        var projectPathInput = new InquirerCore.Prompts.Input("projectPath", "Enter the path of the Project: ");
        var projectInputPrompts = new Inquirer(projectNameInput, projectPathInput);
        projectInputPrompts.Ask();
        projName = projectNameInput.Answer();
        projPath = projectPathInput.Answer();

        //validate project path
         
        Console.WriteLine("The name is: {0}", projName);
        Console.WriteLine("The path is: {0}", projPath);
        Project newProject = new Project(projName, projPath);
        if(!File.Exists(this.projectJsonFile))
        {
            List<Project> projects = new List<Project>();
            projects.Add(newProject);
            string projectsJson = JsonSerializer.Serialize(projects);
            File.WriteAllText(this.projectJsonFile, projectsJson);
            return;
        }

        else
        {
            var projects = GetProjectsListFromFile();
            projects.Add(newProject);
            Console.WriteLine(projects);
            string projectsJson = JsonSerializer.Serialize(projects);
            File.WriteAllText(this.projectJsonFile, projectsJson);
            return;
        }
    }

    public void RemoveProject()
    {
        Console.WriteLine("removing project");
        var projects = GetProjectsListFromFile();
        int projectToRemoveIdx = GetProjectIndex(projects);
        projects.RemoveAt(projectToRemoveIdx);
        string projectsJson = JsonSerializer.Serialize(projects);
        File.WriteAllText(this.projectJsonFile, projectsJson);
    }

    public void ListProjects()
    {
        var projects = GetProjectsListFromFile();
        Console.ForegroundColor = ConsoleColor.Yellow;
        projects.ForEach(p => Console.WriteLine(p.projectName));
        Console.ResetColor();
    }

    public void OpenProject()
    {   
        var projects = GetProjectsListFromFile();
        int projectToOpenIdx = GetProjectIndex(projects);
        string projectPath = projects[projectToOpenIdx].projectPath;

        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            WindowsProjectLoader projectLoader = new WindowsProjectLoader();
            projectLoader.LoadProject(projectPath);
        }
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            UnixProjectLoader projectLoader = new UnixProjectLoader();
            projectLoader.LoadProject(projectPath);
        }
        Console.WriteLine("opening project...");
    }


    private List<Project> GetProjectsListFromFile()
    {
        string projectJson = File.ReadAllText(this.projectJsonFile);
        List<Project> projects = JsonSerializer.Deserialize<List<Project>>(projectJson)?? throw new Exception("Could not read JSON file or it's contents.");
        return projects;
    }

    private int GetProjectIndex(List<Project> projects)
    {
        string[] projectArray = projects.Select(x => x.projectName).ToArray();
        Console.ForegroundColor = ConsoleColor.Yellow;
        var projectSelectList = new InquirerCore.Prompts.ListInput("projectList", "Select project to remove: ", projectArray);
        var selectProject = new Inquirer(projectSelectList);
        selectProject.Ask();
        int projectIndex = int.Parse(projectSelectList.Answer()) - 1;
        Console.ResetColor();
        return projectIndex;
    }

    public void setQuit()
    {
        quitFlag = true;
    }
}
