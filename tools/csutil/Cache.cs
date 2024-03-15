//using Microsoft.Extensions.Caching.Memory;

//namespace My.Utility;

//public interface ICache
//{
//  public T? Get<T>(string key);
//  public bool TryGet<T>(string key, out T? value);
//  public T? Set<T>(string key, T value);
//  public T? Get<T>() where T : class;
//  public bool TryGet<T>(out T? value) where T : class;
//  public T Set<T>(T value) where T : class;
//}

//public class Cache : ICache
//{
//  private readonly IMemoryCache _memory;

//  public Cache(IMemoryCache memory)
//    => _memory = memory;

//  public T? Get<T>(string key)
//    => _memory.Get<T>(key);

//  public bool TryGet<T>(string key, out T? value)
//    => _memory.TryGetValue(key, out value);

//  public T? Set<T>(string key, T value)
//    => _memory.Get<T>(key);

//  public T? Get<T>() where T : class
//    => Get<T>(typeof(T).AssemblyQualifiedName);

//  public bool TryGet<T>(out T? value) where T : class
//    => TryGet(typeof(T).AssemblyQualifiedName, out value);

//  public T Set<T>(T value) where T : class
//    => Set<T>(typeof(T).AssemblyQualifiedName, value);
//}

//public static class InjectorCacheExtensions
//{
//  public static Injector AddCaches(this Injector self, Config config)
//     => self.Edit(x => x.AddMemoryCache())
//      .Add<ICache, Cache>();
//}
