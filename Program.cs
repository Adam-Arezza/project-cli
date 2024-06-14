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
        Console.WriteLine("Select an option, if this is the first time running, select setup.");
       while(!mainMenu.quitFlag)
       {
           mainMenu.ShowOptions();
       }
    }
}

