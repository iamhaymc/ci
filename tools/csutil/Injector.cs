//using Microsoft.Extensions.DependencyInjection.Extensions;

//namespace My.Utility;

//public class Injector
//{
//  private IServiceProvider? _provider;
//  public IServiceCollection Collection { get; private set; }

//  public Injector() : this(new ServiceCollection()) { }
//  public Injector(IServiceCollection collection)
//    => Collection = collection ?? new ServiceCollection();

//  private void Invalidate()
//     => _provider = null;

//  private IServiceProvider Validate()
//     => _provider ??= Collection.BuildServiceProvider();

//  public Injector Edit(Action<IServiceCollection> edit)
//  {
//    Invalidate();
//    edit(Collection);
//    return this;
//  }
//  public Injector Add(Type type)
//  {
//    Invalidate();
//    Collection.TryAddSingleton(type);
//    return this;
//  }
//  public Injector Add(Type type, Type impl)
//  {
//    Invalidate();
//    Collection.TryAddSingleton(type, impl);
//    return this;
//  }
//  public Injector Add<T>() where T : class
//  {
//    Invalidate();
//    Collection.TryAddSingleton<T>();
//    return this;
//  }
//  public Injector Add<T, I>() where T : class where I : class, T
//  {
//    Invalidate();
//    Collection.TryAddSingleton<T, I>();
//    return this;
//  }
//  public Injector Add<T, I>(I inst) where T : class where I : class, T
//  {
//    Invalidate();
//    Collection.TryAddSingleton(inst);
//    return this;
//  }
//  public Injector Add<T, I>(Func<I> ctor) where T : class where I : class, T
//  {
//    Invalidate();
//    Collection.TryAddSingleton(ctor);
//    return this;
//  }
//  public Injector AddTransient(Type type)
//  {
//    Invalidate();
//    Collection.TryAddTransient(type);
//    return this;
//  }
//  public Injector AddTransient(Type type, Type impl)
//  {
//    Invalidate();
//    Collection.TryAddTransient(type, impl);
//    return this;
//  }
//  public Injector AddTransient<T>() where T : class
//  {
//    Invalidate();
//    Collection.TryAddTransient<T>();
//    return this;
//  }
//  public Injector AddTransient<T, I>() where T : class where I : class, T
//  {
//    Invalidate();
//    Collection.TryAddTransient<T, I>();
//    return this;
//  }

//  public object? Get(Type type)
//     => Validate().GetService(type);

//  public T? Get<T>() where T : class
//     => Validate().GetService<T>();
//}
