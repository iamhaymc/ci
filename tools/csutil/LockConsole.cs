using System;
using System.Collections.Generic;
using System.Linq;
using static Trane.Submittals.Pipeline.LockConsole;

namespace Trane.Submittals.Pipeline
{
  /// <summary>
  /// Write content to the console in a thread safe way
  /// </summary>
  public static class LockConsole
  {
    static object _lock = new object();

    public struct Message
    {
      public static Message From(string content, ConsoleColor? foreground = null, ConsoleColor? background = null)
        => new Message { Content = content, Foreground = foreground, Background = background };
      public static Message From(string content, bool lineReturn, bool lineBreak, ConsoleColor? foreground, ConsoleColor? background)
        => new Message { Content = content, LineReturn = lineReturn, LineBreak = lineBreak, Foreground = foreground, Background = background };

      public static Message Line(string content, ConsoleColor? foreground = null, ConsoleColor? background = null)
        => new Message { Content = content, LineReturn = false, LineBreak = true, Foreground = foreground, Background = background };
      public static Message Line(string content, bool lineReturn, ConsoleColor? foreground = null, ConsoleColor? background = null)
        => new Message { Content = content, LineReturn = lineReturn, LineBreak = true, Foreground = foreground, Background = background };

      public string Content { get; set; }
      public bool LineReturn { get; set; }
      public bool LineBreak { get; set; }
      public ConsoleColor? Foreground { get; set; }
      public ConsoleColor? Background { get; set; }
    }

    /// <summary>
    /// Write content to the console in a thread safe way
    /// </summary>
    public static void Write(params Message[] messages) => Write(messages.AsEnumerable());

    /// <summary>
    /// Write content to the console in a thread safe way
    /// </summary>
    public static void Write(IEnumerable<Message> messages)
    {
      lock (_lock)
      {
        ConsoleColor lastFg = Console.ForegroundColor;
        ConsoleColor lastBg = Console.BackgroundColor;
        try
        {
          foreach (var message in messages)
          {
            if (message.Content != null)
            {
              if (message.LineReturn || message.Foreground != null || message.Background != null)
              {

                if (message.Foreground != null)
                  Console.ForegroundColor = message.Foreground.Value;
                if (message.Background != null)
                  Console.BackgroundColor = message.Background.Value;
                if (message.LineReturn)
                  Console.CursorLeft = 0;
                if (!message.LineBreak)
                  Console.Write(message.Content);
                else
                  Console.WriteLine(message.Content);
              }
            }

          }
        }
        finally
        {
          Console.ForegroundColor = lastFg;
          Console.BackgroundColor = lastBg;
        }
      }
    }
  }
}
