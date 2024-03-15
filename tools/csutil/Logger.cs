using System;

namespace Trane.Submittals.Pipeline
{
  public interface ILogger
  {
    void Trace(string message, params object[] args);
    void Debug(string message, params object[] args);
    void Info(string message, params object[] args);
    void Warn(string message, params object[] args);
    void Error(string message, params object[] args);
    void Error(Exception exception, string message, params object[] args);
  }

  public enum LogLevel
  {
    Trace = 0,
    Debug,
    Info,
    Warning,
    Error
  }

  public class DebugLogger : ILogger
  {
    public const LogLevel DefaultLogLevel = LogLevel.Debug;

    public LogLevel LogLevel { get; set; }

    public DebugLogger(LogLevel logLevel = DefaultLogLevel)
    {
      LogLevel = logLevel;
    }

    public void Trace(string message, params object[] args)
    {
      if (LogLevel > LogLevel.Trace) return;
      System.Diagnostics.Debug.WriteLine($"Pipeline Trace: {message}");
    }

    public void Debug(string message, params object[] args)
    {
      if (LogLevel > LogLevel.Debug) return;
      System.Diagnostics.Debug.WriteLine($"Pipeline Debug: {message}");
    }

    public void Info(string message, params object[] args)
    {
      if (LogLevel > LogLevel.Info) return;
      System.Diagnostics.Debug.WriteLine($"Pipeline Debug: {message}");
    }

    public void Warn(string message, params object[] args)
    {
      if (LogLevel > LogLevel.Warning) return;
      System.Diagnostics.Debug.WriteLine($"Pipeline Warning: {message}");
    }

    public void Error(string message, params object[] args)
    {
      if (LogLevel > LogLevel.Error) return;
      System.Diagnostics.Debug.WriteLine($"Pipeline ERROR: {message}");
    }

    public void Error(Exception exception, string message, params object[] args)
    {
      if (LogLevel > LogLevel.Error) return;
      System.Diagnostics.Debug.WriteLine($"Pipeline ERROR: {message}");
    }
  }
}

//using Microsoft.Extensions.DependencyInjection.Extensions;
//using Microsoft.Extensions.Logging.Configuration;

//namespace My.Utility;

//public class LogSettings
//{
//  public LogLevel LogLevel { get; }
//  public int EventId { get; }
//}

//[ProviderAlias("StdLogger")]
//public class StdLoggerProvider : ILoggerProvider
//{
//  private LogSettings _settings;
//  private readonly IDisposable? _onSettings;
//  private readonly ConcurrentDictionary<string, StdLogger> _loggers =
//     new(StringComparer.OrdinalIgnoreCase);

//  public StdLoggerProvider(LogSettings settings)
//     => _settings = settings;

//  public StdLoggerProvider(IOptionsMonitor<LogSettings> options)
//  {
//    _settings = options.CurrentValue;
//    _onSettings = options.OnChange(updatedConfig
//       => _settings = updatedConfig);
//  }

//  private LogSettings GetSettings()
//     => _settings;

//  public ILogger CreateLogger(string categoryName)
//    => _loggers.GetOrAdd(categoryName, name
//       => new StdLogger(name, GetSettings));

//  public void Dispose()
//  {
//    _onSettings?.Dispose();
//    _loggers.Clear();
//  }
//}

//public class StdLogger : ILogger
//{
//  private string _name;
//  private Func<LogSettings> _cfgr;

//  public StdLogger(string name, Func<LogSettings> cfgr)
//    => (_name, _cfgr) = (name, cfgr);

//  public bool IsEnabled(LogLevel logLevel)
//     => _cfgr().LogLevel <= logLevel;

//  public IDisposable BeginScope<TState>(TState state)
//     where TState : notnull
//     => default!;

//  public void Log<TState>(
//    LogLevel logLevel, EventId eventId, TState state,
//    Exception? exception, Func<TState, Exception?, string> formatter)
//  {
//    if (IsEnabled(logLevel))
//    {
//      LogSettings cfg = _cfgr();

