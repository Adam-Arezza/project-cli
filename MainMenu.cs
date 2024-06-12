using InquirerCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


class MainMenu
{
    private Dictionary<string,Action> options;
    private string projectDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\projects";
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
                                                         "Select an option, if this is the first time running, select setup.", 
                                                         optionsArray);
        var inquiries = new Inquirer(optionsList);
        Console.ForegroundColor = ConsoleColor.Cyan;
        inquiries.Ask();
        int selection = int.Parse(optionsList.Answer()) - 1;
        Console.WriteLine(optionsArray[selection]);
        options[optionsArray[selection]].Invoke();
        Console.ResetColor();
    }

    public void Setup()
    {
        //setup
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
               Directory.Delete(this.projectDirectory);
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
        Console.WriteLine("adding new project");//add a project to the list
        string projName;
        string projPath;
        var projectNameInput = new InquirerCore.Prompts.Input("projectName", "Enter the name of the Project: ");
        var projectPathInput = new InquirerCore.Prompts.Input("projectPath", "Enter the path of the Project: ");
        var projectInputPrompts = new Inquirer(projectNameInput, projectPathInput);
        projectInputPrompts.Ask();
        projName = projectNameInput.Answer();
        projPath = projectPathInput.Answer();
        Console.WriteLine("The name is: {0}", projName);
        Console.WriteLine("The path is: {0}", projPath);
        string projectsJsonFile = this.projectDirectory + "\\projects.json";
        Project newProject = new Project(projName, projPath);

        if(!File.Exists(projectsJsonFile))
        {
            List<Project> projects = new List<Project>();
            projects.Add(newProject);
            string projectsJson = JsonSerializer.Serialize(projects);
            File.WriteAllText(projectsJsonFile, projectsJson);
            return;
        }

        else
        {
            string projectJson = File.ReadAllText(projectsJsonFile);
            Console.WriteLine(projectJson);
            List<Project>? projects = JsonSerializer.Deserialize<List<Project>>(projectJson);
            if(projects?.Count > 0)
            {
                projects.Add(newProject);
            }
            Console.WriteLine(projects);
            string projectsJson = JsonSerializer.Serialize(projects);
            File.WriteAllText(projectsJsonFile, projectsJson);
            return;
        }
    }

    public void RemoveProject()
    {
        Console.WriteLine("removing project");//remove a project from the list
    }

    public void ListProjects()
    {
        string projectsJsonFile = this.projectDirectory + "\\projects.json";
        Console.WriteLine("listing projects...");// print out all the projects
        string projectJson = File.ReadAllText(projectsJsonFile);
        Console.WriteLine(projectJson);
    }

    public void OpenProject()
    {
        Console.WriteLine("opening project...");//opens a project
    }
    public void setQuit()
    {
        quitFlag = true;
    }
}
