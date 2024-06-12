// See https://aka.ms/new-console-template for more information
using System;
using Figgle;


class Program
{
    static void Main(string[] args)
    {
        MainMenu mainMenu = new MainMenu();
        mainMenu.quitFlag = false;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(FiggleFonts.Slant.Render("Projects - CLI"));
        Console.ResetColor();
       while(!mainMenu.quitFlag)
       {
           mainMenu.ShowOptions();
       }
    }
}


//var menuOptions = new string[] {"test1", "test2"};
//var nameInput = new InquirerCore.Prompts.Input("name", "what is your name?");
//var mainMenu = new InquirerCore.Prompts.ListInput("option", "select an option",menuOptions);
//var inq = new Inquirer(nameInput, mainMenu);
////inq.Ask();
//
//string PROJECTS = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
//PROJECTS = PROJECTS + "\\projects_cli";
//bool quitFlag = false;
//Console.WriteLine(PROJECTS);
//
//
//public void setup(){
//    // if there is no projects path, create it
//    // give the option to rerun the setup
//    // if yes delete the original and recreate
//}
//
//public void listProjects(){
//    //read the projects file
//    //save the json contents into a variable
//    //print the projects
//}
//
//public void addProject(){
//    //prompt for the project name
//    //prompt for the project path
//    //create an empty array to store project information
//    //if there is no projects.json file, create it
//    //create a new projects object
//    //
//}

