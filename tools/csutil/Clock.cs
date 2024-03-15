using System;
using System.Diagnostics;

namespace Trane.Submittals.Pipeline
{
  public class Clock
  {
    public static Clock Create()
    {
      Clock c = new Clock();
      c.Start();
      return c;
    }

    private readonly Stopwatch _watch;

    public Clock()
    {
      _watch = new Stopwatch();
    }

    public TimeSpan Elapsed => _watch.Elapsed;

    public Clock Start() { _watch.Start(); return this; }
    public Clock Stop() { _watch.Stop(); return this; }
    public Clock Reset() { _watch.Reset(); return this; }

    public override string ToString() => Elapsed.ToString();
  }
}
