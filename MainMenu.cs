using InquirerCore;
using System;
using System.Collections.Generic;
using System.Linq;

class MainMenu
{
    private Dictionary<string,Action> options;
    public MainMenu()
    {
       options = new Dictionary<string, Action>
       {
           {"setup", this.Setup},
           {"add project", this.AddProject},
           {"remove project", this.RemoveProject},
           {"list projects", this.ListProjects},
           {"open project", this.OpenProject}
       }; 
    }

    public void ShowOptions()
    {
        List<string> optionNames = new List<string>(options.Keys);
        string[] optionsArray = optionNames.ToArray();
        var optionsList = new InquirerCore.Prompts.ListInput("option", 
                                                         "Select an option, if this is the first time running, select setup.", optionsArray);
        var inquiries = new Inquirer(optionsList);
        inquiries.Ask();
        int selection = int.Parse(optionsList.Answer()) - 1;
        Console.WriteLine(selection);
        Console.WriteLine(optionsArray[selection]);
    }

    public void Setup()
    {
        //setup
       Console.WriteLine("running setup...");
    }

    public void AddProject()
    {
        Console.WriteLine("adding new project");//add a project to the list
    }

    public void RemoveProject()
    {
        Console.WriteLine("removing project");//remove a project from the list
    }

    public void ListProjects()
    {
        Console.WriteLine("listing projects...");// print out all the projects
    }

    public void OpenProject()
    {
        Console.WriteLine("opening project...");//opens a project
    }
}
