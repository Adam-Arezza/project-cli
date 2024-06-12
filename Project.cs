public class Project
{
    public string projectName {get; set;}

    public string projectPath {get; set;}

    public Project(string projectName, string projectPath)
    {
      this.projectName = projectName;
      this.projectPath = projectPath; 
    }
}
