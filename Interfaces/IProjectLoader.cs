interface IProjectLoader
{
    public void LoadProject(string projectPath);
    public bool IsPythonProject(string projectPath);
    public string GetPythonActivateScript(string projectPath);
}
