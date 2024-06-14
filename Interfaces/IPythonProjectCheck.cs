interface IPythonProjectCheck
{
    public bool IsPythonProject(string projectPath);
    public string GetPythonActivateScript(string projectPath);
}
