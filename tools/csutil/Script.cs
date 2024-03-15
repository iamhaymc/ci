//using Microsoft.CodeAnalysis.CSharp.Scripting;
//using Microsoft.CodeAnalysis.Scripting;

//namespace My.Utility;

//#region [Engines]

//public interface IScriptEngine<TCtx>
//  where TCtx : class, IScriptContext, new()
//{
//  IScriptEngine<TCtx> LoadLibrary();
//  IScriptEngine<TCtx> LoadAssembly(params Assembly[] assemblies);
//  IScriptEngine<TCtx> UseNamespace(params string[] names);
//  IScriptEngine<TCtx> BindLogger(ILogger? Logger = null);
//  IScriptEngine<TCtx> BindContext(TCtx? context = null);
//  Task<object> Evaluate(string code);
//}

//public class ScriptEngine<TCtx> : IScriptEngine<TCtx>
//  where TCtx : class, IScriptContext, new()
//{
//  private ILogger? _logger;
//  private ScriptOptions _options;
//  private TCtx? _api;

//  public ScriptEngine(ILogger? logger = null)
//  {
//    _options = ScriptOptions.Default;
//    BindLogger(logger);
//  }

//  public virtual IScriptEngine<TCtx> LoadLibrary()
//      => UseNamespace(
//        "System.Collections.Concurrent",
//        "System.Collections.Generic",
//        "System.Collections.Specialized",
//        "System.Collections",
//        "System.Globalization",
//        "System.IO",
//        "System.Linq.Expressions",
//        "System.Linq",
//        "System.Numerics",
//        "System.Reflection",
//        "System.Runtime.InteropServices",
//        "System.Security.Cryptography",
//        "System.Text.Json.Nodes",
//        "System.Text.Json.Serialization",
//        "System.Text.Json",
//        "System.Text.RegularExpressions",
//        "System.Text",
//        "System.Threading.Tasks",
//        "System")
//      .LoadAssembly(GetType().Assembly)
//      .UseNamespace(GetType().Assembly.GetName().Name!);

//  public IScriptEngine<TCtx> LoadAssembly(params Assembly[] assemblies)
//  {
//    foreach (Assembly assembly in assemblies)
//      _options = _options.AddReferences(assembly);
//    return this;
//  }

//  public IScriptEngine<TCtx> UseNamespace(params string[] names)
//  {
//    foreach (string name in names)
//      _options = _options.AddImports(name);
//    return this;
//  }

//  public IScriptEngine<TCtx> BindLogger(ILogger? logger = null)
//  {
//    _logger = logger;
//    _api?.UseLogger(_logger);
//    return this;
//  }
//  public IScriptEngine<TCtx> BindContext(TCtx? context = null)
//  {
//    _api = context ?? (TCtx)(new TCtx().UseLogger(_logger));
//    return this;
//  }

//  public Task<object> Evaluate(string code)
//    => CSharpScript.EvaluateAsync(code, _options, _api, _api?.GetType());
//}

//#endregion

//#region [Contexts]

//public interface IScriptContext
//{
//  IScriptContext UseLogger(ILogger? Logger);
//  void LogTrace(string arg);
//  void LogDebug(string arg);
//  void LogInfo(string arg);
//  void LogWarn(string arg);
//  void LogError(string arg);
//}

//public class ScriptContext : IScriptContext
//{
//  private string[]? _args;
//  private ILogger? _logger;

//  public ScriptContext() { }
//  public ScriptContext(ILogger logger, string[]? args = null)
//    => UseArguments(args)
//      .UseLogger(logger);

//  public IScriptContext UseArguments(params string[]? args)
//  {
//    _args = args;
//    return this;
//  }
//  public string? GetArgument(int index)
//  {
//    return _args?[index];
//  }

//  public IScriptContext UseLogger(ILogger? Logger)
//  {
//    _logger = Logger;
//    return this;
//  }
//  public void LogTrace(string arg) => _logger?.LogTrace(arg);
//  public void LogDebug(string arg) => _logger?.LogDebug(arg);
//  public void LogInfo(string arg) => _logger?.LogInformation(arg);
//  public void LogWarn(string arg) => _logger?.LogWarning(arg);
//  public void LogError(string arg) => _logger?.LogError(arg);
//}

//#endregion
