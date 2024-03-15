var _log = new ConversionLogger();
var _clock = Stopwatch.StartNew();

ToolSettings _cfg = (new ConfigurationBuilder()
  .AddJsonFile("settings.json", optional: true)
  .AddCommandLine(args)
  .Build()
  .Get<ToolSettings>() ?? new ToolSettings())
  .Ensure();

var vbProjFile = _cfg.ProjectFile ?? "My.vbproj";
var vbProjDir = Path.GetDirectoryName(vbProjFile)!;

MSBuildLocator.RegisterDefaults();
using var workspace = MSBuildWorkspace.Create();
var vbProj = await workspace.OpenProjectAsync(vbProjFile);
var vbDocs = vbProj.Documents.Where(x => x.Name.EndsWith(".vb"));

// Shuffle and batch documents
// Then process document batch in the order of ascending file size
await vbDocs.WorkEach(
  workers: _cfg.WorkerCount!.Value,
  order: f => f.FilePath != null ? FsFile.Measure(f.FilePath!) : 0,
  action: async vbDoc =>
{
  var vbName = FsPath.Relative(vbProjDir, vbDoc.FilePath!);
  var csPath = vbDoc.FilePath! + ".cs";

  // Delete if already converted
  if (_cfg.DeleteAlreadyConverted!.Value && FsFile.Test(csPath))
  {
    FsFile.Delete(csPath);
    _log.LogDelete(csPath);
  }

  // Ignore if already converted  (if it does not have errors)
  if (_cfg.IgnoreAlreadyConverted!.Value && FsFile.Test(csPath)
    && !Regex.IsMatch(FsFile.StringIn(csPath), "#error", RegexOptions.Multiline))
  {
    _log.LogIgnore(vbName);
    return;
  }
  _log.LogConvert(vbName);

  TimeSpan time = _clock.Elapsed;

  // Convert document to target code
  var result = await ProjectConversion.ConvertSingleAsync<VBToCSConversion>(vbDoc,
    new SingleConversionOptions { RootNamespaceOverride = _cfg.RootNamespace ?? null });

  string csOutput = result.ConvertedCode;

  if (_cfg.PostProcess.Enable.Value)
  {
    // Post process output code
    var csAst = CSharpSyntaxTree.ParseText(csOutput).GetCompilationUnitRoot();
    if (_cfg.PostProcess.RemoveNormalComments.Value || _cfg.PostProcess.RemoveDocumentationComments.Value)
    {
      var csTrivia = csAst.DescendantTrivia(x => true, true);
      csAst = csAst.ReplaceTrivia(csTrivia, (n0, n1) =>
      {
        if (_cfg.PostProcess.RemoveNormalComments.Value
        && (n0.IsKind(SyntaxKind.SingleLineCommentTrivia) || n0.IsKind(SyntaxKind.MultiLineCommentTrivia)))
        {
          return SyntaxFactory.CarriageReturn;
        }
        else if (_cfg.PostProcess.RemoveDocumentationComments.Value
          && (n0.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) || n0.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia)))
        {
          return SyntaxFactory.CarriageReturn;
        }
        else
          return n1;
      });
    }
    csOutput = csAst.NormalizeWhitespace(indentation: "   ").GetText().ToString();
  }

  TimeSpan duration = _clock.Elapsed - time;
  bool success = result.Success && !result.Exceptions.Any();

  if (success)
    _log.LogSuccess(vbName, duration);
  else
  {
    // Log conversion exception(s)
    string error = string.Join("\n", result.Exceptions
      .Select(x => Regex.Match(x, ".+:\\s*(.+)\n").Groups[1].Value));
    _log.LogFailure(vbName, error, duration);
  }

  // Possibly emit the converted code
  if (success || _cfg.EmitOnError!.Value)
    FsFile.StringOut(csPath, csOutput);
});
