using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trane.Submittals.Pipeline
{
  public static class EnumerableExtensions
  {
    private static readonly Random _rng = new Random();

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> self)
    {
      return self.OrderBy(x => _rng.Next());
    }

    public static IEnumerable<IEnumerable<T>> Distribute<T>(this IEnumerable<T> self, int? partitions = null)
    {
      return self.Select((f, i) => new { f, i })
        .GroupBy(x => x.i % partitions).Select(x => x.Select(y => y.f));
    }

    public static Task<Task[]> WorkEach<T>(this IEnumerable<T> self, Action<T> action, int? workers = null)
    {
      return self.WorkEach<T, object>(order: null, action: action, workers: workers);
    }

    public static async Task<Task[]> WorkEach<T, OK>(this IEnumerable<T> self, Func<T, OK> order, Action<T> action, int? workers = null)
    {
      var batches = self.Shuffle().Distribute(workers ?? Environment.ProcessorCount);
      var tasks = batches.Select(batch => Task.Run(() =>
      {
        foreach (var item in order != null ? batch.OrderBy(order) : batch)
          action(item);
      })).ToArray();
      await Task.WhenAll(tasks);
      return tasks;
    }

    public static Task<Task[]> WorkEach<T>(this IEnumerable<T> self, Func<T, Task> action, int? workers = null)
    {
      return self.WorkEach<T, object>(order: null, action: action, workers: workers);
    }

    public static async Task<Task[]> WorkEach<T, OK>(this IEnumerable<T> self, Func<T, OK> order, Func<T, Task> action, int? workers = null)
    {
      var batches = self.Shuffle().Distribute(workers ?? Environment.ProcessorCount);
      var tasks = batches.Select(batch => Task.Run(async () =>
      {
        foreach (var item in order != null ? batch.OrderBy(order) : batch)
          await action(item);
      })).ToArray();
      await Task.WhenAll(tasks);
      return tasks;
    }
  }
}
