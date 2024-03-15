namespace Tool;

internal class ConversionLogger
{
  object _consoleLock = new object();

  public void LogDelete(string target)
  {
    LockConsole.Write(LockConsole.Message.Line($"[ Delete  ] ~ {target}", ConsoleColor.DarkGray));
  }
  public void LogIgnore(string target)
  {
    LockConsole.Write(LockConsole.Message.Line($"[ Ignore  ] ~ {target}", ConsoleColor.DarkGray));
  }
  public void LogConvert(string target)
  {
    LockConsole.Write(
      LockConsole.Message.From($"[ Convert ] | ", ConsoleColor.DarkCyan),
      LockConsole.Message.Line(target, ConsoleColor.DarkGray)
      );
  }
  public void LogSuccess(string target, TimeSpan duration)
  {
    LockConsole.Write(
      LockConsole.Message.From($"[ Success ] ", ConsoleColor.DarkGreen),
      LockConsole.Message.From($" ~ {target} ", ConsoleColor.DarkGray),
      LockConsole.Message.Line($"({Math.Round((double)duration.Seconds, 2)} secs)", ConsoleColor.DarkGray)
      );
  }
  public void LogFailure(string target, string description, TimeSpan duration)
  {
    LockConsole.Write(
      LockConsole.Message.From($"[ Failure ] ", ConsoleColor.Red),
      LockConsole.Message.From($" ~ {target} ", ConsoleColor.White),
      LockConsole.Message.Line($"({Math.Round((double)duration.Seconds, 2)} secs)", ConsoleColor.DarkGray)
      LockConsole.Message.From($" -> ", ConsoleColor.DarkGray),
      LockConsole.Message.Line(description, ConsoleColor.Gray)
      );
  }
}
