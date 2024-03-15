namespace Tool;

internal class ToolSettings
{
  public string? SolutionFile { get; set; }
  public string? ProjectFile { get; set; }
  public string? RootNamespace { get; set; }
  public int? WorkerCount { get; set; }
  public bool? DeleteAlreadyConverted { get; set; }
  public bool? IgnoreAlreadyConverted { get; set; }
  public PostProcessSettings PostProcess { get; set; }
  public bool? EmitOnError { get; set; }

  public ToolSettings Ensure()
  {
    SolutionFile ??= "My.sln";
    ProjectFile ??= "My.vbproj";
    RootNamespace ??= "MyNamespace";
    DeleteAlreadyConverted ??= false;
    IgnoreAlreadyConverted ??= true;
    WorkerCount ??= Environment.ProcessorCount;
    PostProcess = (PostProcess ?? new PostProcessSettings()).Ensure();
    EmitOnError ??= true;
    return this;
  }
}

internal class PostProcessSettings
{
  public bool? Enable { get; set; }
  public bool? RemoveNormalComments { get; set; }
  public bool? RemoveDocumentationComments { get; set; }
  public string IndentToken { get; set; }

  public PostProcessSettings Ensure()
  {
    Enable ??= true;
    RemoveNormalComments ??= false;
    RemoveDocumentationComments ??= false;
    IndentToken ??= "   ";
    return this;
  }
}
