namespace Tool;

internal class ToolSettings
{
  public string? SolutionFile { get; set; }
  public string? ProjectFile { get; set; }
  public bool? RemoveNormalComments { get; set; }
  public bool? RemoveDocumentationComments { get; set; }
  public string IndentToken { get; set; }

  public ToolSettings Ensure()
  {
    SolutionFile ??= "My.sln";
    ProjectFile ??= "My.csproj";
    RemoveNormalComments ??= false;
    RemoveDocumentationComments ??= false;
    IndentToken ??= "   ";
    return this;
  }
}
