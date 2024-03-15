ToolSettings _cfg = (new ConfigurationBuilder()
  .AddJsonFile("settings.json", optional: true)
  .AddCommandLine(args)
  .Build().Get<ToolSettings>() ?? new ToolSettings())
  .Ensure();

MSBuildLocator.RegisterDefaults();
using var workspace = MSBuildWorkspace.Create();
var project = await workspace.OpenProjectAsync(_cfg.ProjectFile);
var documents = project.Documents.Where(x => x.Name.EndsWith(".vb"));

await documents.WorkEach(document =>
{
  string code = File.ReadAllText(document.FilePath);

  var ast = VisualBasicSyntaxTree.ParseText(code).GetCompilationUnitRoot();

  // Remove comments
  if (_cfg.RemoveNormalComments.Value || _cfg.RemoveDocumentationComments.Value)
  {
    var csTrivia = ast.DescendantTrivia(x => true, true);
    ast = ast.ReplaceTrivia(csTrivia, (n0, n1) =>
    {
      if (_cfg.RemoveNormalComments.Value
        && (n0.IsKind(SyntaxKind.CommentTrivia)))
      {
        return SyntaxFactory.CarriageReturn;
      }
      else if (_cfg.RemoveDocumentationComments.Value
        && (n0.IsKind(SyntaxKind.DocumentationCommentTrivia)))
      {
        return SyntaxFactory.CarriageReturn;
      }
      else
        return n1;
    });
  }

  string outputCode = ast.NormalizeWhitespace(indentation: _cfg.IndentToken).GetText().ToString();

  File.WriteAllText(document.FilePath, outputCode);
});