//      var message = formatter(state, exception);
//      ConsoleColor origBgColor = Console.BackgroundColor;
//      ConsoleColor origFgColor = Console.ForegroundColor;
//      switch (logLevel)
//      {
//        case LogLevel.Trace:
//          Console.ForegroundColor = ConsoleColor.DarkGray;
//          Console.Write($" - ");
//          Console.ForegroundColor = ConsoleColor.DarkGray;
//          Console.WriteLine(message);
//          break;
//        case LogLevel.Debug:
//          Console.ForegroundColor = ConsoleColor.Magenta;
//          Console.Write($" ~ ");
//          Console.ForegroundColor = ConsoleColor.Gray;
//          Console.WriteLine(message);
//          break;
//        case LogLevel.Information:
//          Console.ForegroundColor = ConsoleColor.Cyan;
//          Console.Write($"[?] ");
//          Console.ForegroundColor = ConsoleColor.White;
//          Console.WriteLine(message);
//          break;
//        case LogLevel.Warning:
//          Console.ForegroundColor = ConsoleColor.Cyan;
//          Console.Write($"[!] ");
//          Console.ForegroundColor = ConsoleColor.White;
//          Console.WriteLine(message);
//          break;
//        case LogLevel.Error:
//          Console.ForegroundColor = ConsoleColor.Red;
//          Console.Write($"[X] ");
//          Console.ForegroundColor = ConsoleColor.White;
//          Console.WriteLine(message);
//          break;
//        case LogLevel.Critical:
//          Console.ForegroundColor = ConsoleColor.Red;
//          Console.Write($"[X] ");
//          Console.ForegroundColor = ConsoleColor.White;
//          Console.WriteLine(message);
//          break;
//        case LogLevel.None:
//        default:
//          Console.WriteLine(message);
//          break;
//      }
//      Console.BackgroundColor = origBgColor;
//      Console.ForegroundColor = origFgColor;
//    }
//  }
//}

//public static class InjectorStdLoggerExtensions
//{
//  public static Injector AddLoggers(this Injector self)
//     => self.AddLoggers<StdLoggerProvider>();

//  public static Injector AddLoggers(this Injector self, Config config)
//     => self.AddLoggers<StdLoggerProvider>(config.Root);

//  public static Injector AddLoggers(this Injector self, Config config, string section)
//  => self.AddLoggers<StdLoggerProvider>(config.Root.GetSection(section));

//  public static Injector AddLoggers(this Injector self, IConfiguration? section = null)
//     => self.AddLoggers<StdLoggerProvider>(section);
//}

//public static class InjectorLoggerExtensions
//{
//  public static Injector AddLoggers<TLoggerProvider>(this Injector self)
//      where TLoggerProvider : class, ILoggerProvider
//    => self.AddLoggers();

//  public static Injector AddLoggers<TLoggerProvider>(this Injector self, Config config)
//       where TLoggerProvider : class, ILoggerProvider
//     => self.AddLoggers(config.Root);

//  public static Injector AddLoggers<TLoggerProvider>(this Injector self, Config config, string section)
//       where TLoggerProvider : class, ILoggerProvider
//  => self.AddLoggers(config.Root.GetSection(section));

//  public static Injector AddLoggers<TLoggerProvider>(this Injector self, IConfiguration? section = null)
//       where TLoggerProvider : class, ILoggerProvider
//     => self.Edit(c => c.AddLogging(b =>
//     {
//       b.ClearProviders();
//       if (section != null) b.AddConfiguration(section);
//       b.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, TLoggerProvider>());
//       LoggerProviderOptions.RegisterProviderOptions<LogSettings, TLoggerProvider>(b.Services);
//     }));

//  public static ILogger? GetLogger<T>(this Injector self)
//     => self.Get<ILogger<T>>();

//  public static ILogger? GetLogger(this Injector self)
//     => self.Get<ILogger>();
//}
